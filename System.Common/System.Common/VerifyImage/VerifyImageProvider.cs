using System.Collections;

namespace System
{
    /// <summary>
    /// 验证码图片创建类
    /// </summary>
    public class VerifyImageProvider
    {
        private static Hashtable _instance = new Hashtable();
        private static object lockHelper = new object();

        /// <summary>
        /// 获取验证码的类实例
        /// </summary>
        /// <param name="assemlyName">用于区分库文件的名称</param>
        /// <returns></returns>
        public static IVerifyImage GetInstance(string assemlyName)
        {
            if (!_instance.ContainsKey(assemlyName))
            {
                lock (lockHelper)
                {
                    if (!_instance.ContainsKey(assemlyName))
                    {
                        IVerifyImage p = new VerifyImage();

                        _instance.Add(assemlyName, p);
                    }
                }
            }
            return (IVerifyImage)_instance[assemlyName];
        }
    }
}
