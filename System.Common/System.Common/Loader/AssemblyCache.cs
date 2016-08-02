using System.Collections.Generic;

namespace System.Loader
{
    /// <summary>
    /// 程序集缓存类（AssemblyInfo型的List）
    /// <code>
    ///  AssemblyInfo assemblyInfo = AssemblyCache.GlobalAssemblyCache.FirstOrDefault(a => AssemblyName.ReferenceMatchesDefinition(assemblyName, a.AssemblyName));
    /// </code>
    /// </summary>
    public class AssemblyCache
    {
        public static IList<AssemblyInfo> GlobalAssemblyCache = new List<AssemblyInfo>();
    }
}
