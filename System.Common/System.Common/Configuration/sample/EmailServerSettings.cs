using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Xml.Serialization;

namespace Public.Common.Test
{
    public class EmailServerSettings
    {
        private string ip;
        private string name;


        [XmlElement("IP")]
        public string IP { 
            get {return ip;}
            set { ip = value; } 
        }

        
        [XmlElement("Name")]        
        public string Name {
            get { return name; }
            set { name = value; }
        }


        [XmlArray("Tinfos")]
        [XmlArrayItem("Tinfo")]
        public Tinfo[] tlist { get; set; }


        public override string ToString()
        {
            return String.Format("Name: {0}, IP: {1}", Name, IP);
        }
    }

    public class Tinfo
    {
        [XmlAttribute("n")]
        public string n { get; set; }

        [XmlText]
        public string text { get; set; }
    }
}
