using System.Text;
using System.IO;
using System.Net;

namespace System
{
    public class FileHelper
    {

        /// <summary>
        /// 取得文件的byte[]
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string filePath)
        {
            byte[] bytes = null;
            using (WebClient webClient = new WebClient())
            {
                bytes = webClient.DownloadData(filePath);
                webClient.Dispose();
            }
            return bytes;
        }


        /// <summary>
        /// 取得文件的Stream
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Stream GetStream(string filePath)
        {
            byte[] bytes = GetBytes(filePath);
            return new MemoryStream(bytes);
        }


        /// <summary>
        /// 返回文件的内容,如TXT的内容
        /// </summary>
        /// <param name="fileName">文件地址</param>
        /// <param name="ed">编码</param>
        /// <returns></returns>
        public static string GetContent(string filePath, Encoding encoding = null)
        {
            byte[] bytes = GetBytes(filePath);
            return encoding == null ? Encoding.Default.GetString(bytes) : encoding.GetString(bytes);
        }

        /// <summary>
        /// 向现有文件增加内容
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static bool Writer(string savePath, byte[] data)
        {
            //using (var writer = new StreamWriter(savePath, true, encoding))
            //{
            //    writer.Write(data);
            //    return true;
            //}


            using (FileStream fs = new FileStream(savePath, FileMode.Append | FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                //开始写入
                fs.Write(data, 0, data.Length);
                //清空缓冲区、关闭流
                fs.Flush();
            }

            return false;
        }


    }
}
