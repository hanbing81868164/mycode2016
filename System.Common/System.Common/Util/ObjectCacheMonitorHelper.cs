namespace System
{
    public class ObjectCacheMonitorHelper<T> : SingletonBase<ObjectCacheMonitorHelper<T>> where T : class ,new()
    {
        GenericCache<Type, ObjectCacheMonitorManage<T>> Caches = new GenericCache<Type, ObjectCacheMonitorManage<T>>();

        public ObjectCacheMonitorHelper() { }

        public void Add(string filePath, Func<object, T> fun)
        {
            if (!Caches.ContainsKey(typeof(T)))
            {
                ObjectCacheMonitorManage<T> res = new ObjectCacheMonitorManage<T>(filePath, fun);
                Caches.Add(typeof(T), res);
            }
        }

        public void Remove()
        {
            if (Caches.ContainsKey(typeof(T)))
            {
                ObjectCacheMonitorManage<T> res = null;
                if (Caches.TryGetValue(typeof(T), out res))
                {
                    res.Remove();
                }
            }
        }

        public T GetData()
        {
            if (Caches.ContainsKey(typeof(T)))
            {
                ObjectCacheMonitorManage<T> res = null;
                if (Caches.TryGetValue(typeof(T), out res))
                {
                    return res.GetData();
                }
            }
            return null;
        }
    }
}
