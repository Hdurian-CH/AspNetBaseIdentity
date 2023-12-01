namespace WebAPIApplication.Services.IServices.Cache;

/// <summary>
///     Cache操作的泛型抽象
/// </summary>
public interface ICacheService
{
    public bool Set<T, TValue>(T key, TValue value, DateTime lastTime);
    public T? Get<T>(object key);

    public bool check(object key);
}