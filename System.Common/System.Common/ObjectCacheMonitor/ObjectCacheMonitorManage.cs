namespace System
{
    public class ObjectCacheMonitorManage<T> where T : class ,new()
    {
        ObjectCacheMonitor<T> _ObjectCacheMonitor = null;
        Func<object, T> _fun = null;
        string _filePath = string.Empty;

        public ObjectCacheMonitorManage(string filePath, Func<object, T> fun)
        {
            _filePath = filePath;
            _fun = fun;
            _ObjectCacheMonitor = new ObjectCacheMonitor<T>(_filePath, _fun);
        }

        public void Remove()
        {
            _ObjectCacheMonitor.Remove();
        }

        public T GetData()
        {
            return _ObjectCacheMonitor.GetCache();
        }
    }
}
