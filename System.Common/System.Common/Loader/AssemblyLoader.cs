using System.IO;
using System.Reflection;

namespace System.Loader
{
    /// <summary>
    /// 程序集加载类（用于加载程序集、获取文件URI）
    /// </summary>
    public class AssemblyLoader
    {
        /// <summary>
        /// 加载程序集
        /// <code>
        /// assemblyInfo = AssemblyLoader.Load(assemblyInfo);
        /// </code>
        /// </summary>
        public static AssemblyInfo Load(AssemblyInfo assemblyInfo)
        {
            if (File.Exists(assemblyInfo.AssemblyUri.LocalPath))
            {
                assemblyInfo.Assembly = Assembly.LoadFrom(assemblyInfo.AssemblyUri.LocalPath);
                return assemblyInfo;
            }
            else
            {
                assemblyInfo.Assembly =null;
                return assemblyInfo;
            }

            /*
            #region 从内存中加载
            if (assemblyInfo.AssemblyBuffer == null)
            {
                
                if (File.Exists(assemblyInfo.AssemblyUri.LocalPath))
                {
                    byte[] buffer;
                    using (FileStream imageStream = new FileStream(assemblyInfo.AssemblyUri.LocalPath, FileMode.Open,FileAccess.Read))
                    {
                        long bytestreamMaxLength = imageStream.Length;
                        buffer = new byte[bytestreamMaxLength];
                        imageStream.Read(buffer, 0, (int)bytestreamMaxLength);
                    }
                    assemblyInfo.AssemblyBuffer = buffer;
                    assemblyInfo.Assembly = Assembly.Load(assemblyInfo.AssemblyBuffer);
                    assemblyInfo.AssemblyBuffer = null;
                    return assemblyInfo;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                assemblyInfo.Assembly = Assembly.Load(assemblyInfo.AssemblyBuffer);
                return assemblyInfo;
            }
            #endregion
             */ 


        }
       
        /// <summary>
        /// 获取文件URI
        /// <code>
        /// Uri assemblyUri = AssemblyLoader.GetFileUri(assemblyFilePath);
        /// </code>
        /// </summary>
        /// <param name="filePath">文件完整路径</param>
        /// <returns>URI</returns>
        public static Uri GetFileUri(string filePath)
        {
            if (String.IsNullOrEmpty(filePath))
            {
                return null;
            }
            Uri uri;
            if (!Uri.TryCreate(filePath, UriKind.Absolute, out uri))
            {
                return null;
            }
            if (!uri.IsFile)
            {
                return null;
            }
            return uri;
        }
    }
}
