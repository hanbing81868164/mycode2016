using System.Linq;
using System.IO;
using System.Reflection;

namespace System.Loader
{
    /// <summary>
    /// 程序集加载（可以采用内存加载，也可以切换成Appdomain方式加载）
    /// </summary>
    public class AssemblyResolver : SingletonBase<AssemblyResolver>, IAssemblyResolver, IDisposable
    {
        private bool handlesAssemblyResolve;
        private ResolveEventHandler resolveEventHandler;

        public AssemblyResolver() { }

        /// <summary>
        /// 加载制定路径的程序集到程序集缓存
        /// </summary>
        /// <param name="assemblyFilePath">完整的程序集路径</param>
        public void LoadAssemblyFrom(string assemblyFilePath)
        {
            resolveEventHandler = (o, e) =>
            {
                return CurrentDomain_AssemblyResolve(e);
            };
            if (!this.handlesAssemblyResolve)
            {
                AppDomain.CurrentDomain.AssemblyResolve += resolveEventHandler;
                //AppDomain.CurrentDomain
                this.handlesAssemblyResolve = true;
            }
            Uri assemblyUri = AssemblyLoader.GetFileUri(assemblyFilePath);
            if (assemblyUri == null)
            {
                throw new ArgumentException("", "assemblyFilePath");
            }
            if (!File.Exists(assemblyUri.LocalPath))
            {
                throw new FileNotFoundException();
            }
            AssemblyName assemblyName = AssemblyName.GetAssemblyName(assemblyUri.LocalPath);
            AssemblyInfo assemblyInfo = AssemblyCache.GlobalAssemblyCache.FirstOrDefault(a => AssemblyName.ReferenceMatchesDefinition(assemblyName, a.AssemblyName));
            if (assemblyInfo != null)
            {
                return;
            }

            assemblyInfo = new AssemblyInfo() { AssemblyName = assemblyName, AssemblyUri = assemblyUri };
            AssemblyCache.GlobalAssemblyCache.Add(assemblyInfo);

        }

        /// <summary>
        /// 加载ResolveEventArgs对象的程序集
        /// <code>
        /// CurrentDomain_AssemblyResolve(e)
        /// </code>
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public Assembly CurrentDomain_AssemblyResolve(ResolveEventArgs args)
        {
            AssemblyName assemblyName = new AssemblyName(args.Name);

            AssemblyInfo assemblyInfo = AssemblyCache.GlobalAssemblyCache.FirstOrDefault(a => AssemblyName.ReferenceMatchesDefinition(assemblyName, a.AssemblyName));

            if (assemblyInfo != null)
            {
                if (assemblyInfo.Assembly == null)
                {
                    assemblyInfo = AssemblyLoader.Load(assemblyInfo); //Assembly.LoadFrom(assemblyInfo.AssemblyUri.LocalPath);
                }

                return assemblyInfo.Assembly;
            }

            return null;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.handlesAssemblyResolve)
            {
                AppDomain.CurrentDomain.AssemblyResolve -= resolveEventHandler;
                this.handlesAssemblyResolve = false;
            }
        }
    }
}
