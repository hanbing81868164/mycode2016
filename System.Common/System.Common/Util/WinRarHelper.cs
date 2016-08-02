using Microsoft.Win32;
using System.Diagnostics;

namespace System
{
    public class WinRarHelper
    {

        public static string GetRarPath()
        {
            string rarexe;       //WinRAR.exe 的完整路径   
            RegistryKey regkey;  //注册表键   
            Object regvalue;     //键值   
            regkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe");
            regvalue = regkey.GetValue("");  // 键值为 "d:\Program Files\WinRAR\WinRAR.exe" "%1"   
            rarexe = regvalue.ToString().Replace("WinRAR.exe", "RAR.EXE");
            regkey.Close();
            return rarexe;
        }

        /// </summary> 
        /// 利用 WinRAR 进行压缩   
        /// </summary>   
        /// <param name="path">将要被压缩的文件夹（绝对路径）</param>   
        /// <param name="rarPath">压缩后的 .rar 的存放目录（绝对路径）</param>   
        /// <returns>true 或 false。压缩成功返回 true，反之，false。</returns>   
        public static bool RAR(string path, string rarPath)
        {
            bool flag = false;
            //string rarexe;       //WinRAR.exe 的完整路径   
            //RegistryKey regkey;  //注册表键   
            //Object regvalue;     //键值   
            string cmd;          //WinRAR 命令参数   
            ProcessStartInfo startinfo;
            Process process;
            try
            {
                //regkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe");
                //regvalue = regkey.GetValue("");  // 键值为 "d:\Program Files\WinRAR\WinRAR.exe" "%1"   
                //rarexe = regvalue.ToString();
                //regkey.Close();

                //rarexe = rarexe.Substring(1, rarexe.Length - 7);  // d:\Program Files\WinRAR\WinRAR.exe   
                //DirectoryHelper.CreateDirectory(rarPath);
                //压缩命令，相当于在要压缩的文件夹(path)上点右键 ->WinRAR->添加到压缩文件->输入压缩文件名(rarName)   
                //rar a temp.rar *.*


//a psd9770.zip "D:\psd9770\psd9770" -ep1 -o+ -inul -r -ibck
//D:\psd9770\



                cmd = string.Format("a \"{0}\" \"{1}\" -ep1 -r", rarPath.GetFileName(), path);
                startinfo = new ProcessStartInfo();
                startinfo.FileName = GetRarPath();
                startinfo.Arguments = cmd;                          //设置命令参数   
                startinfo.WindowStyle = ProcessWindowStyle.Hidden;  //隐藏 WinRAR 窗口   
                startinfo.WorkingDirectory = rarPath.GetDirectoryName();// path;// rarPath;
                process = new Process();
                process.StartInfo = startinfo;
                process.Start();
                process.WaitForExit(); //无限期等待进程 winrar.exe 退出   
                if (process.HasExited)
                {
                    flag = true;
                }
                process.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            return flag;
        }
        /// <summary>   
        /// 利用 WinRAR 进行解压缩   
        /// </summary>   
        /// <param name="path">文件解压路径（绝对）</param>   
        /// <param name="rarPath">将要解压缩的 .rar 文件的存放目录（绝对路径）</param>   
        /// <param name="rarName">将要解压缩的 .rar 文件名（包括后缀）</param>   
        /// <returns>true 或 false。解压缩成功返回 true，反之，false。</returns>   
        public static bool UnRAR(string path, string rarPath, string rarName)
        {
            bool flag = false;
            string rarexe;
            RegistryKey regkey;
            Object regvalue;
            string cmd;
            ProcessStartInfo startinfo;
            Process process;
            try
            {
                //regkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe");
                //regvalue = regkey.GetValue("");
                //rarexe = regvalue.ToString();
                //regkey.Close();
                //rarexe = rarexe.Substring(1, rarexe.Length - 7);
                DirectoryHelper.CreateDirectory(path);
                //解压缩命令，相当于在要压缩文件(rarName)上点右键 ->WinRAR->解压到当前文件夹   
                cmd = string.Format("x {0} {1} -y", rarName, path);
                startinfo = new ProcessStartInfo();
                startinfo.FileName = GetRarPath();// rarexe;
                startinfo.Arguments = cmd;
                startinfo.WindowStyle = ProcessWindowStyle.Hidden;
                startinfo.WorkingDirectory = rarPath;
                process = new Process();
                process.StartInfo = startinfo;
                process.Start();
                process.WaitForExit();
                if (process.HasExited)
                {
                    flag = true;
                }
                process.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            return flag;
        }

    }

    /*
RAR参数：

一、压缩命令
1、将temp.txt压缩为temp.rar rar a temp.rar temp.txt
2、将当前目录下所有文件压缩到temp.rar rar a temp.rar *.*
3、将当前目录下所有文件及其所有子目录压缩到temp.rar rar a temp.rar *.* -r
4、将当前目录下所有文件及其所有子目录压缩到temp.rar，并加上密码123rar a temp.rar *.* -r -p123

二、解压命令
1、将temp.rar解压到c:\temp目录 rar e temp.rar c:\temp rar e *.rar c:\temp(支持批量操作)
2、将temp.rar解压到c:\temp目录，并且解压后的目录结构和temp.rar中的目录结构一

压缩目录test及其子目录的文件内容
Wzzip test.zip test -r -P
WINRAR A test.rar test -r

删除压缩包中的*.txt文件
Wzzip test.zip *.txt -d
WinRAR d test.rar *.txt

刷新压缩包中的文件，即添加已经存在于压缩包中但更新的文件
Wzzip test.zip test -f
Winrar f test.rar test

更新压缩包中的文件，即添加已经存在于压缩包中但更新的文件以及新文件
Wzzip test.zip test -u
Winrar u test.rar test

移动文件到压缩包，即添加文件到压缩包后再删除被压缩的文件
Wzzip test.zip -r -P -m
Winrar m test.rar test -r

添加全部 *.exe 文件到压缩文件，但排除有 a或b
开头名称的文件
Wzzip test *.exe -xf*.* -xb*.*
WinRAR a test *.exe -xf*.* -xb*.*

加密码进行压缩
Wzzip test.zip test
-s123。注意密码是大小写敏感的。在图形界面下打开带密码的压缩文件，会看到+号标记（附图1）。
WINRAR A test.rar test -p123
-r。注意密码是大小写敏感的。在图形界面下打开带密码的压缩文件，会看到*号标记（附图2）。

按名字排序、以简要方式列表显示压缩包文件
Wzzip test.zip -vbn
Rar l test.rar

锁定压缩包，即防止未来对压缩包的任何修改
无对应命令
Winrar k test.rar

创建360kb大小的分卷压缩包
无对应命令
Winrar a -v360 test

带子目录信息解压缩文件
Wzunzip test -d
Winrar x test -r

不带子目录信息解压缩文件
Wzunzip test
Winrar e test

解压缩文件到指定目录，如果目录不存在，自动创建
Wzunzip test newfolder
Winrar x test newfolder

解压缩文件并确认覆盖文件
Wzunzip test -y
Winrar x test -y

解压缩特定文件
Wzunzip test *.txt
Winrar x test *.txt

解压缩现有文件的更新文件
Wzunzip test -f
Winrar x test -f

解压缩现有文件的更新文件及新文件
Wzunzip test -n
Winrar x test -u

批量解压缩文件
Wzunzip *.zip
WinRAR e *.rar 
     */
}
