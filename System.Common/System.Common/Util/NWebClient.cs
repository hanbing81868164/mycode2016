using System.Net;

namespace System
{
    public class NWebClient : System.Net.WebClient
    {
        public int Timeout { get; set; }

        public NWebClient()
        {
            Timeout = 1000 * 60 * 60 * 5;
        }

        protected override System.Net.WebRequest GetWebRequest(Uri address)
        {
            //return base.GetWebRequest(address);

            //HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            //request.Timeout = 1000 * 60 * 60 * 5;
            //request.ReadWriteTimeout = 1000 * 60 * 60 * 5;
            //return request;

            HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
            if (request != null)
            {
                request.Timeout = Timeout;
                request.ReadWriteTimeout = Timeout;
            }
            return request;
        }
    }
}





