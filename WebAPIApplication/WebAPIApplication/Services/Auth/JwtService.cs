using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebAPIApplication.Services.IServices.Auth;

namespace WebAPIApplication.Services.Auth;

public class JwtService : IJwtService
{
    /// <summary>
    ///     生成密钥
    /// </summary>
    private byte[] SecretBytes => Encoding.UTF8.GetBytes(JwtServiceOptions.Secret);

    /// <summary>
    ///     JWT的生成
    /// </summary>
    /// <param name="claims">传入的与账户有关的Claim，用于放入JWT中</param>
    /// <returns></returns>
    public JwtSecurityToken GetToken(IEnumerable<Claim> claims)
    {
        var authSigningKey = new SymmetricSecurityKey(SecretBytes);

        return new JwtSecurityToken(
            JwtServiceOptions.ValidIssuer,
            JwtServiceOptions.ValidAudience,
            claims,
            expires: DateTime.Now.AddHours(12),
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
    }
}