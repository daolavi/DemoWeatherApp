using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace WeatherService.Api.Extensions;

public static class CacheExtensions
{
    public static async Task StoreObjectAsync<T>(this IDistributedCache cache, 
        string key, T value, TimeSpan? absoluteExpiration = null, CancellationToken token = default)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpiration ?? TimeSpan.FromDays(1) // Default expiration
        };
        
        var serializedValue = JsonSerializer.Serialize(value);
        
        await cache.SetStringAsync(key, serializedValue, options, token);
    }
    
    public static async Task<T?> GetObjectAsync<T>(this IDistributedCache cache, string key, CancellationToken token = default)
    {
        var serializedValue = await cache.GetStringAsync(key, token);
        
        if (serializedValue == null)
            return default;
        
        return JsonSerializer.Deserialize<T>(serializedValue);
    }
}