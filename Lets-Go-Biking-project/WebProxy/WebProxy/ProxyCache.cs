using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace WebProxy
{
    public class ProxyCache<T> where T : new()
    {
        protected ObjectCache allCache;
        protected DateTimeOffset myDate;

        public ProxyCache()
        {
            allCache = MemoryCache.Default;
            myDate = ObjectCache.InfiniteAbsoluteExpiration;
        }

        public T Get(string url, double time)
        {
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(time),
            };

            if (!allCache.Contains(url))
            {
                T data = (T)Activator.CreateInstance(typeof(T), url);
                allCache.Add(url, data, policy);
                return data;
            }
            return (T)allCache.Get(url);
        }
    }
}
