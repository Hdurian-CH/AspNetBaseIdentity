using Microsoft.EntityFrameworkCore;
using WebAPIApplication.DbContext.Auth;

namespace WebAPIApplication.ServiceCollection.DbContext;

public static class AuthDbContextInject
{
    /// <summary>
    ///     AuthDbContext相关的依赖注入
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddAuthDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthDbContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("Connection"))
        );
        return services;
    }
}