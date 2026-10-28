using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using ProjectManager.Domain.Services;

namespace ProjectManager.EntityFrameworkCore;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
    {
        var json = await _cache.GetStringAsync(key, ct);
        return string.IsNullOrEmpty(json) ? default : JsonSerializer.Deserialize<T>(json, JsonOptions);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken ct = default)
    {
        var json = JsonSerializer.Serialize(value, JsonOptions);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(10)
        };
        return _cache.SetStringAsync(key, json, options, ct);
    }

    public Task RemoveAsync(string key, CancellationToken ct = default)
        => _cache.RemoveAsync(key, ct);

    public async Task<long> IncreaseVersionAsync(string versionKey, CancellationToken ct = default)
    {
        var current = await GetVersionAsync(versionKey, ct);
        var next = current + 1;
        await _cache.SetStringAsync(versionKey, next.ToString(), ct);
        return next;
    }

    public async Task<long> GetVersionAsync(string versionKey, CancellationToken ct = default)
    {
        var val = await _cache.GetStringAsync(versionKey, ct);
        return long.TryParse(val, out var v) ? v : 0;
    }
}