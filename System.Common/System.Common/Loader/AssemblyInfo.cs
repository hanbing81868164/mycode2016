using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace System.Loader
{
    /// <summary>
    /// 程序集信息（主属性有：程序集名称、程序集路径、程序集、程序集缓存等）
    /// <code>
    /// AssemblyInfo assemblyInfo = AssemblyCache.GlobalAssemblyCache.FirstOrDefault(a => AssemblyName.ReferenceMatchesDefinition(assemblyName, a.AssemblyName));
    /// </code>
    /// </summary>
     public class AssemblyInfo
    {
        /// <summary>
        /// 程序集名称
        /// </summary>
        public AssemblyName AssemblyName { get; set; }
        /// <summary>
        /// 程序集路径
        /// </summary>
        public Uri AssemblyUri { get; set; }
        /// <summary>
        /// 程序集
        /// </summary>
        public Assembly Assembly { get; set; }
        /// <summary>
        /// 程序集缓存
        /// </summary>
        public byte[] AssemblyBuffer{ get;set;}
        //应用域
       

    }
}
