using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace System
{
/*
 
      调用方法
private static void DoWork(object state)
{
    Window1 win = (Window1) state;
    for (int i = 0; i < 100; i++)
    {
        // do some work
        win.progress1.Dispatch((p, v) => p.Value = v, i);
    }

    win.progress1.Dispatch(p => p.Value = 100);
}

*/
    public static partial class Extention
    {
        public static TResult Dispatch<TResult>(this System.Windows.Threading.DispatcherObject source, Func<TResult> func)
        {
            if (source.Dispatcher.CheckAccess())
                return func();

            return (TResult)source.Dispatcher.Invoke(func);
        }

        public static TResult Dispatch<T, TResult>(this T source, Func<T, TResult> func) where T : System.Windows.Threading.DispatcherObject
        {
            if (source.Dispatcher.CheckAccess())
                return func(source);

            return (TResult)source.Dispatcher.Invoke(func, source);
        }

        public static TResult Dispatch<TSource, T, TResult>(this TSource source, Func<TSource, T, TResult> func, T param1) where TSource : System.Windows.Threading.DispatcherObject
        {
            if (source.Dispatcher.CheckAccess())
                return func(source, param1);

            return (TResult)source.Dispatcher.Invoke(func, source, param1);
        }

        public static TResult Dispatch<TSource, T1, T2, TResult>(this TSource source, Func<TSource, T1, T2, TResult> func, T1 param1, T2 param2) where TSource : System.Windows.Threading.DispatcherObject
        {
            if (source.Dispatcher.CheckAccess())
                return func(source, param1, param2);

            return (TResult)source.Dispatcher.Invoke(func, source, param1, param2);
        }

        public static TResult Dispatch<TSource, T1, T2, T3, TResult>(this TSource source, Func<TSource, T1, T2, T3, TResult> func, T1 param1, T2 param2, T3 param3) where TSource : System.Windows.Threading.DispatcherObject
        {
            if (source.Dispatcher.CheckAccess())
                return func(source, param1, param2, param3);

            return (TResult)source.Dispatcher.Invoke(func, source, param1, param2, param3);
        }

        public static void Dispatch(this System.Windows.Threading.DispatcherObject source, Action func)
        {
            if (source.Dispatcher.CheckAccess())
                func();
            else
                source.Dispatcher.Invoke(func);
        }

        public static void Dispatch<TSource>(this TSource source, Action<TSource> func) where TSource : System.Windows.Threading.DispatcherObject
        {
            if (source.Dispatcher.CheckAccess())
                func(source);
            else
                source.Dispatcher.Invoke(func, source);
        }

        public static void Dispatch<TSource, T1>(this TSource source, Action<TSource, T1> func, T1 param1) where TSource : System.Windows.Threading.DispatcherObject
        {
            if (source.Dispatcher.CheckAccess())
                func(source, param1);
            else
                source.Dispatcher.Invoke(func, source, param1);
        }

        public static void Dispatch<TSource, T1, T2>(this TSource source, Action<TSource, T1, T2> func, T1 param1, T2 param2) where TSource : System.Windows.Threading.DispatcherObject
        {
            if (source.Dispatcher.CheckAccess())
                func(source, param1, param2);
            else
                source.Dispatcher.Invoke(func, source, param1, param2);
        }

        public static void Dispatch<TSource, T1, T2, T3>(this TSource source, Action<TSource, T1, T2, T3> func,
                                                         T1 param1, T2 param2, T3 param3) where TSource : System.Windows.Threading.DispatcherObject
        {
            if (source.Dispatcher.CheckAccess())
                func(source, param1, param2, param3);
            else
                source.Dispatcher.Invoke(func, source, param1, param2, param3);
        }
    }
}
