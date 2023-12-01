using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebAPIApplication.Services.Auth;
using WebAPIApplication.Services.Auth.UserService;
using WebAPIApplication.Services.IServices.Auth;
using WebAPIApplication.Services.IServices.Auth.IUserService;

namespace WebAPIApplication.ServiceCollection.AuthService;

public static class AuthDependencyInjection
{
    /// <summary>
    ///     身份验证相关的配置与依赖注入
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddAuthService(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection("JwtService");
        JwtServiceOptions.ValidAudience =
            config.GetValue<string>("ValidAudience") ?? throw new InvalidOperationException();
        JwtServiceOptions.Secret = config.GetValue<string>("Secret") ?? throw new InvalidOperationException();
        JwtServiceOptions.ValidIssuer = config.GetValue<string>("ValidIssuer") ?? throw new InvalidOperationException();

        services.AddSingleton<IJwtService, JwtService>();
        services.AddScoped<IUserService, UserService>();
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var secretBytes = Encoding.UTF8.GetBytes(JwtServiceOptions.Secret);
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = JwtServiceOptions.ValidIssuer,
                    ValidAudience = JwtServiceOptions.ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(secretBytes)
                };
            });

        return services;
    }
}