namespace System
{
    /// <summary>
    /// 文件操作
    /// </summary>
    public class NTxtHelper
    {
        public static GenericCache<string, NTxt> cacheFiles = new GenericCache<string, NTxt>();

        public static NTxt GetNetFile(string filePath)
        {
            NTxt res = null;
            if (cacheFiles.TryGetValue(filePath, out res))
            {
                return res;
            }
            else
            {
                res = new NTxt(filePath);
                cacheFiles.Add(filePath, res);
                return res;
            }
        }

        static System.Timers.Timer t = null;
        /// <summary>
        /// 文件操作
        /// </summary>
        static NTxtHelper()
        {
            t = new System.Timers.Timer(1000 * 60 * 5);//实例化Timer类，设置间隔时间为5分钟
            t.Elapsed += new System.Timers.ElapsedEventHandler((s, e) =>
            {
                try
                {
                    foreach (var o in cacheFiles.Dictionary)
                    {
                        if ((DateTime.Now - o.Value.LastShowTime).Minutes > 5)//大于5分钟
                            cacheFiles.Remove(o.Key);
                    }

                }
                catch { }
            });//到达时间的时候执行事件；
            t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件； 
            t.Start();
        }

        /// <summary>
        /// 向指定的文件里写入一行数据
        /// </summary>
        /// <param name="filepath">文件名(包含地址,如:c:\123.txt)</param>
        /// <param name="WriteLineText">写入的内容</param>
        public static void WriteLine(string filePath, string WriteLineText)
        {
            WriteLineText = WriteLineText + "\r\n";
            GetNetFile(filePath).Write(WriteLineText);
        }

        /// <summary>
        /// 向指定的文件里写入数据,可以为多行
        /// </summary>
        /// <param name="filepath">文件名(包含地址,如:c:\123.txt)</param>
        /// <param name="WriteText_">写入的内容</param>
        public static void WriteText(string filePath, string WriteText)
        {
            GetNetFile(filePath).Write(WriteText);
        }


        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool Close(string filePath)
        {
            System.Threading.Thread.Sleep(100);
            var file = GetNetFile(filePath);
            if (file != null)
            {
                while (file.msgs.Count > 0)
                {
                    System.Threading.Thread.Sleep(100);
                }
                file.Close();
            }

            if (cacheFiles.ContainsKey(filePath))
            {
                cacheFiles.Remove(filePath);
            }
            return true;
        }

    }
}
