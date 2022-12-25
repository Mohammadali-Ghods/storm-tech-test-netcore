
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Todo.Interface;

namespace Services
{
    public class CacheManagement<T> : ICacheManagement<T>
    {

        private static readonly SemaphoreSlim GetSemaphore = new SemaphoreSlim(1, 1);
        private readonly IMemoryCache _cache;
        public CacheManagement(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }
        public async Task<IEnumerable<T>> GetListCache(string cacheKey)
        {
            return await GetListCache(cacheKey, GetSemaphore);
        }
        public async Task<T> GetSingleCache(string cacheKey)
        {
            return await GetSingleCache(cacheKey, GetSemaphore);
        }
        private async Task<IEnumerable<T>> GetListCache(string cacheKey, SemaphoreSlim semaphore)
        {
            if (_cache.TryGetValue(cacheKey, out IEnumerable<T> t))
            {
                return t;
            }
            else
            {
                try
                {
                    await semaphore.WaitAsync();

                    if (_cache.TryGetValue(cacheKey, out t))
                    {
                        return t;
                    }
                    else
                    {
                        return null;
                    }
                }
                finally
                {
                    semaphore.Release();
                }
            }
        }
        private async Task<T> GetSingleCache(string cacheKey, SemaphoreSlim semaphore)
        {
            if (_cache.TryGetValue(cacheKey, out T t))
            {
                return t;
            }
            else
            {
                try
                {
                    await semaphore.WaitAsync();

                    if (_cache.TryGetValue(cacheKey, out t))
                    {
                        return t;
                    }
                    else
                    {
                        return default;
                    }
                }
                finally
                {
                    semaphore.Release();
                }
            }
        }
        public void SetListCache(string cacheKey, List<T> t, int minutes)
        {
            if (minutes <= 2) minutes = 3;
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                              .SetSlidingExpiration(TimeSpan.FromMinutes(minutes))
                              .SetAbsoluteExpiration(TimeSpan.FromMinutes(minutes - 2))
                              .SetPriority(CacheItemPriority.Normal);
            _cache.Set(cacheKey, t, cacheEntryOptions);
        }
        public void SetSingleCache(string cacheKey, T t, int minutes)
        {
            if (minutes <= 2) minutes = 3;
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                              .SetSlidingExpiration(TimeSpan.FromMinutes(minutes))
                              .SetAbsoluteExpiration(TimeSpan.FromMinutes(minutes - 2))
                              .SetPriority(CacheItemPriority.Normal);
            _cache.Set(cacheKey, t, cacheEntryOptions);
        }
    }
}