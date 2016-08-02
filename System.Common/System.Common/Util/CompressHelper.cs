using SharpCompress.Archive.Zip;
using SharpCompress.Common;
using SharpCompress.Reader;
using SharpCompress.Archive;
using System.IO;

namespace System
{
    public class CompressHelper
    {
        /// <summary>
        /// 通用解压方法,可解压zip,rar
        /// SharpCompress - a fully native C# library for RAR, 7Zip, Zip, Tar, GZip, BZip2
        /// </summary>
        /// <param name="compressFilePath">待解压文件,c:\123.zip or c:\123.rar</param>
        /// <param name="compressPath">解压文件存放目录,c:\temp</param>
        /// <returns></returns>
        public static bool UnCompress(string compressFilePath, string compressPath)
        {
           string fileExtension=compressFilePath.GetExtension().ToUpper();
            //using (Stream stream = File.OpenRead(compressFilePath))
            //{
            //    using (var reader = ReaderFactory.Open(stream))
            //    {
            //        reader.WriteAllToDirectory(compressPath, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
            //    }
            //}
            //return true;

            if(fileExtension==".RAR")
            {
                using (Stream stream = File.OpenRead(compressFilePath))
                {
                    var reader = ReaderFactory.Open(stream);
                    while (reader.MoveToNextEntry())
                    {
                        if (!reader.Entry.IsDirectory)
                        {
                            reader.WriteEntryToDirectory(compressPath, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                        }
                    }
                }
            }
            else if(fileExtension==".ZIP")
            {
                var archive = ArchiveFactory.Open(compressFilePath);
                foreach (var entry in archive.Entries)
                {
                    if (!entry.IsDirectory)
                    {
                        entry.WriteToDirectory(compressPath, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 压缩目录
        /// </summary>
        /// <param name="directoryPath">待压缩目录,c:\123</param>
        /// <param name="zipFilePath">生成的压缩文件路径,c:\123.zip</param>
        /// <returns></returns>
        public static bool Compress(string directoryPath, string zipFilePath)
        {
            using (var archive = ZipArchive.Create())
            {
                archive.DeflateCompressionLevel = SharpCompress.Compressor.Deflate.CompressionLevel.Level9;
                archive.AddAllFromDirectory(directoryPath);
                archive.SaveTo(zipFilePath, CompressionType.Deflate);
            }
            return true;
        }

    }
}
