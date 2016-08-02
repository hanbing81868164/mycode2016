using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace System
{
    #region 使用方法
    /*
     * 
     * 
     * AssemblyInfo.cs
[assembly: SecurityTransparent]
[assembly: AllowPartiallyTrustedCallers]
// Use the .NET Framework 2.0 transparency rules (level 1 transparency) as default
#if (CLR4)
[assembly: SecurityRules(SecurityRuleSet.Level1)]
#endif
[assembly: System.Security.SecurityRules(System.Security.SecurityRuleSet.Level1)]
     * 
     * 
     * test.cs
                CachableDynamicMethodProxyFactory fac = new CachableDynamicMethodProxyFactory();
                string methodname = "Message_" + e.MessageCode.ToUpper();
                MethodInfo mi = class.GetType().GetMethod(methodname);
                DynamicMethodProxyDelegate dmd = fac.GetMethodDelegate(mi);
                dmd(messageHandle, id, e.MessageBody);
     * 
     * 
     * 
     * 
     * 
     * 
     * 
    public class Program
    {
        //用于测试的静态方法
        public static void Add<T>(T obj, IList<T> list)
        {
            //list.Add同时也是用于测试的非静态方法
            list.Add(obj);
        }

        private const int REPEAT_TIME = 20000000;

        static void Main(string[] args)
        {
            //创建一个内置缓存支持的DynamicMethodProxyFactory实例
            NBear.DynamicMethodHelperer.DynamicMethodProxyFactory fac = new NBear.DynamicMethodHelperer.CachableDynamicMethodProxyFactory();

            //创建一个internal的System.Web.HttpDictionary类实例
            Console.WriteLine("Create an instance of internal type - System.Web.HttpDictionary.");
            object temp = fac.CreateInstance(typeof(System.Web.HttpApplication).Module, "System.Web.HttpDictionary", false, false);

            //创建访问该实例的SetValue和GetValue方法的DynamicMethosProxyDelegate
            Console.WriteLine("Create DynamicMethodProxyDelegate of HttpDictionary's SetValue and GetValue methods.");
            NBear.DynamicMethodHelperer.DynamicMethodProxyDelegate setDelegate = fac.GetMethodDelegate(typeof(System.Web.HttpApplication).Module, temp.GetType().GetMethod("SetValue", BindingFlags.NonPublic | BindingFlags.Instance));
            NBear.DynamicMethodHelperer.DynamicMethodProxyDelegate getDelegate = fac.GetMethodDelegate(typeof(System.Web.HttpApplication).Module, fac.GetMethodInfoBySignature(temp.GetType(), "System.Object GetValue(System.String)", false, false));

            //在该实例上 Set & Get 测试数据
            Console.WriteLine("set \"test value\" on the internal type instance");
            setDelegate(temp, "test", "test value");
            Console.WriteLine("get the test value from the internal type instance: " + getDelegate(temp, "test"));

            //测试DynamicMethodProxyDelegate的性能
            Console.WriteLine("Test the performance of DynamicMethodProxyDelegate.");
            List<int> list = new List<int>();
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Reset();
            watch.Start();
            for (int i = 0; i < REPEAT_TIME; i++)
            {
                //这里是直接泛型调用
                Program.Add<int>(i, list);
            }
            watch.Stop();
            long l1 = watch.ElapsedMilliseconds;
            watch.Reset();
            MethodInfo mi = typeof(Program).GetMethod("Add");
            //创建一个静态方法的StaticDynamicMethodDelegate
            NBear.DynamicMethodHelperer.StaticDynamicMethodProxyDelegate sdmd = fac.GetStaticMethodDelegate(mi, typeof(int));
            watch.Start();
            for (int i = 0; i < REPEAT_TIME; i++)
            {
                //通过StaticDynamicMethodDelegate以非泛型方式访问泛型方法
                sdmd(i, list);
            }
            watch.Stop();
            long l2 = watch.ElapsedMilliseconds;
            watch.Reset();
            MethodInfo mi2 = list.GetType().GetMethod("Add");
            //创建一个DynamicMethodDelegate访问同样的Add方法
            NBear.DynamicMethodHelperer.DynamicMethodProxyDelegate dmd = fac.GetMethodDelegate(mi2);
            watch.Start();
            for (int i = 0; i < REPEAT_TIME; i++)
            {
                //通过DynamicMethodDelegate访问
                dmd(list, i);
            }
            watch.Stop();
            long l3 = watch.ElapsedMilliseconds;
            Console.WriteLine("{0}\nDirectly vs Static vs Non-static\n{1} vs {2} vs {3}", list.Count, l1, l2, l3);
            Console.ReadLine();
        }
    }
     */
    #endregion
    /// <summary>
    /// Delegate for calling static method
    /// </summary>
    /// <param name="paramObjs">The parameters passing to the invoking method.</param>
    /// <returns>The return value.</returns>
    public delegate object StaticDynamicMethodProxyDelegate(params object[] paramObjs);

    /// <summary>
    /// Delegate for calling non-static method
    /// </summary>
    /// <param name="ownerInstance">The object instance owns the invoking method.</param>
    /// <param name="paramObjs">The parameters passing to the invoking method.</param>
    /// <returns>The return value.</returns>
    public delegate object DynamicMethodProxyDelegate(object ownerInstance, params object[] paramObjs);

    public class DynamicMethodProxyFactory
    {
        #region Helperer Methods

        protected static void LoadIndex(ILGenerator gen, int index)
        {
            switch (index)
            {
                case 0:
                    gen.Emit(OpCodes.Ldc_I4_0);
                    break;
                case 1:
                    gen.Emit(OpCodes.Ldc_I4_1);
                    break;
                case 2:
                    gen.Emit(OpCodes.Ldc_I4_2);
                    break;
                case 3:
                    gen.Emit(OpCodes.Ldc_I4_3);
                    break;
                case 4:
                    gen.Emit(OpCodes.Ldc_I4_4);
                    break;
                case 5:
                    gen.Emit(OpCodes.Ldc_I4_5);
                    break;
                case 6:
                    gen.Emit(OpCodes.Ldc_I4_6);
                    break;
                case 7:
                    gen.Emit(OpCodes.Ldc_I4_7);
                    break;
                case 8:
                    gen.Emit(OpCodes.Ldc_I4_8);
                    break;
                default:
                    if (index < 128)
                    {
                        gen.Emit(OpCodes.Ldc_I4_S, index);
                    }
                    else
                    {
                        gen.Emit(OpCodes.Ldc_I4, index);
                    }
                    break;
            }
        }

        protected static void StoreLocal(ILGenerator gen, int index)
        {
            switch (index)
            {
                case 0:
                    gen.Emit(OpCodes.Stloc_0);
                    break;
                case 1:
                    gen.Emit(OpCodes.Stloc_1);
                    break;
                case 2:
                    gen.Emit(OpCodes.Stloc_2);
                    break;
                case 3:
                    gen.Emit(OpCodes.Stloc_3);
                    break;
                default:
                    if (index < 128)
                    {
                        gen.Emit(OpCodes.Stloc_S, index);
                    }
                    else
                    {
                        gen.Emit(OpCodes.Stloc, index);
                    }
                    break;
            }
        }

        protected static void LoadLocal(ILGenerator gen, int index)
        {
            switch (index)
            {
                case 0:
                    gen.Emit(OpCodes.Ldloc_0);
                    break;
                case 1:
                    gen.Emit(OpCodes.Ldloc_1);
                    break;
                case 2:
                    gen.Emit(OpCodes.Ldloc_2);
                    break;
                case 3:
                    gen.Emit(OpCodes.Ldloc_3);
                    break;
                default:
                    if (index < 128)
                    {
                        gen.Emit(OpCodes.Ldloc_S, index);
                    }
                    else
                    {
                        gen.Emit(OpCodes.Ldloc, index);
                    }
                    break;
            }
        }

        protected static MethodInfo MakeMethodGeneric(MethodInfo genericMethodInfo, Type[] genericParameterTypes)
        {
            MethodInfo makeGenericMethodInfo;
            if (genericParameterTypes != null && genericParameterTypes.Length > 0)
            {
                makeGenericMethodInfo = genericMethodInfo.MakeGenericMethod(genericParameterTypes);
            }
            else
            {
                makeGenericMethodInfo = genericMethodInfo;
            }
            return makeGenericMethodInfo;
        }

        protected static void DeclareLocalVariablesForMethodParameters(ILGenerator il, ParameterInfo[] pis)
        {
            for (int i = 0; i < pis.Length; ++i)
            {
                il.DeclareLocal(pis[i].ParameterType);
            }
        }

        protected static void LoadParameterValues(ILGenerator il, ParameterInfo[] pis)
        {
            for (int i = 0; i < pis.Length; ++i)
            {
                LoadLocal(il, i);
            }
        }

        protected static void ParseValuesFromParametersToLocalVariables(ILGenerator il, ParameterInfo[] pis, bool isMethodStatic)
        {
            for (int i = 0; i < pis.Length; ++i)
            {
                if (isMethodStatic)
                {
                    il.Emit(OpCodes.Ldarg_0);
                }
                else
                {
                    il.Emit(OpCodes.Ldarg_1);
                }
                LoadIndex(il, i);
                il.Emit(OpCodes.Ldelem_Ref);
                if (pis[i].ParameterType.IsValueType)
                {
                    il.Emit(OpCodes.Unbox_Any, pis[i].ParameterType);
                }
                else if (pis[i].ParameterType != typeof(object))
                {
                    il.Emit(OpCodes.Castclass, pis[i].ParameterType);
                }
                StoreLocal(il, i);
            }
        }

        #endregion

        #region Get static method delegate

        protected StaticDynamicMethodProxyDelegate DoGetStaticMethodDelegate(
            Module targetModule, 
            MethodInfo genericMethodInfo, 
            params Type[] genericParameterTypes)
        {
            #region Validate parameters

            if (targetModule == null)
            {
                throw new ArgumentNullException("targetModule could not be null!");
            }

            if (genericMethodInfo == null)
            {
                throw new ArgumentNullException("genericMethodInfo to be invoke could not be null!");
            }

            if (genericParameterTypes != null)
            {
                if (genericParameterTypes.Length != genericMethodInfo.GetGenericArguments().Length)
                {
                    throw new ArgumentException("The number of generic type parameter of genericMethodInfo and the input types must equal!");
                }
            }
            else
            {
                if (genericMethodInfo.GetGenericArguments().Length > 0)
                {
                    throw new ArgumentException("Must specify types of type parameters for genericMethodInfo!");
                }
            }

            if (!genericMethodInfo.IsStatic)
            {
                throw new ArgumentException("genericMethodInfo must be static here!");
            }

            #endregion

            //Create a dynamic method proxy delegate used to call the specified methodinfo
            DynamicMethod dm = new DynamicMethod(
                Guid.NewGuid().ToString("N"), 
                typeof(object), 
                new Type[] { typeof(object[]) }, 
                targetModule);

            ILGenerator il = dm.GetILGenerator();

            #region Create local variables for all the parameters passing to the invoking method and parse values to local variables

            MethodInfo makeGenericMethodInfo = MakeMethodGeneric(genericMethodInfo, genericParameterTypes);
            ParameterInfo[] pis = makeGenericMethodInfo.GetParameters();
            DeclareLocalVariablesForMethodParameters(il, pis);
            ParseValuesFromParametersToLocalVariables(il, pis, true);

            #endregion

            #region Execute the target method

            LoadParameterValues(il, pis);

            il.Emit(OpCodes.Call, makeGenericMethodInfo);

            if (makeGenericMethodInfo.ReturnType == typeof(void))
            {
                il.Emit(OpCodes.Ldnull);
            }

            #endregion

            il.Emit(OpCodes.Ret);

            return (StaticDynamicMethodProxyDelegate)dm.CreateDelegate(typeof(StaticDynamicMethodProxyDelegate));
        }

        public virtual StaticDynamicMethodProxyDelegate GetStaticMethodDelegate(
            MethodInfo genericMethodInfo, 
            params Type[] genericParameterTypes)
        {
            return DoGetStaticMethodDelegate(typeof(string).Module, genericMethodInfo, genericParameterTypes);
        }

        public virtual StaticDynamicMethodProxyDelegate GetStaticMethodDelegate(
            Module targetModule, 
            MethodInfo genericMethodInfo, 
            params Type[] genericParameterTypes)
        {
            return DoGetStaticMethodDelegate(targetModule, genericMethodInfo, genericParameterTypes);
        }

        #endregion

        #region Get non-static method delegate

        protected DynamicMethodProxyDelegate DoGetMethodDelegate(
            Module targetModule, 
            MethodInfo genericMethodInfo, 
            params Type[] genericParameterTypes)
        {
            #region Validate parameters

            if (targetModule == null)
            {
                throw new ArgumentNullException("targetModule could not be null!");
            }

            if (genericMethodInfo == null)
            {
                throw new ArgumentNullException("genericMethodInfo to be invoke could not be null!");
            }

            if (genericParameterTypes != null)
            {
                if (genericParameterTypes.Length != genericMethodInfo.GetGenericArguments().Length)
                {
                    throw new ArgumentException("The number of generic type parameter of genericMethodInfo and the input types must equal!");
                }
            }
            else
            {
                if (genericMethodInfo.GetGenericArguments().Length > 0)
                {
                    throw new ArgumentException("Must specify types of type parameters for genericMethodInfo!");
                }
            }

            if (genericMethodInfo.IsStatic)
            {
                throw new ArgumentException("genericMethodInfo must not be static here!");
            }

            #endregion

            //Create a dynamic method proxy delegate used to call the specified methodinfo
            DynamicMethod dm = new DynamicMethod(
                Guid.NewGuid().ToString("N"), 
                typeof(object), 
                new Type[] { typeof(object), typeof(object[]) }, 
                targetModule);

            ILGenerator il = dm.GetILGenerator();

            #region Create local variables for all the parameters passing to the invoking method and parse values to local variables

            MethodInfo makeGenericMethodInfo = MakeMethodGeneric(genericMethodInfo, genericParameterTypes);
            ParameterInfo[] pis = makeGenericMethodInfo.GetParameters();
            DeclareLocalVariablesForMethodParameters(il, pis);
            ParseValuesFromParametersToLocalVariables(il, pis, false);

            #endregion

            #region Execute the target method

            il.Emit(OpCodes.Ldarg_0);

            LoadParameterValues(il, pis);

            il.Emit(OpCodes.Callvirt, makeGenericMethodInfo);

            if (makeGenericMethodInfo.ReturnType == typeof(void))
            {
                il.Emit(OpCodes.Ldnull);
            }

            #endregion

            il.Emit(OpCodes.Ret);

            return (DynamicMethodProxyDelegate)dm.CreateDelegate(typeof(DynamicMethodProxyDelegate));
        }

        public virtual DynamicMethodProxyDelegate GetMethodDelegate(
            MethodInfo genericMethodInfo, 
            params Type[] genericParameterTypes)
        {
            return DoGetMethodDelegate(typeof(string).Module, genericMethodInfo, genericParameterTypes);
        }

        public virtual DynamicMethodProxyDelegate GetMethodDelegate(
            Module targetModule, 
            MethodInfo genericMethodInfo, 
            params Type[] genericParameterTypes)
        {
            return DoGetMethodDelegate(targetModule, genericMethodInfo, genericParameterTypes);
        }

        #endregion

        #region Visit internal members

        private MethodInfo GetMethodInfoFromArrayBySignature(string signature, MethodInfo[] mis)
        {
            if (mis == null)
            {
                return null;
            }

            foreach (MethodInfo mi in mis)
            {
                if (mi.ToString() == signature)
                {
                    return mi;
                }
            }

            return null;
        }

        public MethodInfo GetMethodInfoBySignature(Type type, string signature, bool isPublic, bool isStatic)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type could not be null!");
            }

            BindingFlags flags = BindingFlags.Instance | (isPublic ? BindingFlags.Public : BindingFlags.NonPublic);
            if (isStatic)
            {
                flags = flags | BindingFlags.Static;
            }
            return GetMethodInfoFromArrayBySignature(signature, type.GetMethods(flags));
        }

        public object CreateInstance(Module targetModule, string typeFullName, bool ignoreCase, bool isPublic, Binder binder, System.Globalization.CultureInfo culture, object[] activationAttrs, params object[] paramObjs)
        {
            //get method info of Assembly.CreateInstance() method first
            MethodInfo mi = GetMethodInfoFromArrayBySignature(
                "System.Object CreateInstance(System.String, Boolean, System.Reflection.BindingFlags, System.Reflection.Binder, System.Object[], System.Globalization.CultureInfo, System.Object[])", 
                typeof(Assembly).GetMethods());

            DynamicMethodProxyDelegate dmd = GetMethodDelegate(targetModule, mi);
            return dmd(targetModule.Assembly, typeFullName, ignoreCase, BindingFlags.Instance | (isPublic ? BindingFlags.Public : BindingFlags.NonPublic), binder, paramObjs, culture, activationAttrs);
        }

        public object CreateInstance(Module targetModule, string typeFullName, bool ignoreCase, bool isPublic, params object[] paramObjs)
        {
            return CreateInstance(targetModule, typeFullName, ignoreCase, isPublic, null, null, null, paramObjs);
        }

        #endregion
    }

    public class CachableDynamicMethodProxyFactory : DynamicMethodProxyFactory
    {
        private Dictionary<string, StaticDynamicMethodProxyDelegate> cache = new Dictionary<string, StaticDynamicMethodProxyDelegate>();
        private Dictionary<string, DynamicMethodProxyDelegate> cache2 = new Dictionary<string, DynamicMethodProxyDelegate>();

        #region Get static method delegate

        public override StaticDynamicMethodProxyDelegate GetStaticMethodDelegate(
            Module targetModule, 
            MethodInfo genericMethodInfo, 
            params Type[] genericParameterTypes)
        {
            #region  Construct cache key
            
            if (targetModule == null)
            {
                throw new ArgumentNullException("targetModule could not be null!");
            }

            string key = targetModule.FullyQualifiedName + "|" + genericMethodInfo.DeclaringType.ToString() + "|" + genericMethodInfo.ToString();
            if (genericParameterTypes != null)
            {
                for (int i = 0; i < genericParameterTypes.Length; ++i)
                {
                    key += "|" + genericParameterTypes[i].ToString();
                }
            }

            #endregion

            StaticDynamicMethodProxyDelegate dmd;

            lock (cache)
            {
                if (cache.ContainsKey(key))
                {
                    dmd = cache[key];
                }
                else
                {
                    dmd = DoGetStaticMethodDelegate(targetModule, genericMethodInfo, genericParameterTypes);
                    cache.Add(key, dmd);
                }
            }

            return dmd;
        }

        public override StaticDynamicMethodProxyDelegate GetStaticMethodDelegate(
            MethodInfo genericMethodInfo, 
            params Type[] genericParameterTypes)
        {
            return GetStaticMethodDelegate(typeof(string).Module, genericMethodInfo, genericParameterTypes);
        }

        #endregion

        #region Get non-static method delegate

        public override DynamicMethodProxyDelegate GetMethodDelegate(
            Module targetModule, 
            MethodInfo genericMethodInfo, 
            params Type[] genericParameterTypes)
        {
            #region  Construct cache key

            if (targetModule == null)
            {
                throw new ArgumentNullException("targetModule could not be null!");
            }

            string key = targetModule.FullyQualifiedName + "|" + genericMethodInfo.DeclaringType.ToString() + "|" + genericMethodInfo.ToString();
            if (genericParameterTypes != null)
            {
                for (int i = 0; i < genericParameterTypes.Length; ++i)
                {
                    key += "|" + genericParameterTypes[i].ToString();
                }
            }

            #endregion

            DynamicMethodProxyDelegate dmd;

            lock (cache2)
            {
                if (cache2.ContainsKey(key))
                {
                    dmd = cache2[key];
                }
                else
                {
                    dmd = DoGetMethodDelegate(targetModule, genericMethodInfo, genericParameterTypes);
                    cache2.Add(key, dmd);
                }
            }

            return dmd;
        }

        public override DynamicMethodProxyDelegate GetMethodDelegate(
            MethodInfo genericMethodInfo, 
            params Type[] genericParameterTypes)
        {
            return GetMethodDelegate(typeof(string).Module, genericMethodInfo, genericParameterTypes);
        }

        #endregion
    }
}
