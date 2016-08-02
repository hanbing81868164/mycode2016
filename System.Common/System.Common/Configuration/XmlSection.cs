using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Configuration;

namespace System
{
    /// <summary>
    /// Represents a strongly typed XML section within a configuration file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XmlSection<T> : ConfigurationSection where T: class
    {
        private XmlSerializer serializer;

        private T configurationItem;     

        public XmlSection() { }

        #region Get section

        /// <summary>
        /// Retrieves the specified XML section for the current application's default
        /// configuration
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static T GetSection(string sectionName)
        {
            XmlSection<T> section = (XmlSection<T>)ConfigurationManager.GetSection(sectionName);
            if (null == section)
                return null;
            else
                return section.configurationItem;
        }

        /// <summary>
        /// Retrieves the specified XML section for the specified System.Configuration.Configuration object
        /// </summary>
        /// <param name="sectionName">The name of the section</param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static T GetSection(string sectionName, System.Configuration.Configuration configuration)
        {
            //ExeConfigurationFileMap ecfm = new ExeConfigurationFileMap()
            //{
            //    ExeConfigFilename = ConfigurationFilePath
            //};
            //config = ConfigurationManager.OpenMappedExeConfiguration(ecfm, ConfigurationUserLevel.None);

            XmlSection<T> section = (XmlSection<T>)configuration.GetSection(sectionName);
            if (null == section)
                return null;
            else
                return section.configurationItem;
        }

        #endregion

        #region Load configuration

        protected override void Init()
        {
            base.Init();

            // Load the serializer from the cache
            this.serializer = SerializerCache.Load(typeof(T), SectionInformation.Name);                  
        }

        /// <summary>
        /// Deseializes the XML section
        /// </summary>
        /// <param name="reader"></param>
        protected override void DeserializeSection(XmlReader reader)
        {   
            this.configurationItem = (T)serializer.Deserialize(reader);
        }

        #endregion

        #region Support for saving configuration

        /// <summary>
        /// Serializes the XML section
        /// </summary>
        /// <param name="parentElement"></param>
        /// <param name="name"></param>
        /// <param name="saveMode"></param>
        /// <returns></returns>
        protected override string SerializeSection(ConfigurationElement parentElement, string name, ConfigurationSaveMode saveMode)
        {
            XmlSectionWriter sectionWriter = new XmlSectionWriter(new StringWriter());
            serializer.Serialize(sectionWriter, configurationItem);
            return sectionWriter.ToString();            
        }

        /// <summary>
        /// Indicates whether the configuration section has changed since
        /// it was last loaded
        /// </summary>
        /// <returns></returns>
        protected override bool IsModified()
        {
            return true;
        }

        #endregion

    }

}
