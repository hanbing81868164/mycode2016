namespace System
{
    /*
        System.Common.FileMonitorHelper.AddFileMonitor(@"d:\1.txt", this.CH);
        System.Common.FileMonitorHelper.AddFileMonitor(@"d:\1.txt", this.CH2);

        void CH()
        {
            var test = 0;
        }

        void CH2()
        {
            var test = 0;
        }
     
        System.Common.FileMonitorHelper.RemoveFileMonitor(this.CH);
     
     */
    public class FileMonitorHelper
    {
        static GenericCache<Action, FileMonitor> FileMonitorCache = new GenericCache<Action, FileMonitor>();
        //public static void AddFileMonitor(string filePath, Action<FileSystemEventArgs> act)
        //{
        //    string key = filePath.ToLower() + "ActionFile";
        //    if (!FileMonitorCache.ContainsKey(key))
        //    {
        //        var fileMonitor = new FileMonitor();
        //        fileMonitor.AddFileWatcher(filePath, act);
        //        FileMonitorCache.Add(key, fileMonitor);
        //    }
        //}

        public static void AddFileMonitor(string filePath, Action act)
        {
            if (!FileMonitorCache.ContainsKey(act))
            {
                var fileMonitor = new FileMonitor();
                fileMonitor.AddFileWatcher(filePath, act);
                FileMonitorCache.Add(act, fileMonitor);
            }
        }

        public static void RemoveFileMonitor(Action act)
        {
            //string key = filePath.ToLower() + "ActionFile";
            //if (FileMonitorCache.ContainsKey(key))
            //{
            //    FileMonitorCache.Remove(key);
            //}

            //string key = filePath.ToLower() + "Action";
            if (FileMonitorCache.ContainsKey(act))
            {
                var fileMonitor = new FileMonitor();
                if (FileMonitorCache.TryGetValue(act, out fileMonitor))
                {
                    fileMonitor.Dispose();
                }
                FileMonitorCache.Remove(act);
            }
        }

    }
}
