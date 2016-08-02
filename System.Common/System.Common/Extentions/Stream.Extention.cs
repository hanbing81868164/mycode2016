using System.Text;
using System.IO;



namespace System
{
    public static partial class Extention
    {

        /// <summary>
        /// 将Stream转换为string类型
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ToString(this Stream stream, Encoding encoding = null)
        {
            string Content = null;
            using (StreamReader reader = new StreamReader(stream, (encoding == null ? Encoding.Default : encoding)))
            {
                Content = reader.ReadToEnd();
            }
            return Content;
        }

        /// <summary>
        /// 将 Stream 转成 MemoryStream
        /// </summary>
        /// <param name="mystream"></param>
        /// <returns></returns>
        public static MemoryStream ToMemoryStream(this Stream stream)
        {
            MemoryStream msTemp = new MemoryStream();
            int len = 0;
            byte[] buff = new byte[512];

            while ((len = stream.Read(buff, 0, 512)) > 0)
            {
                msTemp.Write(buff, 0, len);
            }
            stream.Dispose();

            return msTemp;
        }


        /// <summary>
        /// 将 Stream 转成 byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);

            stream.Dispose();

            return bytes;
        }

        /// <summary>
        /// 将stream保存成文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="savePath"></param>
        public static void Save(this Stream stream, string savePath)
        {
            //StreamReader sr = new StreamReader(stream);
            //StreamWriter sw = new StreamWriter(savePath);

            //sw.Write(sr.ReadToEnd());
            //sw.Flush();
            //sw.Close();
            //sr.Close();




            //// 把 Stream 转换成 byte[]
            //byte[] bytes = new byte[stream.Length];
            //stream.Read(bytes, 0, bytes.Length);
            //// 设置当前流的位置为流的开始
            //stream.Seek(0, SeekOrigin.Begin);

            //// 把 byte[] 写入文件
            //FileStream fs = new FileStream(savePath, FileMode.Create);
            //BinaryWriter bw = new BinaryWriter(fs);
            //bw.Write(bytes);
            //bw.Close();
            //fs.Close(); 

            stream.ToBytes().Save(savePath);





        }


        //public static string GetHtmlSource(this Stream stream)
        //{
        //    using (MemoryStream msTemp = new MemoryStream())
        //    {
        //        int len = 0;
        //        byte[] buff = new byte[512];
        //        while ((len = stream.Read(buff, 0, 512)) > 0)
        //        {
        //            msTemp.Write(buff, 0, len);
        //        }
        //        if (msTemp.Length > 0)
        //        {
        //            msTemp.Seek(0, SeekOrigin.Begin);
        //            byte[] PageBytes = new byte[msTemp.Length];
        //            msTemp.Read(PageBytes, 0, PageBytes.Length);

        //            msTemp.Seek(0, SeekOrigin.Begin);
        //            int DetLen = 0;
        //            byte[] DetectBuff = new byte[4096];
        //            Mozilla.NUniversalCharDet.UniversalDetector Det = new Mozilla.NUniversalCharDet.UniversalDetector(null);
        //            while ((DetLen = msTemp.Read(DetectBuff, 0, DetectBuff.Length)) > 0 && !Det.IsDone())
        //            {
        //                Det.HandleData(DetectBuff, 0, DetectBuff.Length);
        //            }
        //            Det.DataEnd();
        //            if (Det.GetDetectedCharset() != null)
        //            {
        //                // "OK! CharSet=" + Det.GetDetectedCharset();
        //                return Encoding.GetEncoding(Det.GetDetectedCharset()).GetString(PageBytes);
        //            }
        //        }
        //    }
        //    return null;
        //}

    }
}
