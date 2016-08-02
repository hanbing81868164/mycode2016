
using System.Configuration;

namespace System
{
    public class ConfigurationHelper<T> : XmlSection<T> where T : class ,new()
    {
        T t = null;
        string sectionName = string.Empty;
        System.Configuration.Configuration config = null;

        public ConfigurationHelper(string sectionName)
            : this(sectionName, null)
        { }

        public ConfigurationHelper(string sectionName, string configPath)
        {
            this.sectionName = sectionName;
            if (configPath.IsNullOrEmpty())
            {
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
            else
            {
                ExeConfigurationFileMap ecfm = new ExeConfigurationFileMap()
                {
                    ExeConfigFilename = configPath
                };
                config = ConfigurationManager.OpenMappedExeConfiguration(ecfm, ConfigurationUserLevel.None);
            }

            t = GetSection(sectionName, config);
        }

        public T GetObj()
        {
            return t;
        }

        public bool Save()
        {
            config.Save(ConfigurationSaveMode.Full);
            return true;
        }

    }
}
