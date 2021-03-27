using Faraboom.Framework.DataAnnotation;

using Microsoft.Extensions.Caching.Memory;

using System;
using System.Threading.Tasks;

namespace Faraboom.Framework.Caching
{
    [ServiceLifetime(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton)]
    public class CacheProvider : ICacheProvider
    {
        private readonly IMemoryCache memoryCache;
        public CacheProvider(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public async Task<TItem> GetAsync<TItem, TKey>(TKey key, Func<ICacheEntry, Task<TItem>> factory = null, string tenant = null)
            where TKey : struct
        {
            return await GetAsync(key.ToString(), factory, tenant);
        }
        public async Task<TItem> GetAsync<TItem>(string key, Func<ICacheEntry, Task<TItem>> factory = null, string tenant = null)
        {
            return factory == null ?
                memoryCache.Get<TItem>($"{tenant}_{key}")
                : await memoryCache.GetOrCreateAsync<TItem>(generateKey(key, tenant), factory);
        }

        public TItem Get<TItem, TKey>(TKey key, Func<ICacheEntry, TItem> factory = null, string tenant = null)
            where TKey : struct
        {
            return Get(key.ToString(), factory, tenant);
        }
        public TItem Get<TItem>(string key, Func<ICacheEntry, TItem> factory, string tenant = null)
        {
            return factory == null ?
                memoryCache.Get<TItem>(generateKey(key, tenant))
                : memoryCache.GetOrCreate<TItem>(generateKey(key, tenant), factory);
        }

        public async Task RemoveAsync<TKey>(TKey key, string tenant = null)
            where TKey : struct
        {
            await RemoveAsync(key.ToString(), tenant);
        }
        public async Task RemoveAsync(string key, string tenant = null)
        {
            await Task.Run(() => Remove(key, tenant));
        }

        public void Remove<TKey>(TKey key, string tenant = null)
            where TKey : struct
        {
            Remove(key.ToString(), tenant);
        }
        public void Remove(string key, string tenant = null)
        {
            memoryCache.Remove(generateKey(key, tenant));
        }

        public async Task<TItem> SetAsync<TItem, TKey>(TKey key, TItem value, string tenant = null, TimeSpan? SlidingExpiration = null, MemoryCacheEntryOptions options = null)
            where TKey : struct
        {
            return await SetAsync(key.ToString(), value, tenant, SlidingExpiration, options);
        }
        public async Task<TItem> SetAsync<TItem>(string key, TItem value, string tenant = null, TimeSpan? SlidingExpiration = null, MemoryCacheEntryOptions options = null)
        {
            return await Task.FromResult(Set(key, value, tenant, SlidingExpiration, options));
        }

        public TItem Set<TItem, TKey>(TKey key, TItem value, string tenant = null, TimeSpan? SlidingExpiration = null, MemoryCacheEntryOptions options = null)
            where TKey : struct
        {
            return Set(key.ToString(), value, tenant, SlidingExpiration, options);
        }
        public TItem Set<TItem>(string key, TItem value, string tenant = null, TimeSpan? SlidingExpiration = null, MemoryCacheEntryOptions options = null)
        {
            if (options != null)
            {
                if (SlidingExpiration.HasValue)
                    options.SlidingExpiration = SlidingExpiration;

                return memoryCache.Set(generateKey(key, tenant), value, options);
            }

            if (SlidingExpiration.HasValue)
                return memoryCache.Set(generateKey(key, tenant), value, new MemoryCacheEntryOptions { SlidingExpiration = SlidingExpiration });

            return memoryCache.Set(generateKey(key, tenant), value);
        }

        private string generateKey(string key, string tenant = null)
        {
            return $"{tenant ?? ""}_{key}";
        }
    }
}