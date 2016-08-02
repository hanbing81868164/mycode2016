namespace System
{
    public class SafeThreadHelper : SingletonBase<SafeThreadHelper>
    {

        public volatile static GenericCache<string, SafeThreadMange> SafeThreadManges = new GenericCache<string, SafeThreadMange>();

        public SafeThreadHelper()
        {

        }

        public bool Add(string key, int maxThread, Action action, Func<bool> hasData)
        {
            var safeThreadMange = new SafeThreadMange(maxThread, action, hasData);
            SafeThreadManges.Add(key, safeThreadMange);

            return true;
        }

        public bool Remove(string key)
        {
            SafeThreadManges.Remove(key);
            return true;
        }
    }
}
