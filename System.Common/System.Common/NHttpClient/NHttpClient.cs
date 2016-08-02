using System.ComponentModel;
using System.IO;
using System.Net.Http;

namespace System
{
    public enum HttpClientMethod
    {
        [Description("Get请求")]
        Get = 0,
        Post = 1,
        Put = 2,
        Delete = 3
    }

    public class NHttpClient
    {
        //HttpClient httpClient = new HttpClient();

        public NHttpClient()
        {
            //httpClient.DefaultRequestHeaders.Accept.Add(new Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"));
            //httpClient.DefaultRequestHeaders.UserAgent.Add(new Net.Http.Headers.ProductInfoHeaderValue("Mozilla/5.0 (Windows; U; Windows NT 5.2; zh-CN; rv:1.9.2.8) Gecko/20100722 Firefox/3.6.8"));
        }

        public void ClearHeaders()
        {
            //httpClient.DefaultRequestHeaders.Clear();
            //httpClient.DefaultRequestHeaders.Accept.Clear();
            //httpClient.DefaultRequestHeaders.UserAgent.Clear();
        }

        public static Stream GetStream(string uri)
        {
            using (var httpClient = new HttpClient())
            {
                var res = httpClient.GetStreamAsync(uri);
                res.Wait();
                return res.Result;
            }
        }

        public static string GetString(string uri)
        {
            using (var httpClient = new HttpClient())
            {
                var res = httpClient.GetStringAsync(uri);
                res.Wait();
                return res.Result;
            }
        }

    }
}
