using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;



namespace System
{
    /// <summary>
    /// 企业应用框架的日志类
    /// </summary>
    /// <remarks>此日志类提供高性能的日志记录实现。
    /// 当调用Write方法时不会造成线程阻塞,而是立即完成方法调用,因此调用线程不用等待日志写入文件之后才返回。</remarks>
    public class NTxt : IDisposable
    {
        //日志对象的缓存队列
        public Queue<string> msgs;
        //日志文件保存的路径
        private string path;
        //日志写入线程的控制标记
        private bool state;
        //日志文件写入流对象
        private StreamWriter writer;

        private Encoding encoding;

        /// <summary>
        /// 最后一次操作文件时间
        /// </summary>
        public DateTime LastShowTime { get; set; }

        /// <summary>
        /// 创建日志对象的新实例，采用默认当前程序位置作为日志路径和默认的每日日志文件类型记录日志
        /// </summary>
        public NTxt(string _path)
            : this(_path, Encoding.UTF8)
        { }

        Thread thread = null;
        public NTxt(string _path, Encoding _encoding)
        {
            DirectoryHelper.CreateDirectory(_path.GetDirectoryName());

            if (msgs == null)
            {
                state = true;
                path = _path;
                encoding = _encoding;
                msgs = new Queue<string>();
                thread = new Thread(work);
                thread.IsBackground = true;
                thread.Start();
            }
        }

        //日志文件写入线程执行的方法
        private void work()
        {
            while (true)
            {
                //判断队列中是否存在待写入的日志
                if (msgs.Count > 0)
                {
                    string msg = null;
                    lock (msgs)
                    {
                        msg = msgs.Dequeue();
                    }
                    if (msg != null)
                    {
                        FileWrite(msg);
                    }
                }
                else
                {
                    //判断是否已经发出终止日志并关闭的消息
                    if (state)
                    {
                        Thread.Sleep(1);
                    }
                    else
                    {
                        Close();
                    }
                }
            }
        }

        //写入日志文本到文件的方法
        private void FileWrite(string msg)
        {
            try
            {
                if (writer == null)
                {
                    Open();
                }
                writer.Write(msg);
                writer.Flush();
            }
            catch (Exception e)
            {
                //Console.Out.Write(e);
            }

            this.LastShowTime = DateTime.Now;
        }

        //打开文件准备写入
        private void Open()
        {
            writer = new StreamWriter(path, true, encoding);
        }

        //关闭打开的日志文件
        public void Close()
        {
            if (writer != null)
            {
                writer.Flush();
                writer.Close();
                writer.Dispose();
                writer = null;
            }
        }

        /// <summary>
        /// 写入新日志，根据指定的日志对象Msg
        /// </summary>
        /// <param name="msg">日志内容对象</param>
        public void Write(string msg)
        {
            if (msg != null)
            {
                lock (msgs)
                {
                    msgs.Enqueue(msg);
                }
            }
        }

        #region IDisposable 成员

        /// <summary>
        /// 销毁日志对象
        /// </summary>
        public void Dispose()
        {
            state = false;
        }

        #endregion
    }
}


namespace System
{
    /// <summary>
    /// 日志类型的枚举
    /// </summary>
    /// <remarks>日志类型枚举指示日志文件创建的方式，如果日志比较多可考虑每天创建一个日志文件
    /// 如果日志量比较小可考虑每周、每月或每年创建一个日志文件</remarks>
    public enum LogType
    {
        /// <summary>
        /// 此枚举指示每天创建一个新的日志文件
        /// </summary>
        Daily,

        /// <summary>
        /// 此枚举指示每周创建一个新的日志文件
        /// </summary>
        Weekly,

        /// <summary>
        /// 此枚举指示每月创建一个新的日志文件
        /// </summary>
        Monthly,

        /// <summary>
        /// 此枚举指示每年创建一个新的日志文件
        /// </summary>
        Annually
    }
}

namespace System
{
    /// <summary>
    /// 表示一个日志记录的对象
    /// </summary>
    public class Msg
    {
        //日志记录的时间
        private DateTime datetime;
        //日志记录的内容
        private string text;
        //日志记录的类型
        private MsgType type;

        /// <summary>
        /// 创建新的日志记录实例;日志记录的内容为空,消息类型为MsgType.Unknown,日志时间为当前时间
        /// </summary>
        public Msg()
            : this("", MsgType.Unknown)
        {
        }

        /// <summary>
        /// 创建新的日志记录实例;日志事件为当前时间
        /// </summary>
        /// <param name="t">日志记录的文本内容</param>
        /// <param name="p">日志记录的消息类型</param>
        public Msg(string t, MsgType p)
            : this(DateTime.Now, t, p)
        {
        }

        /// <summary>
        /// 创建新的日志记录实例;
        /// </summary>
        /// <param name="dt">日志记录的时间</param>
        /// <param name="t">日志记录的文本内容</param>
        /// <param name="p">日志记录的消息类型</param>
        public Msg(DateTime dt, string t, MsgType p)
        {
            datetime = dt;
            type = p;
            text = t;
        }

        /// <summary>
        /// 获取或设置日志记录的时间
        /// </summary>
        public DateTime Datetime
        {
            get { return datetime; }
            set { datetime = value; }
        }

        /// <summary>
        /// 获取或设置日志记录的文本内容
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// 获取或设置日志记录的消息类型
        /// </summary>
        public MsgType Type
        {
            get { return type; }
            set { type = value; }
        }

        public new string ToString()
        {
            return datetime.ToString() + "\t" + text + "\n";
        }
    }
}

namespace System
{
    /// <summary>
    /// 日志消息类型的枚举
    /// </summary>
    public enum MsgType
    {
        /// <summary>
        /// 指示未知信息类型的日志记录
        /// </summary>
        Unknown,

        /// <summary>
        /// 指示普通信息类型的日志记录
        /// </summary>
        Information,

        /// <summary>
        /// 指示警告信息类型的日志记录
        /// </summary>
        Warning,

        /// <summary>
        /// 指示错误信息类型的日志记录
        /// </summary>
        Error,

        /// <summary>
        /// 指示成功信息类型的日志记录
        /// </summary>
        Success
    }
}
