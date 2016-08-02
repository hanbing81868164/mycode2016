using System.Collections.Generic;
using System.IO;
using System.Net;


namespace System
{

    public static class HttpRequest
    {

        public static Stream Request(string uri, string method = "GET", Dictionary<string, string> headers = null, byte[] data = null)
        {
            Stream stream = null;
            HttpWebRequest req = WebRequest.Create(uri) as HttpWebRequest;

            if (headers != null && headers.Count > 0)
            {
                foreach (var h in headers)
                {
                    req.Headers.Add(h.Key, h.Value);
                }
            }

            if (!method.IsNullOrEmpty())
                req.Method = method.ToUpper();

            if (data != null)
            {
                using (var reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                }
            }

            WebResponse resp = null;
            try
            {
                resp = req.GetResponse();
            }
            catch (WebException wex)
            {
                throw wex;//  httpwebresponse = wex.Response as HttpWebResponse;
            }

            if (resp != null)
            {
                stream = resp.GetResponseStream();
                try
                {
                    resp.Close();
                }
                catch (Exception ex) { }
            }
            return stream;
        }

    }
}
