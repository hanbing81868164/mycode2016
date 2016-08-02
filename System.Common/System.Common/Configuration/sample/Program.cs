using System;
using System.Collections.Generic;
using System.Text;
using Config = System.Configuration;




namespace Public.Common.Test
{
    public class gc
    {
        public gc(){
            Main();
        }

        void Main()
        {
            // The object that represents our email server's settings
            EmailServerSettings anEmailServer;

            // Get the section that has an external configuration file
            anEmailServer = XmlSection<EmailServerSettings>.GetSection("EmailSettings");
            Console.WriteLine(anEmailServer);

            // Update the section, refresh and load it again
            Config.Configuration c = Config.ConfigurationManager.OpenExeConfiguration(Config.ConfigurationUserLevel.None);
            XmlSection<EmailServerSettings>.GetSection("EmailSettings", c).Name = "Hello";
            c.Save();
            Config.ConfigurationManager.RefreshSection("EmailSettings");
            anEmailServer = XmlSection<EmailServerSettings>.GetSection("EmailSettings");
            Console.WriteLine(anEmailServer);

            // Get settings that are specified inline
            anEmailServer = XmlSection<EmailServerSettings>.GetSection("ProxyEmailServer");
            Console.WriteLine(anEmailServer);

            Console.ReadKey();            
        }

    }
}
