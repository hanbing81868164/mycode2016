using System.Collections.Generic;
using System.IO;

namespace System
{
    public class DirectoryHelper
    {
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool Exists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// 创建文件夹,存在则不创建
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDirectory(string path)
        {
            if (!Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 返回指定目录下的文件名,包含地址
        /// </summary>
        /// <param name="path">目录地址</param>
        /// <param name="type_">文件类型,如:*.jpg或null或""</param>
        /// <returns></returns>
        public static string[] GetFiles(string path, string searchPattern)
        {
            if (Exists(path))
            {
                if (string.IsNullOrEmpty(searchPattern))
                {
                    return Directory.GetFiles(path);
                }
                else
                {
                    return Directory.GetFiles(path, searchPattern);
                }
            }
            return null;
        }

        /// <summary>
        /// 删除文件来
        /// </summary>
        /// <param name="path"></param>
        public static void Delete(string path)
        {
            if (path.IsNullOrEmpty())
                return;

            DeleteDirectories(path);

            //if (Exists(path))
            //{
            //    try
            //    {
            //        System.IO.Directory.Delete(path, true);
            //    }
            //    catch { }

            //    //string[] fiels = System.IO.Directory.GetFiles(path);
            //    //foreach (string file in fiels)//删除文件
            //    //{
            //    //    System.IO.File.Delete(file);
            //    //}

            //    //string[] directorys = System.IO.Directory.GetDirectories(path);
            //    //foreach (string directory in directorys)
            //    //{
            //    //    Delete(directory);
            //    //}

            //    //System.IO.Directory.Delete(path, true);
            //}
        }


        public static bool DeleteDirectories(string root)
        {
            bool removed = false;

            var dirs = new Stack<string>();
            var emptyDirStack = new Stack<string>();
            var emptyDirs = new Dictionary<string, int>();

            if (!Directory.Exists(root))
            {
                return true;
                //throw new ArgumentException();
            }

            foreach (string f in Directory.GetFiles(root, "*", SearchOption.AllDirectories))
            {
                f.DeleteFile();
            }

            dirs.Push(root);

            while (dirs.Count > 0)
            {
                string currentDir = dirs.Pop();

                string[] subDirs;
                try
                {
                    subDirs = Directory.GetDirectories(currentDir);
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                if (Directory.GetFiles(currentDir).Length == 0)
                {
                    emptyDirStack.Push(currentDir);
                    emptyDirs.Add(currentDir, subDirs.Length); // add directory path and number of sub directories
                }

                // Push the subdirectories onto the stack for traversal.
                foreach (string str in subDirs)
                    dirs.Push(str);
            }

            while (emptyDirStack.Count > 0)
            {
                string currentDir = emptyDirStack.Pop();
                if (emptyDirs[currentDir] == 0)
                {
                    string parentDir = Directory.GetParent(currentDir).FullName;
                    Console.WriteLine(currentDir); // comment this line
                    if (Exists(currentDir))
                    {
                        try {
                            Directory.Delete(currentDir, true); // uncomment this line to delete
                        }
                        catch { }
                    }

                    if (emptyDirs.ContainsKey(parentDir))
                    {
                        emptyDirs[parentDir]--; // decrement number of subdirectories of parent
                    }
                    removed = true;
                }
            }

            return removed;
        }


        /// <summary>
        /// 修改文件名称
        /// </summary>
        /// <param name="path"></param>
        /// <param name="newpath"></param>
        public static void Move(string sourceDirName, string destDirName)
        {
            if (Exists(sourceDirName))
            {
                Directory.Move(sourceDirName, destDirName);
            }
        }

        /// <summary> 
        /// 复制文件夹（及文件夹下所有子文件夹和文件） 
        /// </summary> 
        /// <param name="sourcePath">待复制的文件夹路径</param> 
        /// <param name="destinationPath">目标路径</param> 
        public static void CopyDirectory(String sourcePath, String destinationPath)
        {
            if (!Exists(sourcePath))
                return;

            DirectoryInfo info = new DirectoryInfo(sourcePath);
            CreateDirectory(destinationPath);

            foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
            {
                String destName = Path.Combine(destinationPath, fsi.Name);

                if (fsi is System.IO.FileInfo)          //如果是文件，复制文件 
                {
                    File.Copy(fsi.FullName, destName, true);//复制新文件
                }
                else                                    //如果是文件夹，新建文件夹，递归 
                {
                    CreateDirectory(destName);
                    CopyDirectory(fsi.FullName, destName);
                }
            }
        }
    }
}
