using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*
$(TargetDir)ClearTool.exe $(ProjectDir)
*/
namespace ClearTool
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                string path = args[0];

                if (System.IO.Directory.Exists(path))
                {
                    string delPath = path + "bin";
                    DeleteFile(delPath);

                    delPath = path + "obj";
                    DeleteFile(delPath);
                }
            }
        }

        static void DeleteFile(string delPath)
        {
            try
            {
                foreach (string s in System.IO.Directory.GetFiles(delPath, "*.*", System.IO.SearchOption.AllDirectories))
                {
                    System.IO.File.Delete(s);
                }
                DeleteDirectory(delPath);
            }
            catch { }
        }
        static void DeleteDirectory(string path)
        {
            try
            {
                if (System.IO.Directory.Exists(path))
                    System.IO.Directory.Delete(path, true);
            }
            catch { }
        }

    }
}
