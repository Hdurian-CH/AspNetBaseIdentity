using DTO.Request.AccountController;
using DTO.Response.Error;
using Masuit.Tools.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIApplication.ErrorMessageDefine.Auth;
using WebAPIApplication.ErrorMessageDefine.DatabaseError;
using WebAPIApplication.Model.Auth;
using WebAPIApplication.Services.Auth.Secret;
using WebAPIApplication.Services.IServices.Auth.IUserService;

namespace WebAPIApplication.Controllers.Account;

/// <summary>
///     账号管理Controller
/// </summary>
[ApiController]
[Route("[controller]/[action]")]
public class AccountController : ControllerBase
{
    /// <inheritdoc />
    public AccountController(IUserService userService)
    {
        UserService = userService;
    }

    private IUserService UserService { get; }

    /// <summary>
    ///     创建用户
    /// </summary>
    /// <param name="accountRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateAccount(CreateAccountRequest accountRequest)
    {
        var password = RsaService.RsaDecrypt(accountRequest.Password);
        if (password is null) return BadRequest(new BaseErrorResponse(new[] { AuthErrorMessages.PassWordError }));
        if (password.Length is < 8 or > 16)
            return BadRequest(new BaseErrorResponse(new[] { AuthErrorMessages.PasswordLengthError }));
        var passwordSha256 = password.SHA256();
        var newUser = new User(accountRequest.UserName, passwordSha256, accountRequest.Email);
        var res = await UserService.CreateUser(newUser);
        return res.Match<IActionResult>(
            _ => Ok(),
            error => BadRequest(new BaseErrorResponse(new[] { BaseDatabaseErrorMessage.DatabaseSaveError }))
        );
    }
}