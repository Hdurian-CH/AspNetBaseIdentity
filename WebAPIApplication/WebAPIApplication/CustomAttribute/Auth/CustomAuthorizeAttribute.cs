using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPIApplication.Services.IServices.Cache;

namespace WebAPIApplication.CustomAttribute.Auth;

/// <summary>
///     自定义的授权注解，使用时需置于[Authorize]之下，因为此处并不校验token合法性，仅校验该token过期与否
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    /// <inheritdoc />
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
            return;
        var userId = context.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.UserData).Value;
        var memoryCacheService =
            context.HttpContext.RequestServices.GetKeyedService<ICacheService>("AspMemoryCache") ??
            throw new NullReferenceException();
        if (memoryCacheService.check($"UserTokenInfo:{userId}")) return;
        context.Result = new UnauthorizedResult();
    }
}