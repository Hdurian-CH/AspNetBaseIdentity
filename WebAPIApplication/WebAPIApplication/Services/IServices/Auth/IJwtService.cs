using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebAPIApplication.Services.IServices.Auth;

public interface IJwtService
{
    public JwtSecurityToken GetToken(IEnumerable<Claim> claims);
}