namespace System.Loader
{
    /// <summary>
    /// 程序集加载适配器（AssemblyResolver的基类）
    /// </summary>
    public interface IAssemblyResolver
    {
        void LoadAssemblyFrom(string assemblyFilePath);
    }
}
