namespace System
{
    public class AssemblyHelper
    {

        /// <summary>
        /// 将指定路径下的DLL加载到当前域中
        /// </summary>
        /// <param name="assemblyFilePath"></param>
        public static void LoadAssembly(string assemblyFilePath)
        {
            Loader.AssemblyResolver.Instance().LoadAssemblyFrom(assemblyFilePath);
        }

    }
}
