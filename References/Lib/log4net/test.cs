            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + @"config\log4net.config"));
            log4net.ILog log = log4net.LogManager.GetLogger("test");
            log.Error("aaaaaaa\r\n999");