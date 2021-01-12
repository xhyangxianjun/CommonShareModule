using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTDDJYDS.CommonModule
{
    public class ServiceStackRedis
    {
        private readonly int _expirySeconds = -1;
        //使用客户端链接池模式提升链接速度
        private readonly PooledRedisClientManager _redisClientManager;
        //只使用长链接
        //protected static RedisClient Redis = new RedisClient("127.0.0.1", 6379);
        public ServiceStackRedis(string host, int port, string password, int expirySeconds, long db)
        {
            _expirySeconds = expirySeconds;
            var hosts = new[] { string.Format("{0}@{1}:{2}", password, host, port) };
            _redisClientManager = new PooledRedisClientManager(hosts, hosts, null, db, 500, _expirySeconds);
        }

        public ServiceStackRedis(string host)
            : this(host, 6379, null, -1, 0)
        {
        }

        public ServiceStackRedis()
            : this("localhost", 6379, null, -1, 0)
        {
        }

        public bool Set<T>(string key, T value)
        {
            if (key == null) throw new ArgumentNullException("key");

            if (_expirySeconds != -1) 
                return Set(key, value, _expirySeconds);

            
            using (var client = _redisClientManager.GetClient())
            {
                return client.Set<T>(key, value);
            }
        }

        public bool Set<T>(string key, T value, int duration)
        {
            if (key == null) throw new ArgumentNullException("key");

            using (var client = _redisClientManager.GetClient())
            {
                return client.Set<T>(key, value, DateTime.Now.AddSeconds(duration));
            }
        }

        public T Get<T>(string key)
        {
            if (key == null) throw new ArgumentNullException("key");

            T data;
            using (var client = _redisClientManager.GetClient())
            {
                data = client.Get<T>(key);
            }
            return data == null ? default(T) : data;
        }
        public bool Remove(string key)
        {
            using (var client = _redisClientManager.GetClient())
            {
                return client.Remove(key);
            }
        }

        public bool RemoveAll()
        {
            using (var client = _redisClientManager.GetClient())
            {
                try
                {
                    client.FlushDb();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public bool ContainsKey(string key)
        {
            using (var client = _redisClientManager.GetClient())
            {
                return client.ContainsKey(key);
            }
        }

        public List<string> GetAllKeys()
        {
            using (var client = _redisClientManager.GetClient())
            {
                return client.SearchKeys("Chaint.*");
            }
        }
    }
}
