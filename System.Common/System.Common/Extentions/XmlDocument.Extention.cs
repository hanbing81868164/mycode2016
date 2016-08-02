using Newtonsoft.Json;
using System.Xml;


namespace System
{
    public static partial class Extention
    {

        public static string SerializeXmlNode(this XmlDocument doc)
        {
            return JsonConvert.SerializeXmlNode(doc);
        }



    }
}
