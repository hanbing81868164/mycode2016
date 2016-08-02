using System.IO;

namespace System
{
    public class FileMonitor : IDisposable
    {

        int TimeoutMillis = 2000;
        FileSystemWatcher fsw = null;
        WatcherTimer watcher = null;
        Action<FileSystemEventArgs> action = null;
        Action action2 = null;

        public FileMonitor()
        {
        }

        public void AddFileWatcher(string filePath, Action<FileSystemEventArgs> act)
        {
            action = act;
            watcher = new WatcherTimer(fsw_Changed, TimeoutMillis);
            fsw = new FileSystemWatcher(Path.GetDirectoryName(filePath), Path.GetFileName(filePath));

            fsw.Created += new FileSystemEventHandler(watcher.OnFileChanged);
            fsw.Changed += new FileSystemEventHandler(watcher.OnFileChanged);
            fsw.EnableRaisingEvents = true;
        }

        public void AddFileWatcher(string filePath, Action act)
        {
            action2 = act;
            watcher = new WatcherTimer(fsw_Changed, TimeoutMillis);
            fsw = new FileSystemWatcher(Path.GetDirectoryName(filePath), Path.GetFileName(filePath));

            fsw.Created += new FileSystemEventHandler(watcher.OnFileChanged);
            fsw.Changed += new FileSystemEventHandler(watcher.OnFileChanged);
            fsw.EnableRaisingEvents = true;
        }

        void fsw_Changed(object sender, FileSystemEventArgs e)
        {
            if (action != null)
                action(e);
            if (action2 != null)
                action2();
        }


        public void Dispose()
        {
            if (fsw != null)
            {
                fsw.EnableRaisingEvents = false;
                fsw.Dispose();
                fsw = null;
            }

            if (watcher != null)
            {
                watcher.Dispose();
                watcher = null;
            }

            action = null;
            action2 = null;
        }
    }
}
