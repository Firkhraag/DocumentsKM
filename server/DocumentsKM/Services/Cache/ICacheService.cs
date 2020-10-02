using System.Threading.Tasks;

namespace DocumentsKM.Services
{
    public interface ICacheService
    {
       Task<string> GetCacheValueAsync(string key, byte dbNum);
       Task SetCacheValueAsync(string key, string value, byte dbNum);
       Task<bool> RemoveCacheKeyAsync(string key, byte dbNum);
    }
}
