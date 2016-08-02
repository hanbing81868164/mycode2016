using System.Text;
using System.IO;



namespace System
{
    public static partial class Extention
    {

        /// <summary>
        /// 将byte[]类弄转换为string类型
        /// </summary>
        /// <param name="bt"></param>
        /// <param name="ed">Encoding编码,可为null</param>
        /// <returns></returns>
        public static string ToString(this byte[] bytes, Encoding encoding = null)
        {
            return encoding == null ? System.Text.Encoding.Default.GetString(bytes) : encoding.GetString(bytes);
        }


        /// <summary>
        /// 将bytes转换为Base64String
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string ToBase64String(this byte[] bytes, Base64FormattingOptions options = Base64FormattingOptions.None)
        {
            return Convert.ToBase64String(bytes);
        }


        /// <summary>
        /// 将 byte[] 转成 Stream
        /// </summary>
        public static Stream ToStream(this byte[] bytes)
        {
            return new MemoryStream(bytes);
        }


        public static MemoryStream ToMemoryStream(this　byte[] data)
        {
            return new MemoryStream(data);
        }

        /// <summary>
        /// 将bytes保存成文件
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="savePath"></param>
        public static void Save(this byte[] bytes, string savePath)
        {
            File.WriteAllBytes(savePath, bytes);
            //ToStream(bytes).Save(savePath);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="bytes"></param>
        ///// <returns></returns>
        //public static Encoding GetDetectEncoding(this byte[] bytes)
        //{
        //    Mozilla.NUniversalCharDet.UniversalDetector Det = new Mozilla.NUniversalCharDet.UniversalDetector(null);
        //    while (!Det.IsDone())
        //    {
        //        Det.HandleData(bytes, 0, bytes.Length);
        //    }
        //    Det.DataEnd();
        //    if (Det.GetDetectedCharset() != null)
        //    {
        //        return Encoding.GetEncoding(Det.GetDetectedCharset());
        //    }
        //    return Encoding.Default;
        //}


        //public static string GetHtmlSource(this byte[] bytes)
        //{
        //    return bytes.ToStream().GetHtmlSource();
        //}
    }
}
