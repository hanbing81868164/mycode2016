using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace System.Common
{
    public class HttpHelp
    {
        //定義一個網關對象
        public static string GetPageSource(string url, Encoding encoding = null, WebHeaderCollection headers = null, WebProxy proxy = null)
        {
            //代理
            //int daili = ConfigurationManager.AppSettings["daili"] != null ? int.Parse(ConfigurationManager.AppSettings["daili"].ToString()) : 0;
            //Uri a = new Uri("http://localhost:8118");
            //WebProxy proxy = new WebProxy();
            //proxy.Address = a;

            /*
            WebProxy proxy = new WebProxy();                                    //定義一個網關對象
            proxy.Address = new Uri("http://proxy.domain.com:3128");            //網關服務器:端口
            proxy.Credentials = new NetworkCredential("f3210316", "6978233");    //用戶名,密碼
            hwr.UseDefaultCredentials = true;                                    //啟用網關認証
            hwr.Proxy = proxy;                                                    //設置網關
            */


            WebResponse response = null;
            Stream stream = null;
            StreamReader reader = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                if (proxy != null)
                {
                    request.Proxy = proxy;
                }
                request.Timeout = 90000;

                //request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                //request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.2; zh-CN; rv:1.9.2.8) Gecko/20100722 Firefox/3.6.8";
                if (headers != null)
                {
                    request.Headers = headers;
                }

                response = request.GetResponse();

                stream = response.GetResponseStream();

                if (!response.ContentType.ToLower().StartsWith("text/"))
                {
                    return null;
                }
                string buffer = "", line;

                reader = new StreamReader(stream, (encoding == null ? Encoding.UTF8 : encoding));

                while ((line = reader.ReadLine()) != null)
                {
                    buffer += line + "\r\n";
                }

                return buffer;
            }
            catch (WebException e)
            {
                LOG.WriteText(e.Message);
                return null;
            }
            catch (IOException e)
            {
                LOG.WriteText(e.Message);
                return null;
            }
            catch (Exception e)
            {
                LOG.WriteText(e.Message);
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (stream != null)
                    stream.Close();

                if (response != null)
                    response.Close();
            }
        }

    }
}
