using System.Collections.Generic;
using System.Text;
using System.Web;

namespace System
{
    public class Cookies
    {
        private static volatile Cookies instance = null;
        private static object lockHelperer = new object();

        private IList<string> KeyList = null;


        public Cookies()
        {
            KeyList = new List<string>();
        }

        public void Clear()
        {
            if (KeyList.Count > 0)
            {
                for (int i = 0; i < KeyList.Count; i++)
                {
                    Del(KeyList[i]);
                }
            }
        }

        /// <summary>
        /// 返回接口
        /// </summary>
        /// <returns></returns>
        public static Cookies GetServices()
        {
            if (instance == null)
            {
                lock (lockHelperer)
                {
                    if (instance == null)
                    {
                        instance = new Cookies();
                    }
                }
            }
            return instance;
        }


        /// <summary>
        /// 判断一个cookie是否存在
        /// </summary>
        /// <param name="Key">名称</param>
        /// <returns></returns>
        public virtual bool IsNull(string Name)
        {
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[Name];
            return cookie == null;
        }



        /// <summary>
        /// 对cookie进行UTF编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string GetUTF(string str)
        {
            Encoding stre = Encoding.GetEncoding("UTF-8");
            return HttpUtility.UrlDecode(str, stre);
        }

        /// <summary>
        /// 返回cookie值
        /// </summary>
        /// <param name="Name">名称</param>
        /// <returns></returns>
        public virtual string GetValue(string Name)
        {
            if (!IsNull(Name))
            {
                HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[Name];
                if (cookie == null)
                    return null;
                return GetUTF(cookie.Value);
            }
            return null;
        }


        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public virtual void Add(string Name, string Value, double ExpiresMinutes = 0, string Domain = "")
        {
            if (!KeyList.Contains(Name))
                KeyList.Add(Name);

            Value = HttpUtility.UrlEncode(Value);

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[Name];
            if (cookie == null)
            {
                cookie = new HttpCookie(Name);
            }
            cookie.Value = Value;
            if (ExpiresMinutes > 0)
            {
                cookie.Expires = DateTime.Now.AddMinutes(ExpiresMinutes);
            }

            if (!string.IsNullOrEmpty(Domain))
            {
                cookie.Domain = Domain;
            }
            HttpContext.Current.Response.AppendCookie(cookie);
        }


        /// <summary>
        /// 删除cookie
        /// </summary>
        /// <param name="Name">名称</param>
        public virtual void Del(string Name)
        {
            if (Name.IsNullOrEmpty())
                return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[Name];
            if (cookie == null)
                return;

            cookie.Expires = DateTime.Now.AddHours(-1);
            HttpContext.Current.Response.AppendCookie(cookie);
        }
    }
}
