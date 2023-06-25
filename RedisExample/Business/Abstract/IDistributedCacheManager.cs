namespace RedisExample.Business.Abstract
{
    public interface IDistributedCacheManager
    {
        byte[] Get(string key);
        T Get<T>(string key);
        void Set(string key, object value);
        void Refresh(string key);
        bool Any(string key);
        void Remove(string key);
    }
}
