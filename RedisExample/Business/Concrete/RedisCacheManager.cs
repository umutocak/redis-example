using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RedisExample.Business.Abstract;
using StackExchange.Redis;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RedisExample.Business.Concrete
{
    public class RedisCacheManager : IDistributedCacheManager
    {
        private readonly RedisClient _redisClient;

        public RedisCacheManager(RedisClient redisServer)
        {
            this._redisClient = redisServer;
        }

        public T Get<T>(string key)
        {
            var utf8String = Encoding.UTF8.GetString(Get(key));
            var result = JsonConvert.DeserializeObject<T>(utf8String);
            return result;
        }

        public byte[] Get(string key)
        {
            return _redisClient.RedisCache.Get(key);
        }

        public void Set(string key, object value)
        {
            var serializedObject = JsonConvert.SerializeObject(value);
            _redisClient.RedisCache.SetString(key, serializedObject, options: new DistributedCacheEntryOptions
            {
                //Determines how long the data stays on redis
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                //Refreshes the remaining time each time a request is fired.
                SlidingExpiration = TimeSpan.FromSeconds(30)
            });
        }

        public void Refresh(string key)
        {
            _redisClient.RedisCache.Refresh(key);
        }

        public bool Any(string key)
        {
            return _redisClient.RedisCache.Get(key) != null;
        }

        public void Remove(string key)
        {
            _redisClient.RedisCache.Remove(key);
        }
    }
}
