using System.IO;
using System.Xml;


namespace System
{
    public class XmlSectionWriter : XmlTextWriter
    {
        #region Properties and fields

        bool skipAttribute = false;
        StringWriter stringWriter;
        XmlWriterSettings settings = new XmlWriterSettings();

        #endregion

        #region Ctor

        public XmlSectionWriter(StringWriter w)
            : base(w)
        {
            this.stringWriter = w;        
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return stringWriter.ToString();
        }

        public override void WriteDocType(string name, string pubid, string sysid, string subset)
        {
            return;
        }

        public override void WriteStartDocument()
        {
            return;
        }

        public override void WriteStartDocument(bool standalone)
        {
            return;
        }

        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            if (prefix == "xmlns" && (localName == "xsi" || localName == "xsd"))
            {
                skipAttribute = true;
                return;
            }
            base.WriteStartAttribute(prefix, localName, ns);
        }

        public override void WriteString(string text)
        {
            if (skipAttribute)
            {
                return;
            }
            base.WriteString(text);
        }

        public override void WriteEndAttribute()
        {
            if (skipAttribute)
            {
                skipAttribute = false;
                return;
            }
            base.WriteEndAttribute();
        }

        #endregion
    }
}
