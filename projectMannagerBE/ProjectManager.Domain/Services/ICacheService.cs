namespace ProjectManager.Domain.Services;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken ct = default);
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken ct = default);
    Task RemoveAsync(string key, CancellationToken ct = default);
    Task<long> IncreaseVersionAsync(string versionKey, CancellationToken ct = default);
    Task<long> GetVersionAsync(string versionKey, CancellationToken ct = default);
}