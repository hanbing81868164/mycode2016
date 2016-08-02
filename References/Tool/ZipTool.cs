using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZipTool
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args != null && args.Length == 2)
            {
                string path = args[0];
                string savaPath = args[1];

                if (System.IO.Directory.Exists(path))
                {
                    using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                    {
                        if (System.IO.File.Exists(savaPath))
                            System.IO.File.Delete(savaPath);

                        zip.AddDirectory(path);
                        zip.Save(savaPath);
                    }
                }
            }
        }
    }
}
