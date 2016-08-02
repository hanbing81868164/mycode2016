using System.Collections.Generic;
using System.Runtime.Caching;

namespace System
{
    public class ObjectCacheMonitor<T> where T : class ,new()
    {
        private string cacheKey = Guid.NewGuid().ToString();
        Func<object, T> _cacheManage = null;


        //1、获取内存缓存对象
        ObjectCache cache = MemoryCache.Default;
        List<string> filePaths = new List<string>();
        HostFileChangeMonitor monitor = null;

        public ObjectCacheMonitor(string filePath, Func<object, T> cacheManage)
        {
            _cacheManage = cacheManage;

            //7、监视文件需要传入一个IList对象，所以即便只有一个文件也需要新建List对象
            filePaths.Add(filePath);
        }

        public bool Remove()
        {
            if (cache.Contains(cacheKey))
            {
                cache.Remove(cacheKey);
            }
            return true;
        }

        public T GetCache()
        {
            T fileContents = default(T);

            //2、通过Key判断缓存中是否已有词典内容（Key在存入缓存时设置）
            if (cache.Contains(cacheKey))
            {
                //3、直接从缓存中读取词典内容
                fileContents = cache.GetCacheItem(cacheKey).Value as T;
            }
            else
            {
                try
                {
                    //3、读取配置文件，组成词典对象，准备放到缓存中
                    fileContents = _cacheManage(filePaths);
                }
                catch (Exception ex) {
                    Console.WriteLine("{0} {1} {2}", Utils.GetCurrentDateTime(), ex.Message, ex.StackTrace);
                }

                //4、检查是否读取到配置内容
                if (fileContents != null)
                {
                    //4、新建一个CacheItemPolicy对象，该对象用于声明配置对象在缓存中的处理策略
                    CacheItemPolicy policy = new CacheItemPolicy();

                    //5、因为配置文件一直需要读取，所以在此设置缓存优先级为不应删除
                    // 实际情况请酌情考虑，同时可以设置AbsoluteExpiration属性指定过期时间
                    policy.Priority = CacheItemPriority.NotRemovable;

                    //6、将词典内容添加到缓存，传入 缓存Key、配置对象、对象策略
                    // Set方法首先会检查Key是否在缓存中存在，如果存在，更新value，不存在则创建新的
                    // 这里先加入缓存再加监视的原因是：在缓存加入时，也会触发监视事件，会导致出错。
                    cache.Set(cacheKey, fileContents, policy);

                    // //7、监视文件需要传入一个IList对象，所以即便只有一个文件也需要新建List对象
                    // List<string> filePaths = new List<string>() { _filePath };

                    // ////8、新建一个文件监视器对象，添加对资源文件的监视
                    // //HostFileChangeMonitor monitor = new HostFileChangeMonitor(filePaths);

                    ////8、新建一个文件监视器对象，添加对资源文件的监视
                    monitor = new HostFileChangeMonitor(filePaths);
                    //9、调用监视器的NotifyOnChanged方法传入发生改变时的回调方法
                    monitor.NotifyOnChanged(new OnChangedCallback((o) =>
                        {
                            cache.Remove(cacheKey);
                        }
                   ));

                    //10、为配置对象的缓存策略加入监视器
                    policy.ChangeMonitors.Add(monitor);
                }
            }
            return fileContents;
        }
    }
}
