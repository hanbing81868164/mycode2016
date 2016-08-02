using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Net;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Security.AccessControl;
using System.Linq;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace System
{
    /// <summary>
    /// 工具类
    /// </summary>
    public partial class Utils
    {

        public static Assembly LoadAssemblyFromFile(string fileFullName)
        {
            byte[] b = File.ReadAllBytes(fileFullName);
            Assembly asm = Assembly.Load(b);
            return asm;
        }


        /// <summary>
        /// 剪切
        /// </summary>
        /// <param name="b">原始图</param>
        /// <param name="startX">原始图上的起始坐标X值</param>
        /// <param name="startY">原始图上的起始坐标Y值</param>
        /// <param name="iWidth">需要的宽度</param>
        /// <param name="iHeight">需要的高度</param>
        /// <returns>结果图</returns>
        public static Bitmap Cut(Bitmap b, int startX, int startY, int iWidth, int iHeight)
        {
            if (b == null)
            {
                return null;
            }

            int w = b.Width;
            int h = b.Height;

            if (startX >= w || startY >= h)
            {
                return null;
            }

            if (startX + iWidth > w)
            {
                iWidth = w - startX;
            }

            if (startY + iHeight > h)
            {
                iHeight = h - startY;
            }

            Graphics g = null;
            try
            {
                Bitmap bmpOut = new Bitmap(iWidth, iHeight, PixelFormat.Format32bppArgb);

                g = Graphics.FromImage(bmpOut);
                g.DrawImage(b, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(startX, startY, iWidth, iHeight), GraphicsUnit.Pixel);

                return bmpOut;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (g != null)
                    g.Dispose();
            }
        }

        /// <summary>
        /// 返回定图像的小图
        /// </summary>
        /// <param name="originalImage"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        public static Stream GetThumbnail(Image originalImage, int Width, int Height)
        {
            Stream res = null;
            using (Bitmap img = new Bitmap(Width, Height))
            {
                img.SetResolution(72f, 72f);

                Graphics gdiobj = Graphics.FromImage(img);
                gdiobj.CompositingQuality = CompositingQuality.HighQuality;
                gdiobj.SmoothingMode = SmoothingMode.HighQuality;
                gdiobj.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gdiobj.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gdiobj.FillRectangle(new SolidBrush(Color.Transparent), 0, 0, Width, Height);
                Rectangle destrect = new Rectangle(0, 0, Width, Height); ;
                gdiobj.DrawImage(originalImage, destrect, 0, 0, originalImage.Width, originalImage.Height, GraphicsUnit.Pixel);

                res = img.ToMemoryStream();
                try
                {
                    originalImage.Dispose();
                    img.Dispose();
                    gdiobj.Dispose();
                }
                catch { }
            }
            return res;
        }


        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns>字符长度</returns>
        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }

        /// <summary>
        ///合并两个数组
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public byte[] CopyByte(byte[] a, byte[] b)
        {
            byte[] c = new byte[a.Length + b.Length];
            a.CopyTo(c, 0);
            b.CopyTo(c, a.Length);
            return c;
        }


        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }

        /// <summary>
        /// 返回根目录
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDirectory()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            return path.EndsWith("\\") ? path : path + "\\";
        }



        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            return GetSubString(p_SrcString, 0, p_Length, p_TailString);
        }

        #region 2011-01-13添加 卢前程
        /// <summary>
        /// 获取字符串中指定位置开始的指定长度的字符串，支持汉字英文混合 汉字为2字节计数
        /// </summary>
        /// <param name="strSub">输入中英混合字符串</param>
        /// <param name="start">开始截取的起始位置</param>
        /// <param name="length">要截取的字符串长度</param>
        /// <returns></returns>
        public static string GetUnicodeSubString(string str, int len, string p_TailString)
        {
            int start = 0;
            string temp = str;
            int j = 0, k = 0, p = 0;

            bool overlen = false;
            CharEnumerator ce = temp.GetEnumerator();
            while (ce.MoveNext())
            {
                j += (ce.Current > 0 && ce.Current < 255) ? 1 : 2;

                if (j <= start)
                {
                    p++;
                }
                else
                {

                    if (j == temp.GetLength())
                    {
                        temp = temp.Substring(p, k + 1);
                        break;
                    }
                    if (j <= len + start)
                    {
                        k++;
                    }
                    else
                    {
                        overlen = true;
                        temp = temp.Substring(p, k);
                        break;
                    }
                }
            }

            if (overlen && !string.IsNullOrEmpty(p_TailString))
            {
                return temp + p_TailString;
            }
            return temp;
        }


        #endregion


        /// <summary>
        /// 取指定长度的字符串
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_StartIndex">起始位置</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {
            string myResult = p_SrcString;

            Byte[] bComments = Encoding.UTF8.GetBytes(p_SrcString);
            foreach (char c in Encoding.UTF8.GetChars(bComments))
            {    //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
                if ((c > '\u0800' && c < '\u4e00') || (c > '\xAC00' && c < '\xD7A3'))
                {
                    //if (System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\u0800-\u4e00]+") || System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\xAC00-\xD7A3]+"))
                    //当截取的起始位置超出字段串长度时
                    if (p_StartIndex >= p_SrcString.Length)
                        return "";
                    else
                        return p_SrcString.Substring(p_StartIndex,
                                                       ((p_Length + p_StartIndex) > p_SrcString.Length) ? (p_SrcString.Length - p_StartIndex) : p_Length);
                }
            }

            if (p_Length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > p_StartIndex)
                {
                    int p_EndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (p_StartIndex + p_Length))
                    {
                        p_EndIndex = p_Length + p_StartIndex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾

                        p_Length = bsSrcString.Length - p_StartIndex;
                        p_TailString = "";
                    }

                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = p_StartIndex; i < p_EndIndex; i++)
                    {
                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                                nFlag = 1;
                        }
                        else
                            nFlag = 0;

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
                        nRealLength = p_Length + 1;

                    bsResult = new byte[nRealLength];



                    Array.Copy(bsSrcString, p_StartIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);
                    myResult = myResult + p_TailString;
                }
            }
            return myResult;
        }

        static List<IPAddress> IPLIST = new List<IPAddress>();
        /// <summary>
        /// 取得本地所有IP地址
        /// </summary>
        /// <returns></returns>
        public static List<IPAddress> GetIPAddress()
        {
            if (IPLIST.Count == 0)
            {
                NetworkInterface[] NetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface nif in NetworkInterfaces)
                {
                    IPInterfaceProperties IPPS = nif.GetIPProperties();
                    UnicastIPAddressInformationCollection UIPIC = IPPS.UnicastAddresses;
                    foreach (UnicastIPAddressInformation UIP in UIPIC)
                    {
                        if (UIP.Address.AddressFamily == Net.Sockets.AddressFamily.InterNetwork)
                        {
                            if (UIP.Address != null)
                                IPLIST.Add(UIP.Address);
                        }
                    }
                }
            }
            return IPLIST;
        }

        /// <summary>
        /// 返回指定主机的 Internet 协议 (IP) 地址。
        /// </summary>
        /// <param name="hostNameOrAddress">要解析的主机名或 IP 地址</param>
        /// <returns></returns>
        public static string GetHostAddresses(string hostNameOrAddress)
        {
            try
            {
                //IPAddress[] ips = Dns.GetHostAddresses(hostNameOrAddress);
                //return ips[ips.Length-1].ToString();//返回IPV4地址,如果是win7及以上第一个为IPV6
                return GetHostAddressesV4(hostNameOrAddress);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GetHostAddressesV4(string hostNameOrAddress)
        {
            try
            {
                IPAddress[] ips = Dns.GetHostAddresses(hostNameOrAddress);
                return ips.Where((p) => { return p.AddressFamily == Net.Sockets.AddressFamily.InterNetwork; }).FirstOrDefault().ToString();
                //return ips[ips.Length - 1].ToString();//返回IPV4地址,如果是win7及以上第一个为IPV6
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GetHostAddressesV6(string hostNameOrAddress)
        {
            try
            {
                IPAddress[] ips = Dns.GetHostAddresses(hostNameOrAddress);
                return ips.Where((p) => { return p.AddressFamily == Net.Sockets.AddressFamily.InterNetworkV6; }).FirstOrDefault().ToString();
                //return ips[ips.Length - 1].ToString();//返回IPV4地址,如果是win7及以上第一个为IPV6
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 设置目录权限
        /// </summary>
        /// <param name="FileName">f:/k //目标目录</param>
        /// <param name="Account">Administrator";//用户名</param>
        /// <param name="UserRights">"RCFW";//权限字符串，自己定义的</param>
        public static void AddDirectorySecurity(string FileName, string Account, string UserRights)
        {
            FileSystemRights Rights = new FileSystemRights();

            if (string.IsNullOrEmpty(UserRights) || UserRights.ToUpper() == "ALL")
                UserRights = "RCFW";


            if (UserRights.IndexOf("R") >= 0)
            {
                Rights = Rights | FileSystemRights.Read;
            }
            if (UserRights.IndexOf("C") >= 0)
            {
                Rights = Rights | FileSystemRights.ChangePermissions;
            }
            if (UserRights.IndexOf("F") >= 0)
            {
                Rights = Rights | FileSystemRights.FullControl;
            }
            if (UserRights.IndexOf("W") >= 0)
            {
                Rights = Rights | FileSystemRights.Write;
            }
            bool ok;
            DirectoryInfo dInfo = new DirectoryInfo(FileName);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            InheritanceFlags iFlags = new InheritanceFlags();
            iFlags = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;
            FileSystemAccessRule AccessRule2 = new FileSystemAccessRule(Account, Rights, iFlags, PropagationFlags.None, AccessControlType.Allow);
            dSecurity.ModifyAccessRule(AccessControlModification.Add, AccessRule2, out ok);

            dInfo.SetAccessControl(dSecurity);

            //列出目标目录所具有的权限
            //DirectorySecurity sec = Directory.GetAccessControl(FileName, AccessControlSections.All);
            //foreach (FileSystemAccessRule rule in sec.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount)))
            //{
            //    if ((rule.FileSystemRights & FileSystemRights.Read) != 0)
            //        Console.WriteLine(rule.FileSystemRights.ToString());
            //}
            //Console.Read();
        }



        /// <summary>
        /// 获得指定Url参数的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string strName)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
                return "";

            return HttpContext.Current.Request.QueryString[strName];
        }

        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(string strName)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
                return "";

            return HttpContext.Current.Request.Form[strName];
        }

        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的int类型值</returns>
        public static int GetQueryInt(string strName)
        {
            return TypeConverter.StrToInt(HttpContext.Current.Request.QueryString[strName], 0);
        }


        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static int GetQueryInt(string strName, int defValue)
        {
            return TypeConverter.StrToInt(HttpContext.Current.Request.QueryString[strName], defValue);
        }


        /// <summary>
        /// 获得指定表单参数的int类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的int类型值</returns>
        public static int GetFormInt(string strName, int defValue)
        {
            return TypeConverter.StrToInt(HttpContext.Current.Request.Form[strName], defValue);
        }

        /// <summary>
        /// 返回图片宽和高,如：120,90
        /// </summary>
        /// <param name="Stream"></param>
        /// <returns></returns>
        public static string GetImageWidthHeight(Stream Stream)
        {
            string rtxt = "0,0";
            using (Image originalImage = Image.FromStream(Stream))
            {
                if (originalImage != null)
                {
                    rtxt = originalImage.Width.ToString() + "," + originalImage.Height;
                    originalImage.Dispose();
                }
            }
            return rtxt;
        }


        /// <summary>
        /// 返回图片宽和高,如：120,90
        /// </summary>
        /// <param name="imgpath"></param>
        /// <returns></returns>
        public static string GetImageWidthHeight(string imagePath)
        {
            return GetImageWidthHeight(FileHelper.GetStream(imagePath));
        }


        /// <summary>
        /// 合并网页路径成绝对路径
        /// 
        /// absolutePath=http://www.weibobaomu.net/ComAlipay/orderlist.aspx
        /// relativePath=../style/modelpopup.css
        /// 返回：http://www.weibobaomu.net/style/modelpopup.css
        /// 
        /// </summary>
        /// <param name="absolutePath">绝对路径,如:http://www.weibobaomu.net/ComAlipay/orderlist.aspx </param>
        /// <param name="relativePath">相对路径,如:../style/modelpopup.css</param>
        /// <returns></returns>
        public static string GetWebAbsolutePath(string absolutePath, string relativePath)
        {
            //已经是绝对路径直接返回
            if (relativePath.StartsWith("http://", true, System.Globalization.CultureInfo.CurrentCulture) ||
                relativePath.StartsWith("https://", true, System.Globalization.CultureInfo.CurrentCulture))
            {
                return relativePath;
            }

            absolutePath = absolutePath.Split('?')[0];

            //直接指向根目录
            if (relativePath.StartsWith("/") || relativePath.StartsWith("\\"))
            {
                return absolutePath.GetDomain() + relativePath;
            }

            if (!absolutePath.GetExtension().IsNullOrEmpty())
            {
                absolutePath = Regex.Replace(absolutePath, absolutePath.GetFileName(), string.Empty, RegexOptions.IgnoreCase);
            }

            relativePath = Regex.Replace(relativePath, "/./", "/", RegexOptions.IgnoreCase);
            if (relativePath.StartsWith("./") || relativePath.StartsWith(".\\"))
                relativePath = relativePath.Substring(2);
            Uri Uri = new Uri(System.IO.Path.Combine(absolutePath, relativePath));

            return Uri.AbsoluteUri;
        }

        /// <summary>
        /// 判断指定端口是否被占用
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool PortInUse(int port)
        {
            bool inUse = false;
            try
            {
                IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
                IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

                foreach (IPEndPoint endPoint in ipEndPoints)
                {
                    if (endPoint.Port == port)
                    {
                        inUse = true;
                        break;
                    }
                }
            }
            catch { }
            return inUse;
        }

        /// <summary>
        /// 返回枚举的Attribute属性值，目前只支持DisplayNameAttribute，DefaultValueAttribute，DescriptionAttribute
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumerationValue"></param>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        public static object GetEnumAttribute(Enum enumerationValue, Type attributeType)
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue必须是一个枚举值", "enumerationValue");
            }

            //使用反射获取该枚举的成员信息
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(attributeType, false);

                if (attrs != null && attrs.Length > 0)
                {
                    //返回枚举值得描述信息
                    if (attrs[0] is DisplayNameAttribute)
                        return ((DisplayNameAttribute)attrs[0]).DisplayName;
                    else if (attrs[0] is DefaultValueAttribute)
                        return ((DefaultValueAttribute)attrs[0]).Value;
                    else if (attrs[0] is DescriptionAttribute)
                        return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //如果没有描述特性的值，返回该枚举值得字符串形式
            return null;
        }


        public static object GetPropertyAttribute(object o, string fieldName, Type attributeType)
        {
            var property = o.GetType().GetProperty(fieldName);
            if (property != null)
            {
                Attribute attribute = Attribute.GetCustomAttribute(property, attributeType);
                if (attribute != null)
                {
                    //返回枚举值得描述信息
                    if (attribute is DisplayNameAttribute)
                        return ((DisplayNameAttribute)attribute).DisplayName;
                    else if (attribute is DefaultValueAttribute)
                        return ((DefaultValueAttribute)attribute).Value;
                    else if (attribute is DescriptionAttribute)
                        return ((DescriptionAttribute)attribute).Description;
                }
            }
            return null;
        }

        public static string GetCurrentDateTime()
        {
            return DateTime.Now.ToDefaultString();
        }


        static string[] numbers = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
        public static string ConvertChineseCharacters(string number)
        {
            string res = string.Empty;
            char[] chars = number.ToCharArray();
            foreach (char c in chars)
            {
                int i = 0;
                if (int.TryParse(c.ToString(), out i))
                    res += numbers[int.Parse(c.ToString())];
                else
                    res += c.ToString();
            }
            return res;
        }



        // <summary>
        /// base64编码的文本转为图片
        /// </summary>
        /// <param name="txtFileName">保存的路径加文件名</param>
        /// <param name="inputStr">要转换的文本</param>
        public static void Base64StringSaveImage(string txtFileName, string inputStr)
        {
            try
            {
                Bitmap bmp = Base64StringToBitmap(inputStr);

                ImageFormat iFormat = ImageFormat.Jpeg;
                txtFileName = txtFileName.ToLower();

                if (txtFileName.EndsWith(".png"))
                    iFormat = ImageFormat.Png;
                else if (txtFileName.EndsWith(".gif"))
                    iFormat = ImageFormat.Gif;

                bmp.Save(txtFileName, iFormat);
            }
            catch (Exception ex)
            {
            }
        }

        public static Bitmap Base64StringToBitmap(string inputStr)
        {
            try
            {
                Bitmap bmp = null;
                byte[] arr = Convert.FromBase64String(inputStr);
                using (MemoryStream ms = new MemoryStream(arr))
                {
                    bmp = new Bitmap(ms);
                }
                return bmp;
            }
            catch (Exception ex)
            {
            }
            return null;
        }




        #region 硬件相关
        /// <summary>
        /// 获取网卡ID代码 
        /// </summary>
        /// <returns></returns>
        public static string GetNetworkAdpaterID()
        {
            //try
            //{
            //    string mac = "";
            //    ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            //    ManagementObjectCollection moc = mc.GetInstances();
            //    foreach (ManagementObject mo in moc)
            //        if ((bool)mo["IPEnabled"] == true)
            //        {
            //            mac += mo["MacAddress"].ToString() + " ";
            //            break;
            //        }
            //    moc = null;
            //    mc = null;
            //    return mac.Trim();
            //}
            //catch (Exception e)
            //{
            //    return "uMnIk";
            //}

            return NetworkAdpaterHelper.GetMacAddressByNetBios();
        }

        /// <summary>
        /// 返回Cpu序列号
        /// </summary>
        /// <returns></returns>
        public static String GetCpuID()
        {
            //try
            //{
            //    ManagementClass mc = new ManagementClass("Win32_Processor");
            //    ManagementObjectCollection moc = mc.GetInstances();

            //    String strCpuID = null;
            //    foreach (ManagementObject mo in moc)
            //    {
            //        strCpuID = mo.Properties["ProcessorId"].Value.ToString();
            //        break;
            //    }
            //    return strCpuID;
            //}
            //catch (Exception e)
            //{
            //    return "";
            //}
            return string.Empty;
        }
        #endregion


        /// <summary>
        /// 调用如：this.Label1.Text = GetStaticPageNumbers(30000, page, 10, "/pic/88", 8);
        /// </summary>
        /// <param name="Reccount">总数</param>
        /// <param name="curPage">pageindex</param>
        /// <param name="PageSize">pagesize</param>
        /// <param name="url">地址</param>
        /// <param name="extendPage">显示几个页码</param>
        /// <returns></returns>
        public static string GetStaticPageNumbersHtml(int Reccount, int curPage, int PageSize, string url, int extendPage)
        {
            int startPage = 1;
            int endPage = 1;

            int countPage = 0;

            if (Reccount % PageSize == 0)
            {
                countPage = Reccount / PageSize;
            }
            else
            {
                countPage = (int)(Reccount / PageSize) + 1;
            }

            string str_PageFrist = "<a href=\"" + url + "\">|<</a>";//第一页
            string str_PagePrev = string.Empty;//上一页
            string str_PageNext = string.Empty;//下一页
            string str_PageLast = "<a href=\"" + url + "/" + countPage + "/" + Reccount + "\">>|</a>"; //最后一页

            if (curPage > 1)//计算出上一页
            {
                str_PagePrev = "<a href=\"" + url + ((curPage - 1) == 1 ? string.Empty : (string.Format("/{0}/{1}", curPage - 1, Reccount))) + "\"><</a>";
            }

            if (curPage + 1 < countPage)//计算出下一页
            {
                str_PageNext = "<a href=\"" + url + string.Format("/{0}/{1}", curPage + 1, Reccount) + "\">></a>";
            }

            if (countPage < 1) countPage = 1;
            if (extendPage < 3) extendPage = 2;

            if (countPage > extendPage)
            {
                if (curPage - (extendPage / 2) > 0)
                {
                    if (curPage + (extendPage / 2) < countPage)
                    {
                        startPage = curPage - (extendPage / 2);
                        endPage = startPage + extendPage - 1;
                    }
                    else
                    {
                        endPage = countPage;
                        startPage = endPage - extendPage + 1;
                        str_PageLast = str_PageNext = "";
                    }
                }
                else
                {
                    endPage = extendPage;
                    str_PageFrist = str_PagePrev = "";
                }
            }
            else
            {
                startPage = 1;
                endPage = countPage;
                str_PageFrist = str_PagePrev = "";
                str_PageLast = str_PageNext = "";
            }

            StringBuilder s = new StringBuilder("");

            s.Append(str_PageFrist);
            s.Append(str_PagePrev);
            for (int i = startPage; i <= endPage; i++)
            {
                if (i == curPage)
                {
                    s.Append("<span>");
                    s.Append(i);
                    s.Append("</span>");
                }
                else
                {
                    s.Append("<a href=\"");
                    s.Append(url);
                    if (i != 1)
                    {
                        s.Append("/");
                        s.Append(i);
                        s.Append("/" + Reccount);
                    }

                    s.Append("\">");
                    s.Append(i);
                    s.Append("</a>");
                }
            }
            s.Append(str_PageNext);
            s.Append(str_PageLast);

            return s.ToString();
        }


        public static string GetStaticPageNumbers(int Reccount, int curPage, int PageSize, string url, int extendPage)
        {
            int startPage = 1;
            int endPage = 1;

            int countPage = 0;

            if (Reccount % PageSize == 0)
            {
                countPage = Reccount / PageSize;
            }
            else
            {
                countPage = (int)(Reccount / PageSize) + 1;
            }

            string str_PageFrist = "<a href=\"" + url + "\">|<</a>";//第一页
            string str_PagePrev = string.Empty;//上一页
            string str_PageNext = string.Empty;//下一页
            string str_PageLast = "<a href=\"" + url + "&page=" + countPage + "&count=" + Reccount + "\">>|</a>"; //最后一页

            if (curPage > 1)//计算出上一页
            {
                str_PagePrev = "<a href=\"" + url + ((curPage - 1) == 1 ? string.Empty : (string.Format("&page={0}&count={1}", curPage - 1, Reccount))) + "\"><</a>";
            }

            if (curPage + 1 < countPage)//计算出下一页
            {
                str_PageNext = "<a href=\"" + url + string.Format("&page={0}&count={1}", curPage + 1, Reccount) + "\">></a>";
            }

            if (countPage < 1) countPage = 1;
            if (extendPage < 3) extendPage = 2;

            if (countPage > extendPage)
            {
                if (curPage - (extendPage / 2) > 0)
                {
                    if (curPage + (extendPage / 2) < countPage)
                    {
                        startPage = curPage - (extendPage / 2);
                        endPage = startPage + extendPage - 1;
                    }
                    else
                    {
                        endPage = countPage;
                        startPage = endPage - extendPage + 1;
                        str_PageLast = str_PageNext = "";
                    }
                }
                else
                {
                    endPage = extendPage;
                    str_PageFrist = str_PagePrev = "";
                }
            }
            else
            {
                startPage = 1;
                endPage = countPage;
                str_PageFrist = str_PagePrev = "";
                str_PageLast = str_PageNext = "";
            }

            StringBuilder s = new StringBuilder("");

            s.Append(str_PageFrist);
            s.Append(str_PagePrev);
            for (int i = startPage; i <= endPage; i++)
            {
                if (i == curPage)
                {
                    s.Append("<span>");
                    s.Append(i);
                    s.Append("</span>");
                }
                else
                {
                    s.Append("<a href=\"");
                    s.Append(url);
                    if (i != 1)
                    {
                        s.Append("&page=");
                        s.Append(i);
                        s.Append("&count=" + Reccount);
                    }

                    s.Append("\">");
                    s.Append(i);
                    s.Append("</a>");
                }
            }
            s.Append(str_PageNext);
            s.Append(str_PageLast);

            return s.ToString();
        }

        /// <summary>  
        /// 计算文件大小函数(保留两位小数),Size为字节大小  
        /// </summary>  
        /// <param name="Size">初始文件大小</param>  
        /// <returns></returns>  
        public static string GetFileSize(long Size)
        {
            string m_strSize = "";
            long FactSize = 0;
            FactSize = Size;
            if (FactSize < 1024.00)
                m_strSize = FactSize.ToString("F2") + " Byte";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " K";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " M";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";
            return m_strSize;
        }


        /**/
        /// < summary>
        /// 分析用户请求是否正常
        /// < /summary>
        /// < param name="Str">传入用户提交数据< /param>
        /// < returns>返回是否含有SQL注入式攻击代码< /returns>
        public static bool ProcessSqlStr(string Str, int type)
        {
            string SqlStr;
            if (type == 1)
                SqlStr = "exec |insert |select |delete |update |count |chr |mid |master |truncate |char |declare ";
            else
                SqlStr = "'|and|exec|insert|select|delete|update|count|*|chr|mid|master|truncate|char|declare";
            bool ReturnValue = true;
            try
            {
                if (Str != "")
                {
                    string[] anySqlStr = SqlStr.Split('|');
                    foreach (string ss in anySqlStr)
                    {
                        if (Str.IndexOf(ss) >= 0)
                        {
                            ReturnValue = false;
                        }
                    }
                }
            }
            catch
            {
                ReturnValue = false;
            }
            return ReturnValue;
        }


    }
}
