namespace System
{
    public class LogHelper
    {
        static string debugTemplate = "-----{0} {3}-----{1}{2}\r\n\r\n";

        static string logPath = Utils.GetCurrentDirectory() + "logs\\";
        static string GetFileName()
        {
            DirectoryHelper.CreateDirectory(logPath);
            return logPath + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
        }

        static void WriteLog(object message, Exception exception, string level)
        {
            string logTxt = debugTemplate.SFormat(
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                (message != null ? "\r\n" + message.ToString() : string.Empty),
                (exception != null ? "\r\n" + exception.StackTrace : string.Empty),
                level
                );

            NTxtHelper.WriteText(GetFileName(), logTxt);
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(object message)
        {
            WriteLog(message, null, "Debug");
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Debug(object message, Exception exception)
        {
            WriteLog(message, exception, "Debug");
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message"></param>
        public static void Info(object message)
        {
            WriteLog(message, null, "Info");
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Info(object message, Exception exception)
        {
            WriteLog(message, exception, "Info");
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(object message)
        {
            WriteLog(message, null, "Warn");
        }

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Warn(object message, Exception exception)
        {
            WriteLog(message, exception, "Warn");
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="message"></param>
        public static void Error(object message)
        {
            WriteLog(message, null, "Error");
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Error(object message, Exception exception)
        {
            WriteLog(message, exception, "Error");
        }

        /// <summary>
        /// 致命的
        /// </summary>
        /// <param name="message"></param>
        public static void Fatal(object message)
        {
            WriteLog(message, null, "Fatal");
        }

        /// <summary>
        /// 致命的
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void Fatal(object message, Exception exception)
        {
            WriteLog(message, exception, "Fatal");
        }
    }
}
