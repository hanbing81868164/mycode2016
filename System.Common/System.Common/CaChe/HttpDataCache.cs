
using System.Web;
using System.Collections;
using System.Web.Caching;

namespace System
{
    /// <summary>
    /// 缓存相关的操作类
    /// </summary>
    public class HttpDataCache
    {
        //private static int Time = 1440;//缓存一天时间 int.Parse(ConfigurationManager.AppSettings["Time"].ToString());
        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <returns></returns>
        public static object GetCache(string CacheKey)
        {
            return HttpRuntime.Cache[CacheKey];
        }

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        public static void SetCache(string name, object value, int Time = 1440)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(name, value, null, DateTime.MaxValue, TimeSpan.FromMinutes(Time));//15分钟如果没有访问则自动清除
        }

        public static void SetCacheBySql(string key, object value, AggregateCacheDependency acd, int Time = 1440)
        {
            HttpRuntime.Cache.Add(key, value, acd, DateTime.Now.AddMinutes(Time), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
        }
        /// <summary>
        /// 删除一个缓存
        /// </summary>
        /// <param name="CacheKey">缓存名称</param>
        public static void Remove(string name)
        {
            object objType = GetCache(name);//从缓存读取
            if (objType != null)
            {
                try
                {
                    System.Web.Caching.Cache objCache = HttpRuntime.Cache;
                    objCache.Remove(name);//删除一个缓存项
                }
                catch { }
            }
        }

        /// <summary>
        /// 判断某个缓存是否存在,不存在返回true,存在返回false
        /// </summary>
        public static bool IsNull(string name)
        {
            object objType = HttpDataCache.GetCache(name);//从缓存读取
            if (objType == null)
            {
                return true;//不存在
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 清除所有的HttpRuntime数据缓存
        /// </summary>
        public static void RemoveAll()
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            IDictionaryEnumerator em = objCache.GetEnumerator();

            while (em.MoveNext())
            {
                objCache.Remove(em.Key.ToString());
            }
            em = null;
            objCache = null;
        }

    }
}

