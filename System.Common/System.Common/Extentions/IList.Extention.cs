using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using System.Collections.ObjectModel;


namespace System
{
    public static partial class Extention
    {

        /// <summary>
        /// ObservableCollection中的类型转换，可以动态把一个类型数据填充到另一个属性一样的类型
        /// </summary>
        /// <typeparam name="A">源类型</typeparam>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="data">源数据</param>
        /// <returns>目标数据</returns>
        public static ObservableCollection<T> ConvertObservableCollectionEntity<A, T>(this IList<A> data) where T : new()
        {
            ObservableCollection<T> list = new ObservableCollection<T>();

            PropertyInfo[] properties = typeof(T).GetProperties();
            PropertyInfo[] dataprop = typeof(A).GetProperties();

            foreach (A o in data)
            {
                T t = new T();
                foreach (PropertyInfo p in properties)
                {
                    var a = dataprop.FirstOrDefault(p1 => p1.Name == p.Name && p1.PropertyType.IsAssignableFrom(p.PropertyType));
                    if (a != null)
                        p.FastSetValue(t, a.FastGetValue(o));
                }
                list.Add(t);
            }
            return list;
        }





    }
}
