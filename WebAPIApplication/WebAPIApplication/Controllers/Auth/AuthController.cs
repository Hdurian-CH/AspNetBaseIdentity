using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DTO.Request.AuthController;
using DTO.Response;
using DTO.Response.Auth;
using DTO.Response.Error;
using DTO.Response.Secret;
using Masuit.Tools.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIApplication.ErrorMessageDefine.Auth;
using WebAPIApplication.Services.Auth.Secret;
using WebAPIApplication.Services.IServices.Auth;
using WebAPIApplication.Services.IServices.Auth.IUserService;
using WebAPIApplication.Services.IServices.Cache;

namespace WebAPIApplication.Controllers.Auth;

/// <summary>
///     用于身份验证的Controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    /// <inheritdoc />
    public AuthController(IJwtService jwtService, IUserService userService,
        [FromKeyedServices("AspMemoryCache")] ICacheService aspMemoryCacheService)
    {
        JwtService = jwtService;
        UserService = userService;
        MemoryCacheService = aspMemoryCacheService;
    }

    /// <summary>
    ///     获取RSA公钥
    /// </summary>
    /// <returns></returns>
    [HttpGet("[action]")]
    [AllowAnonymous]
    public IActionResult GetPublicKey()
    {
        return Ok(new PublicKeyResponse(RsaServiceOption.PublicKey));
    }

    /// <summary>
    ///     开发环境使用的计算RSA公钥加密密码
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    [HttpPost("[action]")]
    [AllowAnonymous]
    public IActionResult CalculatePassword(string password)
    {
        return Ok(RsaService.RsaEncrypt(password));
    }

    /// <summary>
    ///     用户登录
    /// </summary>
    /// <param name="loginRequest"></param>
    /// <returns></returns>
    [HttpPost("[action]")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var password = RsaService.RsaDecrypt(loginRequest.Password);
        if (password is not null)
        {
            var res = await UserService.CheckUserByUserNameAndPasswordHash(loginRequest.UserName, password.SHA256());
            return res.Match<IActionResult>(
                normal =>
                {
                    var authClaims = new List<Claim>
                    {
                        new(ClaimTypes.Name, loginRequest.UserName),
                        new(ClaimTypes.UserData, normal.Item2)
                    };
                    var token = JwtService.GetToken(authClaims);
                    MemoryCacheService.Set($"UserTokenInfo:{normal.Item2}", token, DateTime.Now.AddHours(3));
                    var response = new LoginResponse(token: new JwtSecurityTokenHandler().WriteToken(token));
                    return Ok(response);
                },
                error =>
                {
                    var response = new LoginResponse(code: 500, message: error.Item2);
                    return Ok(response);
                }
            );
        }

        var errorModel = new BaseErrorResponse(new[] { AuthErrorMessages.PassWordError });
        return BadRequest(errorModel);
    }

    /// <summary>
    ///     token检查端点
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpPost("[action]")]
    public IActionResult CheckToken()
    {
        var idClaim = Request.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.UserData) ??
                      throw new NullReferenceException();
        var userId = idClaim.Value;
        var result = MemoryCacheService.check($"UserTokenInfo:{userId}");
        return Ok(result ? new BaseResponse() : new BaseResponse(500, "token 过期"));
    }

    #region Field

    private IJwtService JwtService { get; }
    private IUserService UserService { get; }
    private ICacheService MemoryCacheService { get; }

    #endregion
}