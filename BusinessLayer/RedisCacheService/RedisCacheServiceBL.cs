using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace BusinessLayer.RedisCacheService
{
    class RedisCacheServiceBL
    {
        private readonly IDistributedCache distributedCache;

        public RedisCacheServiceBL(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        public async Task RemoveNotesRedisCache(long UserID)
        {
            var cacheKey = "ActiveNotes:" + UserID.ToString();
            await distributedCache.RemoveAsync(cacheKey);
            cacheKey = "ArchiveNotes:" + UserID.ToString();
            await distributedCache.RemoveAsync(cacheKey);
            cacheKey = "ReminderNotes:" + UserID.ToString();
            await distributedCache.RemoveAsync(cacheKey);
            cacheKey = "LabelNotes:" + UserID.ToString();
            await distributedCache.RemoveAsync(cacheKey);
        }
        public async Task AddRedisCache(string cacheKey, object obj)
        {
            string serializedNotes = JsonConvert.SerializeObject(obj);
            var redisNoteCollection = Encoding.UTF8.GetBytes(serializedNotes);
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));
            await distributedCache.SetAsync(cacheKey, redisNoteCollection, options);
        }
    }
}
