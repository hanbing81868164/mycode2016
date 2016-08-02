namespace System
{
    public class SingletonBase<TInstance>
        where TInstance : SingletonBase<TInstance>, new()
    {
        private static volatile TInstance instance = null;
        private static object lockHelperer = new object();

        //public SingletonBase() { }

        /// <summary>
        /// 返回接口
        /// </summary>
        /// <returns></returns>
        public static TInstance Instance()
        {
            if (instance == null)
            {
                lock (lockHelperer)
                {
                    if (instance == null)
                    {
                        instance = new TInstance();
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// 设置实例
        /// </summary>
        /// <param name="value"></param>
        public void SetInstance(TInstance value)
        {
            instance = value;
        }
    }
}
