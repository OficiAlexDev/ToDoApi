using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Text.Json.Nodes;

namespace ToDo.Services
{
    public class Redis(IDistributedCache cache)
    {
        /// <summary>
        /// Define cache value by key and expire options
        /// </summary>
        /// <param name="key">Cache  key</param>
        /// <param name="value">Object to set in cache</param>
        /// <returns></returns>
        public async Task SetCache(string key, object value)
        {
            await cache.SetStringAsync(key, value.ToJson(), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60 * 5), SlidingExpiration = TimeSpan.FromSeconds(60 * 3) });
        }
        /// <summary>
        /// Get cache by key
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="key">Cache key</param>
        /// <returns></returns>
        public async Task<T?> GetCache<T>(string key)
        {
            string? value = await cache.GetStringAsync(key);
            if (value != null)
            {
                JsonNode.Parse(value.ToJson());
                return JsonConvert.DeserializeObject<T>(JsonNode.Parse(value.ToJson())!.ToString());
            }
            return default;
        }
    }
}
