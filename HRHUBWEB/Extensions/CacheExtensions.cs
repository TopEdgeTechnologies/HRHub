using Microsoft.Extensions.Caching.Memory;


namespace HRHUBWEB.Extensions
{
    public class CacheExtensions
    {
      private readonly IMemoryCache _memoryCache;


        public CacheExtensions(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void SetObject<T>(string key, T value)
        {
            // Store the object in the cache with the specified key
            _memoryCache.Set(key, value);
        }

        public T GetObject<T>(string key)
        {
            // Retrieve the object from the cache using the specified key
            if (_memoryCache.TryGetValue(key, out T value))
            {
                // Object found in the cache
                return value;
            }

            // Object not found in the cache
            // Perform logic to fetch the object and set it in the cache if needed

            return default(T); // Object not found
        }


    }
}
