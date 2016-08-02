using System.Collections.Generic;
using System.Threading;
using System.IO;

namespace System
{

    public class WatcherTimer : IDisposable
    {
        private int TimeoutMillis = 2000;

        Timer m_timer = null;
        List<String> files = new List<string>();
        FileSystemEventHandler fswHandler = null;

        public WatcherTimer(FileSystemEventHandler watchHandler)
        {
            m_timer = new Timer(new TimerCallback(OnTimer), null, Timeout.Infinite, Timeout.Infinite);
            fswHandler = watchHandler;
        }


        public WatcherTimer(FileSystemEventHandler watchHandler, int timerInterval)
        {
            m_timer = new Timer(new TimerCallback(OnTimer), null, Timeout.Infinite, Timeout.Infinite);
            TimeoutMillis = timerInterval;
            fswHandler = watchHandler;
        }

        public void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            Mutex mutex = new Mutex(false, "FSW");
            mutex.WaitOne();
            if (!files.Contains(e.Name))
            {
                files.Add(e.Name);
            }
            mutex.ReleaseMutex();
            m_timer.Change(TimeoutMillis, Timeout.Infinite);
        }

        private void OnTimer(object state)
        {
            List<String> backup = new List<string>();

            Mutex mutex = new Mutex(false, "FSW");
            mutex.WaitOne();
            backup.AddRange(files);
            files.Clear();
            mutex.ReleaseMutex();

            foreach (string file in backup)
            {
                fswHandler(this, new FileSystemEventArgs(WatcherChangeTypes.Changed, string.Empty, file));
            }
        }




        public void Dispose()
        {
            if (m_timer != null)
            {
                m_timer.Dispose();
                m_timer = null;
            }
            files = null;

            if (fswHandler != null)
            {
                fswHandler = null;
            }
        }
    }
}
