using Microsoft.Extensions.Caching.Memory;
using WebAPIApplication.Services.IServices.Cache;

namespace WebAPIApplication.Services.Cache;

/// <summary>
///     封装Asp.Net Core MemoryCache
/// </summary>
public class AspMemoryCacheService(IMemoryCache cacheService) : ICacheService
{
    /// <summary>
    ///     Cache Set
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="lastTime"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool Set<T, TValue>(T key, TValue value, DateTime lastTime)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = lastTime - DateTime.Now
        };
        try
        {
            cacheService.Set(key ?? throw new ArgumentNullException(nameof(key)), value, cacheEntryOptions);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Set MemoryCache Error");
            return false;
        }
    }

    /// <summary>
    ///     Cache Get
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T? Get<T>(object key)
    {
        cacheService.TryGetValue(key, out T? result);
        return result;
    }

    /// <summary>
    ///     检查缓存是否过期
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool check(object key)
    {
        return cacheService.TryGetValue(key, out _);
    }
}