using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DTO.Request.AuthController;
using DTO.Response;
using DTO.Response.Auth;
using DTO.Response.Error;
using DTO.Response.Secret;
using Masuit.Tools.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPIApplication.CustomAttribute.Auth;
using WebAPIApplication.ErrorMessageDefine.Auth;
using WebAPIApplication.Services.Auth.Secret;
using WebAPIApplication.Services.IServices.Auth;
using WebAPIApplication.Services.IServices.Auth.IUserService;
using WebAPIApplication.Services.IServices.Cache;
using ZstdSharp.Unsafe;

namespace WebAPIApplication.Controllers.Auth;

/// <summary>
///     用于身份验证的Controller
/// </summary>
[ApiController]
[Route("[controller]/[action]")]
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
    [HttpGet]
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
    [HttpPost]
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
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var password = RsaService.RsaDecrypt(loginRequest.Password);
        if (password is not null)
        {
            if (password.Length is < 8 or > 16)
                return BadRequest(new BaseErrorResponse(new[] { AuthErrorMessages.PasswordLengthError }));
            var res = await UserService.CheckUserByUserNameAndPasswordHash(loginRequest.UserName, password.SHA256());
            return res.Match<IActionResult>(
                normal =>
                {
                    var authClaims = new List<Claim>
                    {
                        new(ClaimTypes.Name, loginRequest.UserName),
                        new(ClaimTypes.UserData, normal.Item2)
                    };
                    var token = new JwtSecurityTokenHandler().WriteToken(JwtService.GetToken(authClaims));
                    MemoryCacheService.Set($"UserTokenInfo:{normal.Item2}", token, DateTime.Now.AddHours(3));
                    var response = new LoginResponse(token:token);
                    return Ok(response);
                },
                error =>
                {
                    var response = new BaseErrorResponse(new []{error.Item2});
                    return BadRequest(response);
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
    [CustomAuthorize]
    [HttpPost]
    public IActionResult CheckToken()
    {
        var idClaim = Request.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.UserData) ??
                      throw new NullReferenceException();
        var nameClaim = Request.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name) ?? 
                        throw new NullReferenceException();
        var userId = idClaim.Value;
        var userName = nameClaim.Value;
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, userName),
            new(ClaimTypes.UserData, userId)
        };
        var token = JwtService.GetToken(authClaims);
        MemoryCacheService.Set($"UserTokenInfo:{userId}", token, DateTime.Now.AddHours(3));
        return Ok(new CheckTokenResponse(new JwtSecurityTokenHandler().WriteToken(token)));
    }

    #region Field

    private IJwtService JwtService { get; }
    private IUserService UserService { get; }
    private ICacheService MemoryCacheService { get; }

    #endregion
}