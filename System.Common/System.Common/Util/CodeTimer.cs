using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace System
{
    public static class CodeTimer
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetThreadTimes(IntPtr hThread, out long lpCreationTime, out long lpExitTime, out long lpKernelTime, out long lpUserTime);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentThread();

        private static long GetCurrentThreadTimes()
        {
            long l;
            long kernelTime, userTimer;
            GetThreadTimes(GetCurrentThread(), out l, out l, out kernelTime,
            out userTimer);
            return kernelTime + userTimer;
        }

        static CodeTimer()
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
        }

        public static string Time(string name, int iteration, Action action)
        {
            string res = string.Empty;
            if (String.IsNullOrEmpty(name))
            {
                return res;
            }
            if (action == null)
            {
                return res;
            }
            //1. Print name
            res += name + "=>执行"+iteration+"次资源:\r\n";

            // 2. Record the latest GC counts/记录最新的GC计数
            //GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.Collect(GC.MaxGeneration);
            int[] gcCounts = new int[GC.MaxGeneration + 1];
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                gcCounts[i] = GC.CollectionCount(i);
            }
            // 3. Run action/执行行动
            Stopwatch watch = new Stopwatch();
            watch.Start();
            long ticksFst = GetCurrentThreadTimes(); //100 nanosecond one tick
            for (int i = 0; i < iteration; i++) action();
            long ticks = GetCurrentThreadTimes() - ticksFst;
            watch.Stop();

            // 4. Print CPU/打印CPU
            res += "\r\n执行时间:" + watch.ElapsedMilliseconds.ToString("N0") + "ms";
            res += "\r\n\r\n平均执行时间（一次）:" + (watch.ElapsedMilliseconds / iteration).ToString("N0") + "ms";
            res += "\r\n\r\nCPU 时间:" + (ticks * 100).ToString("N0") + "ns";
            res += "\r\n\r\nCPU 平均时间 (一次):" + (ticks * 100 / iteration).ToString("N0") + "ns";
            // 5. Print GC/打印GC
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                int count = GC.CollectionCount(i) - gcCounts[i];
                res += "\r\n\r\nGen " + i + ":" + count;
            }
            return res;
        }

    }
}
