using WebAPIApplication.Services.Cache;
using WebAPIApplication.Services.IServices.Cache;

namespace WebAPIApplication.ServiceCollection.Cache;

public static class CacheDependencyInjection
{
    /// <summary>
    ///     Cache 相关的依赖注入
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddCacheService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.AddKeyedScoped<ICacheService, AspMemoryCacheService>("AspMemoryCache");
        return services;
    }
}