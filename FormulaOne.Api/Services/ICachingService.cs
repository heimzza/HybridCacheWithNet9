namespace FormulaOne.Api.Services;

public interface ICachingService
{
    T Get<T>(string key);
    void Set<T>(string key, T value, TimeSpan expiration);
    void Remove(string key);
}