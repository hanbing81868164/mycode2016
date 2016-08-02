using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Drawing;
using System.Collections.Specialized;

//namespace System.Runtime.CompilerServices
//{
//    public class ExtensionAttribute : Attribute { }
//}




/*
    调用方法
    new ServiceClient().Using(channel =>
{
    listdatamodel =channel.GetData();
});
 */
namespace System
{
    public static partial class Extention
    {
        /// <summary>
        /// 是否为null或空字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        /// <summary>
        /// 将JSON字符串序列化成指定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(this string s)
        {
            return JsonConvert.DeserializeObject<T>(s);
        }

        /// <summary>
        /// MD5函数
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string ToMD5(this string s, Encoding encoding = null)
        {
            byte[] b = encoding == null ? Encoding.Default.GetBytes(s) : encoding.GetBytes(s);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');

            return ret;
        }

        /// <summary>
        /// SHA256函数
        /// </summary>
        /// /// <param name="str">原始字符串</param>
        /// <returns>SHA256结果</returns>
        public static string ToSHA256(this string s, Encoding encoding = null)
        {
            byte[] SHA256Data = encoding == null ? Encoding.Default.GetBytes(s) : encoding.GetBytes(s);
            SHA256Managed Sha256 = new SHA256Managed();
            byte[] Result = Sha256.ComputeHash(SHA256Data);
            return Convert.ToBase64String(Result);  //返回长度为44字节的字符串
        }

        /// <summary>
        /// 获取指定字符串长度，汉字以2字节计算
        /// </summary>
        /// <param name="aOrgStr">要统计的字符串</param>
        /// <returns></returns>
        public static int GetLength(this string s)
        {
            int intLen = s.Length;
            int i;
            char[] chars = s.ToCharArray();
            for (i = 0; i < chars.Length; i++)
            {
                if (System.Convert.ToInt32(chars[i]) > 255)
                {
                    intLen++;
                }
            }
            return intLen;
        }


        /// <summary>
        /// 检测是否符合email格式
        /// </summary>
        /// <param name="strEmail">要判断的email字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsValidEmail(this string s)
        {
            return Regex.IsMatch(s, @"^[\w\.]+([-]\w+)*@[A-Za-z0-9-_]+[\.][A-Za-z0-9-_]");
        }

        public static bool IsValidDoEmail(this string s)
        {
            return Regex.IsMatch(s, @"^@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        /// 检测是否是正确的Url
        /// </summary>
        /// <param name="strUrl">要验证的Url</param>
        /// <returns>判断结果</returns>
        public static bool IsURL(this string s)
        {
            return Regex.IsMatch(s, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsDomain(this string s)
        {
            return Regex.IsMatch(s, @"\b(?<ym>([a-z0-9]+(-[a-z0-9]+)*\.)+((?!htm|asp|php|css|js|rar|zip|jpg|png|gif|ico)([a-z]{2,})))\b");
        }


        /// <summary>
        /// 转换字符串为日期时间.如果转换失败,则返回指定的默认值
        /// </summary>
        /// <param name="value">要转换的字符串</param>
        /// <param name="defaultValue">如果转换失败,则返回的默认值</param>
        /// <returns>转换后的 <see cref="System.DateTime"/></returns>
        public static DateTime ToDateTime(this string value, DateTime defaultValue)
        {
            DateTime temp;
            return DateTime.TryParse(value, out temp) ? temp : defaultValue;
        }

        /// <summary>
        /// 返回标准时间格式string
        /// </summary>
        public static DateTime ToDateTime(this string s)
        {
            return DateTime.Parse(s);//.ToString("yyyy-MM-dd HH:mm:ss");
        }


        /// <summary>
        /// 将字符串分割成行数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string[] ToLines(this string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;

            return value.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }



        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] Split(this string s, string strSplit)
        {
            if (!string.IsNullOrEmpty(s))
            {
                if (s.IndexOf(strSplit) < 0)
                    return new string[] { s };

                return Regex.Split(s, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
                return new string[0] { };
        }

        /// <summary>
        /// 返回指定的字符串中是否包含另外一个子字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="key">关键字</param>
        /// <param name="comparison">比较方式</param>
        /// <returns>包含为true， 否则为false</returns>
        public static bool Contains(this string str, string key, StringComparison comparison)
        {
            return str.IndexOf(key, comparison) != -1;
        }


        /// <summary>
        /// 返回 HTML 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码结果</returns>
        public static string HtmlEncode(this string s)
        {
            return HttpUtility.HtmlEncode(s);

        }

        /// <summary>
        /// 返回 HTML 字符串的解码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>解码结果</returns>
        public static string HtmlDecode(this string s)
        {
            return HttpUtility.HtmlDecode(s);
        }

        /// <summary>
        /// 返回 URL 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码结果</returns>
        public static string UrlEncode(this string s, Encoding encoding = null)
        {
            return encoding == null ? HttpUtility.UrlEncode(s) : HttpUtility.UrlEncode(s, encoding);
        }

        /// <summary>
        /// 返回 URL 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>解码结果</returns>
        public static string UrlDecode(this string s, Encoding encoding = null)
        {
            return encoding == null ? HttpUtility.UrlDecode(s) : HttpUtility.UrlDecode(s, encoding);
        }

        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(this string s)
        {
            return Regex.IsMatch(s, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }


        /// <summary>
        /// 移除Html标记
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveHtml(this string s)
        {
            return Regex.Replace(s, @"<[^>]*>", string.Empty, RegexOptions.IgnoreCase);
        }


        /// <summary>
        /// 将字符串转换为Color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color ToColor(this string color)
        {
            int red, green, blue = 0;
            char[] rgb;
            color = color.TrimStart('#');
            color = Regex.Replace(color.ToLower(), "[g-zG-Z]", "");
            switch (color.Length)
            {
                case 3:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[0].ToString() + rgb[0].ToString(), 16);
                    green = Convert.ToInt32(rgb[1].ToString() + rgb[1].ToString(), 16);
                    blue = Convert.ToInt32(rgb[2].ToString() + rgb[2].ToString(), 16);
                    return Color.FromArgb(red, green, blue);
                case 6:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[0].ToString() + rgb[1].ToString(), 16);
                    green = Convert.ToInt32(rgb[2].ToString() + rgb[3].ToString(), 16);
                    blue = Convert.ToInt32(rgb[4].ToString() + rgb[5].ToString(), 16);
                    return Color.FromArgb(red, green, blue);
                default:
                    return Color.FromName(color);

            }
        }

        /// <summary>
        /// 格式化
        /// </summary>
        /// <param name="s"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string SFormat(this string s, params object[] args)
        {
            if (!string.IsNullOrEmpty(s))
                return string.Format(s, args);
            return s;
        }

        /// <summary>
        /// 将string类弄转换为byte[]类型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ed">Encoding编码,可为null</param>
        /// <returns></returns>
        public static byte[] ToBytes(this string s, Encoding encoding = null)
        {
            return encoding == null ? System.Text.Encoding.Default.GetBytes(s) : encoding.GetBytes(s);
        }

        /// <summary>
        /// 将string 转换成 Stream
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ed"></param>
        /// <returns></returns>
        public static MemoryStream ToStream(this string s, Encoding encoding = null)
        {
            MemoryStream memStream = new MemoryStream();
            byte[] data = s.ToBytes(encoding);
            memStream.Write(data, 0, data.Length);
            return memStream;
        }

        /// <summary>
        /// 将string类弄转换为byte[]类型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ed">Encoding编码,可为null</param>
        /// <returns></returns>
        public static bool ToBytes(this string s, string savePath, Encoding encoding = null)
        {
            ToBytes(s, encoding).Save(savePath);
            return true;
        }

        /// <summary>
        /// 转换编码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToUTF8(this string s)
        {
            Encoding utf8 = Encoding.UTF8;
            Encoding gb2312 = Encoding.GetEncoding("gb2312");
            byte[] unicodeBytes = gb2312.GetBytes(s);
            byte[] asciiBytes = Encoding.Convert(gb2312, utf8, unicodeBytes);
            char[] asciiChars = new char[utf8.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
            utf8.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
            return new string(asciiChars);
        }

        /// <summary>
        /// 转换编码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToGB2312(this string s)
        {
            Encoding utf8 = Encoding.UTF8;
            Encoding gb2312 = Encoding.GetEncoding("gb2312");
            byte[] unicodeBytes = utf8.GetBytes(s);
            byte[] asciiBytes = Encoding.Convert(utf8, gb2312, unicodeBytes);
            char[] asciiChars = new char[gb2312.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
            gb2312.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
            return new string(asciiChars);
        }


        public static bool IsMatch(this string s, string pattern)
        {
            if (s == null) return false;
            else return Regex.IsMatch(s, pattern);
        }

        public static string Match(this string s, string pattern)
        {
            if (s == null) return string.Empty;
            return Regex.Match(s, pattern).Value;
        }

        public static bool IsInt(this string s)
        {
            int i;
            return int.TryParse(s, out i);
        }

        /// <summary>
        /// 将字符串转换为Int值
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="defaultValue">如果转换失败,则返回的默认值</param>
        /// <returns>转换后的 <see cref="System.Int32"/></returns>
        public static int ToInt32(this string value, int defaultValue)
        {
            int temp;
            return int.TryParse(value, out temp) ? temp : defaultValue;
        }

        /// <summary>
        /// 转换字符串为双精度数.如果转换失败,则返回指定的默认值
        /// </summary>
        /// <param name="value">要转换的字符串</param>
        /// <param name="defaultValue">如果转换失败,则返回的默认值</param>
        /// <returns>转换后的 <see cref="System.Double"/></returns>
        public static double ToDouble(this string value, double defaultValue)
        {
            double temp;
            return double.TryParse(value, out temp) ? temp : defaultValue;
        }

        /// <summary>
        /// 转换字符串为双精度数.如果转换失败,则返回指定的默认值
        /// </summary>
        /// <param name="value">要转换的字符串</param>
        /// <param name="defaultValue">如果转换失败,则返回的默认值</param>
        /// <returns>转换后的 <see cref="System.Double"/></returns>
        public static decimal ToDecimal(this string value, decimal defaultValue)
        {
            decimal temp;
            return decimal.TryParse(value, out temp) ? temp : defaultValue;
        }


        /// <summary>
        /// 返回文件名
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetFileName(this string s)
        {
            return System.IO.Path.GetFileName(s);
        }

        /// <summary>
        /// 返回文件的扩展名 如: .doc .html .jpg .png
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetExtension(this string s)
        {
            return System.IO.Path.GetExtension(s);
        }


        static Regex regex_domain = new Regex("(?<domain>(http|https)://[^.]*.([^/|?]*))", RegexOptions.IgnoreCase);
        /// <summary>
        /// 返回网址里的域名，包含http:// 最后不带/ 如： http://www.baidu.com
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetDomain(this string s)
        {
            if (s.IsNullOrEmpty())
                return s;

            var m = regex_domain.Match(s);
            if (m.Success)
                return m.Groups["domain"].Value;
            return string.Empty;
        }

        public static NameValueCollection ParseQueryString(this string s)
        {
            return System.Web.HttpUtility.ParseQueryString(s);
        }
        public static string GetDirectoryName(this string s)
        {
            if (s.IsNullOrEmpty())
                return null;

            return System.IO.Path.GetDirectoryName(s);
        }



        public static bool CreateDirectory(this string s)
        {
            //if (!System.IO.Directory.Exists(s))
            //    System.IO.Directory.CreateDirectory(s);
            DirectoryHelper.CreateDirectory(s);
            return true;
        }
        public static bool DeleteDirectory(this string s)
        {
            if (s.IsNullOrEmpty())
                return true;

            DirectoryHelper.Delete(s);
            //if (System.IO.Directory.Exists(s))
            //    System.IO.Directory.Delete(s, true);
            return true;
        }

        public static bool DeleteFile(this string s)
        {
            if (s.IsNullOrEmpty())
                return true;
            if (File.Exists(s))
            {
                try
                {
                    File.Delete(s);
                }
                catch { }
            }
            return true;
        }


        /// <summary>
        /// 根据字符串获取枚举值
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">字符串枚举值</param>
        /// <param name="defValue">缺省值</param>
        /// <returns></returns>
        public static T GetEnum<T>(this string value, T defValue)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch (ArgumentException)
            {
                return defValue;
            }
        }

        public static Uri ToUri(this string s)
        {
            return new Uri(s);
        }

        public static bool UrlIsExist(this string url)
        {
            Uri u = null;
            try
            {
                u = new Uri(url);
            }
            catch { return false; }

            bool isExist = false;

            if (!u.AbsoluteUri.IsURL())
                return false;

            System.Net.HttpWebRequest r = null;
            System.Net.HttpWebResponse s = null;
            try
            {
                r = System.Net.HttpWebRequest.Create(u) as System.Net.HttpWebRequest;
                r.Timeout = 10000;
                r.Method = "HEAD";
                r.Proxy = null;

                s = r.GetResponse() as System.Net.HttpWebResponse;
                isExist = (s.StatusCode == Net.HttpStatusCode.OK);
                s.Close();
                s = null;
            }
            catch (System.Net.WebException x)
            {
                try
                {
                    isExist = ((x.Response as System.Net.HttpWebResponse).StatusCode != Net.HttpStatusCode.NotFound);
                }
                catch
                {
                    isExist = (x.Status == Net.WebExceptionStatus.Success);
                }
            }
            finally
            {
                try
                {
                    if (r != null)
                    {
                        r = null;
                    }
                    if (s != null)
                    {
                        s.Close();
                        s = null;
                    }
                }
                catch { }
            }
            return isExist;
        }





        #region 对象扩展

        /// <summary>
        /// 转换为文件夹对象
        /// </summary>
        /// <param name="folder">文件夹路径</param>
        /// <returns>对应的文件夹信息对象</returns>
        public static System.IO.DirectoryInfo AsDirectoryInfo(this string folder)
        {
            return new System.IO.DirectoryInfo(folder);
        }

        /// <summary>
        /// 转换为文件信息对象
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns><see cref="T:System.IO.FileInfo"/></returns>
        public static System.IO.FileInfo AsFileInfo(this string filePath)
        {
            return new System.IO.FileInfo(filePath);
        }

        #endregion

        /// <summary>
        /// 合并两个路径 c:\123\1\45\  ../../ 返回c:\123
        /// </summary>
        /// <param name="basepath">c:\123\1\45\</param>
        /// <param name="relativePath">../../</param>
        /// <returns></returns>
        public static string PathCombine(this string basepath, string relativePath)
        {
            return new Uri(Path.Combine(basepath, relativePath)).LocalPath;
        }


        /// <summary>
        /// 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
        /// </summary>
        /// <param name="strNumber">要确认的字符串数组</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumericArray(this string[] strNumber)
        {
            if (strNumber == null)
                return false;

            if (strNumber.Length < 1)
                return false;

            foreach (string id in strNumber)
            {
                if (!IsInt(id))
                    return false;
            }
            return true;
        }




        /// <summary>
        /// 将一个字符串等分成几个字符串，如：str=123456789,grouplen=2,返回为:12,34,56,78,9
        /// </summary>
        /// <param name="str"></param>
        /// <param name="grouplen"></param>
        /// <returns></returns>
        public static string[] GetGroup(this string str, int grouplen)
        {
            int len = Convert.ToInt32(System.Math.Ceiling((double)(str.Length / grouplen)));

            List<string> ss = new List<string>();
            for (int i = 0; i <= len; i++)
            {
                int s = i * grouplen;
                int e = (str.Length - s) < grouplen ? (str.Length - s) : grouplen;
                if (e != 0)
                {
                    ss.Add(str.Substring(s, e));
                }
            }
            return ss.ToArray();
        }



    }
}
