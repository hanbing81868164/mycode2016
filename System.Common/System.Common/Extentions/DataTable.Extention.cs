using System.Collections.Generic;
using System.Data;
using System.Collections.ObjectModel;
using System.Reflection;


namespace System
{
    public static partial class Extention
    {

        //public static List<T> ToList<T>(this DataTable dt) where T : new()
        //{
        //    List<T> lstT = new List<T>();
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        lstT.Add(dr.ToObject<T>());
        //    }
        //    return lstT;
        //}

        //public static T ToObject<T>(this DataRow dr) where T : new()
        //{
        //    dynamic dynTemp = new T();
        //    return dynTemp.GetFrom(dr);
        //}



        /// <summary>
        /// dataTable 转 ObservableCollection
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="data">原DataTable</param>
        /// <returns></returns>
        public static ObservableCollection<T> ConvertObservableCollectionEntity<T>(this DataTable data) where T : new()
        {
            ObservableCollection<T> list = new ObservableCollection<T>();

            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (DataRow o in data.Rows)
            {
                T t = new T();
                foreach (PropertyInfo p in properties)
                {
                    if (data.Columns.Contains(p.Name))
                    {
                        if (o[p.Name] == null || o[p.Name] == System.DBNull.Value)
                        {
                            continue;
                        }
                        Type type = p.PropertyType;
                        if (type != typeof(string) && o[p.Name].ToString() == "")
                        {
                            continue;
                        }
                        object obj = Convert.ChangeType(o[p.Name], type);
                        p.FastSetValue(t, obj);
                    }

                }
                list.Add(t);
            }
            return list;
        }

        /// <summary>
        /// dataTable 转 IList
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="data">原DataTable</param>
        /// <returns></returns>
        public static IList<T> ConvertList<T>(this DataTable data) where T : new()
        {
            IList<T> list = new List<T>();

            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (DataRow o in data.Rows)
            {
                T t = new T();
                foreach (PropertyInfo p in properties)
                {
                    if (data.Columns.Contains(p.Name))
                    {
                        if (o[p.Name] == null || o[p.Name] == System.DBNull.Value)
                        {
                            continue;
                        }
                        Type type = p.PropertyType;
                        if (type != typeof(string) && o[p.Name].ToString() == "")
                        {
                            continue;
                        }
                        object obj = Convert.ChangeType(o[p.Name], type);
                        p.FastSetValue(t, obj);
                    }

                }
                list.Add(t);
            }
            return list;
        }

    }
}
