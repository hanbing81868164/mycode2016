namespace System
{
    public static partial class Extention
    {

        /// <summary>
        /// 根据时间返回
        /// </summary>
        /// <param name="dt">日期</param>
        /// <param name="type">类型,0表示返回英文星期,1表示返回星期的数字类型,2表示返回中文的星期</param>
        /// <returns></returns>
        public static string GetWeek(this DateTime dt, int type = 0)
        {
            switch (type)
            {
                case 0://英文星期
                    return dt.DayOfWeek.ToString();
                case 1://星期的数字类型
                    return ((int)dt.DayOfWeek).ToString();
                case 2://中文的星期
                    System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("zh-CN");
                    return cultureInfo.DateTimeFormat.GetAbbreviatedDayName(dt.DayOfWeek).ToString();
            }
            //英文星期  
            //string week = dt.DayOfWeek.ToString();//英文星期  
            //int intw = (int)dt.DayOfWeek;//数字  
            //Console.WriteLine(intw.ToString());
            //Console.WriteLine(week);

            ////中文  
            //System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("zh-CN");
            ////string week_cn = "星期" + cultureInfo.DateTimeFormat.GetAbbreviatedDayName(dt.DayOfWeek).ToString();
            //string week_cn = cultureInfo.DateTimeFormat.GetAbbreviatedDayName(dt.DayOfWeek).ToString();
            //return week_cn;
            return string.Empty;
        }



        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>double</returns>
        public static long ToDateTimeInt(this DateTime dt)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(dt - startTime).TotalSeconds;
        }

        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToDefaultString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }






        /// <summary>
        /// 把dt转成string格式(yyyy-MM-dd,如：2014-01-01)
        /// </summary>
        /// <param name="dt">要转换的DateTime</param>
        /// <param name="separator">日期分隔符，默认为-</param>
        /// <returns>转换后的字符串</returns>
        public static string DateFormat(this DateTime dt, char separator = '-')
        {
            string dateString = string.Format("{1}{0}{2}{0}{3}",
                separator,
                dt.Year,
                dt.Month.ToString().PadLeft(2, '0'),
                dt.Day.ToString().PadLeft(2, '0'));

            return dateString;
        }

        /// <summary>
        /// 把dt转成string格式(hh-mm-ss,如：08:25:06)
        /// </summary>
        /// <param name="dt">要转换的DateTime</param>
        /// <param name="separator">时间分隔符，默认为:</param>
        /// <returns>转换后的字符串</returns>
        public static string TimeFormat(this DateTime dt, char separator = ':')
        {
            string dateString = string.Format("{1}{0}{2}{0}{3}",
                separator,
                dt.Hour.ToString().PadLeft(2, '0'),
                dt.Minute.ToString().PadLeft(2, '0'),
                dt.Second.ToString().PadLeft(2, '0'));

            return dateString;
        }

        /// <summary>
        /// 把dt转成string格式(yyyy-MM-dd hh-mm-ss,如：2014-01-01 08:25:06)
        /// </summary>
        /// <param name="dt">要转换的DateTime</param>
        /// <param name="separator">日期时间分隔符 日期默认为- 时间默认为:</param>
        /// <returns>转换后的字符串</returns>
        public static string DateTimeFormat(this DateTime dt, char separator1 = '-', char separator2 = ':')
        {
            string dateString = string.Format("{2}{0}{3}{0}{4} {5}{1}{6}{1}{7}",
                separator1, separator2,
                dt.Year,
                dt.Month.ToString().PadLeft(2, '0'),
                dt.Day.ToString().PadLeft(2, '0'),
                dt.Hour.ToString().PadLeft(2, '0'),
                dt.Minute.ToString().PadLeft(2, '0'),
                dt.Second.ToString().PadLeft(2, '0'));

            return dateString;
        }





    }
}
