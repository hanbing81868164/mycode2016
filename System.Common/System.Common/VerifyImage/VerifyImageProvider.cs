using System.Collections;

namespace System
{
    /// <summary>
    /// ��֤��ͼƬ������
    /// </summary>
    public class VerifyImageProvider
    {
        private static Hashtable _instance = new Hashtable();
        private static object lockHelper = new object();

        /// <summary>
        /// ��ȡ��֤�����ʵ��
        /// </summary>
        /// <param name="assemlyName">�������ֿ��ļ�������</param>
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
