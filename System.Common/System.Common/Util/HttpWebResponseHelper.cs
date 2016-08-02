using System.Net.Security;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace System
{
    /// <summary>
    /// 有关HTTP请求的辅助类
    /// </summary>
    public class HttpWebRequestUtility
    {

        ///// <summary>
        ///// 取得页面HTML代码
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="postData"></param>
        ///// <param name="headers"></param>
        ///// <returns></returns>
        //public static string GetSource(string url, byte[] postData = null, WebHeaderCollection headers = null)
        //{
        //    return GetSource(CreateHttpResponse(url, postData, headers));
        //}

        /// <summary>
        /// 创建POST方式的HTTP请求
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>
        /// <param name="timeout">请求的超时时间</param>
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>
        /// <returns></returns>
        public static WebResponse CreateHttpResponse(string url, byte[] postData = null, WebHeaderCollection headers = null)
        {
            HttpWebRequest request = null;
            //如果是发送HTTPS请求
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = postData != null ? "POST" : "GET";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.2; zh-CN; rv:1.9.2.8) Gecko/20100722 Firefox/3.6.8";

            //如果需要POST数据
            if (postData != null)
            {
                request.ContentType = "application/x-www-form-urlencoded";
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(postData, 0, postData.Length);
                }
            }
            return request.GetResponse();
        }

        //public static string GetSource(WebResponse response)
        //{
        //    string res = string.Empty;
        //    Stream stream = null;
        //    try
        //    {
        //        stream = response.GetResponseStream();
        //        res = stream.GetHtmlSource();
        //    }
        //    catch (WebException e)
        //    {
        //        return null;
        //    }
        //    catch (IOException e)
        //    {
        //        return null;
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        if (stream != null)
        //            stream.Close();

        //        if (response != null)
        //            response.Close();
        //    }
        //    return res;
        //}

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受
        }
    }
}