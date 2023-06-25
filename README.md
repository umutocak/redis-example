# Redis Example
In this project, I have sampled how to use .net core and redis.
In order to use Redis, you must first download it to your computer.
After making the relevant installations, you should add the following library to your project:
```c#
Microsoft.Extensions.Caching.StackExchangeRedis,
Newtonsoft.Json
```
After these libraries are installed, the system will work. Let's take a look at the code now.
```c#
public interface IDistributedCacheManager
    {
        byte[] Get(string key);
        T Get<T>(string key);
        void Set(string key, object value);
        void Refresh(string key);
        bool Any(string key);
        void Remove(string key);
    }
```
The code above is an interface we have created to use redis cached. Let's go a little deeper and examine the contents of these methods.
```c#
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
```
The above code allows us to add the data to the redis side. It records with the key and object that we will pass here.
```c#
options: new DistributedCacheEntryOptions
            {
                //Determines how long the data stays on redis
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                //Refreshes the remaining time each time a request is fired.
                SlidingExpiration = TimeSpan.FromSeconds(30)
            }
```
In this section, we set how often our data will be deleted and under what conditions.
```c#
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
```
Here, we perform data fetching according to the key information that we have transmitted to the redis side. We convert the incoming data as a T object and present it to the user. The codes in Redis are generally like this. You can find more information by examining it in detail. Without forgetting, the following definition must be made on startup.cs for Redis to work:
```c#
builder.Services.AddScoped<RedisClient>();
builder.Services.AddScoped<IDistributedCacheManager, RedisCacheManager>();
```
