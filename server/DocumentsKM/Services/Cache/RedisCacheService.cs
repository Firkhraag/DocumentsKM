using System;
using System.Threading.Tasks;
using DocumentsKM.Helpers;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace DocumentsKM.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly AppSettings _appSettings;

        public RedisCacheService(
            IConnectionMultiplexer connectionMultiplexer,
            IOptions<AppSettings> appSettings)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _appSettings = appSettings.Value;
        }

        public async Task<string> GetCacheValueAsync(string key, byte dbNum)
        {
            var db = _connectionMultiplexer.GetDatabase(dbNum);
            return await db.StringGetAsync(key);
        }

        public async Task SetCacheValueAsync(string key, string value, byte dbNum)
        {
            var db = _connectionMultiplexer.GetDatabase(dbNum);
            var expires = TimeSpan.FromDays(_appSettings.RefreshTokenExpireTimeInDays);
            await db.StringSetAsync(key, value, expires);
        }

        public async Task<bool> RemoveCacheKeyAsync(string key, byte dbNum)
        {
            var db = _connectionMultiplexer.GetDatabase(dbNum);
            return await db.KeyDeleteAsync(key);
        }
    }
}
