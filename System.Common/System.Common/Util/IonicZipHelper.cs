using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Ionic.Zip;

namespace System
{
    /// <summary>
    /// Zip压缩与解压缩 
    /// </summary>
    public class IonicZipHelper
    {

        /// <summary>
        /// 压缩一个目录并返回文件名
        /// </summary>
        /// <param name="directoryPath">要压缩的目录</param>
        /// <param name="zipFilePath">压缩后文件存放目录</param>
        /// <param name="MaxOutputSegmentSize">单个文件大小,单位为M，默认为2048M 即2G</param>
        public static void ZipDirectory(string directoryPath, string zipFilePath)
        {
            using (ZipFile zip = new ZipFile(System.Text.Encoding.UTF8))
            {
                zipFilePath.GetDirectoryName().CreateDirectory();
                zipFilePath.DeleteFile();

                zip.AddDirectory(directoryPath); // recurses subdirectories

                //zip.MaxOutputSegmentSize = maxOutputSegmentSize * 1024 * 1024;
                //zip.CaseSensitiveRetrieval = true;

                zip.BufferSize = 1024;
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                zip.Save(zipFilePath);
            }
        }

        static void ClearZipFiles(string zipFilePath)
        {
            //不带扩展名称
            string fileName = zipFilePath.GetFileName().Replace(zipFilePath.GetExtension(), string.Empty);
            for (int i = 1; i < 100; i++)
            {
                string newFileName = fileName + ".z" + (i < 10 ? "0" + i.ToString() : i.ToString());
                (zipFilePath.GetDirectoryName() + "\\" + newFileName).DeleteFile();
            }
        }

        static void GetFileSize(string filePath, ref long fileSize, ref List<string> files)
        {
            if (File.Exists(filePath))
            {
                FileInfo f = new FileInfo(filePath);
                fileSize += f.Length;
                files.Add(filePath);
            }
        }

        static void ManageFile(string zipFilePath, ref long fileSize, ref List<string> files)
        {
            GetFileSize(zipFilePath, ref  fileSize, ref  files);

            //不带扩展名称
            string fileName = zipFilePath.GetFileName().Replace(zipFilePath.GetExtension(), string.Empty);
            for (int i = 1; i < 100; i++)
            {
                string newFileName = fileName + ".z" + (i < 10 ? "0" + i.ToString() : i.ToString());
                string filePath = (zipFilePath.GetDirectoryName() + "\\" + newFileName);

                GetFileSize(filePath, ref  fileSize, ref  files);
            }
        }

        public static object lock_obj = new object();
        public static void ZipDirectory(ref long fileSize, ref List<string> files, string directoryPath, string zipFilePath, int maxOutputSegmentSize = 2000)
        {
            ClearZipFiles(zipFilePath);

            long size = 0;

            ZipFile zip = new ZipFile(System.Text.Encoding.UTF8);
            try
            {
                //using (ZipFile zip = new ZipFile(System.Text.Encoding.UTF8))
                //{
                //zipFilePath.GetDirectoryName().DeleteDirectory();
                zipFilePath.GetDirectoryName().CreateDirectory();
                zipFilePath.DeleteFile();

                if (System.IO.Directory.Exists(directoryPath))
                {
                    lock (lock_obj)
                    {
                        zip.AddDirectory(directoryPath); // recurses subdirectories
                        zip.MaxOutputSegmentSize = maxOutputSegmentSize * 1024 * 1024;
                        zip.BufferSize = 1024;
                        zip.CaseSensitiveRetrieval = true;
                        zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                        zip.Save(zipFilePath);
                        zip.Dispose();
                        zip = null;
                    }
                }
                //}
            }
            catch (Exception ex)
            {
                zip.Dispose();
                zip = null;
            }
            ManageFile(zipFilePath, ref fileSize, ref files);
        }



    }
}
