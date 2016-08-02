using Newtonsoft.Json;
using System.Xml.Linq;


namespace System
{
    public static partial class Extention
    {
        /// <summary>
        /// 将json字符串转换成xml节点
        /// </summary>
        /// <param name="s"></param>
        /// <param name="deserializeRootElementName"></param>
        /// <returns></returns>
        public static XNode JsonDeserializeXNode(this string s, string deserializeRootElementName = null)
        {
            if (deserializeRootElementName.IsNullOrEmpty())
                return JsonConvert.DeserializeXNode(s);
            return JsonConvert.DeserializeXNode(s, deserializeRootElementName);
        }



    }
}
