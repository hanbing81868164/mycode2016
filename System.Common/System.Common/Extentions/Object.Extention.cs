using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Reflection;
using AutoMapper;


namespace System
{
    public static partial class Extention
    {

        public static string SerializeObjectToJson(this object o)
        {
            return JsonConvert.SerializeObject(o, Formatting.Indented);
        }

        public static Dictionary<string, object> ToDictionary(this object o)
        {
            return o.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(o, new object[0]));
        }

        /// <summary>
        /// 类型转换，可以动态把一个类型数据填充到另一个属性一样的类型
        /// </summary>
        /// <typeparam name="A">源类型</typeparam>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="data">源数据</param>
        /// <returns>目标数据</returns>
        public static T ConvertEntity<A, T>(this A data) where T : new()
        {
            T t = new T();
            PropertyInfo[] properties = typeof(T).GetProperties();
            PropertyInfo[] dataprop = typeof(A).GetProperties();
            foreach (PropertyInfo p in properties)
            {
                p.FastSetValue(t, dataprop.FirstOrDefault(p1 => p1.Name == p.Name && p1.PropertyType.IsAssignableFrom(p.PropertyType)).FastGetValue(data));
            }
            return t;
        }

        /// <summary>
        /// 类型转换，可以动态把一个类型数据填充到另一个属性一样的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static T DynamicMap<T>(this object o) where T : new()
        {
            return MapperHelper.DynamicMap<T>(o);
        }


        /// <summary>
        /// 获取枚举Description描述特性值 string description = Fruit.Orange.GetDescription();
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumerationValue">枚举值</param>
        /// <returns>枚举值的描述/returns>
        public static string GetDescription(this object o, string fieldName)
        {
            return Utils.GetPropertyAttribute(o, fieldName, typeof(DescriptionAttribute)) as string;
        }

        /// <summary>
        /// 获取枚举DefaultValue描述特性值 string description = Fruit.Orange.GetDefaultValue();
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumerationValue">枚举值</param>
        /// <returns>枚举值的描述/returns>
        public static object GetDefaultValue(this object o, string fieldName)
        {
            return Utils.GetPropertyAttribute(o, fieldName, typeof(DefaultValueAttribute));
        }

        /// <summary>
        /// 获取枚举DefaultValue描述特性值 string description = Fruit.Orange.GetDefaultValue();
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumerationValue">枚举值</param>
        /// <returns>枚举值的描述/returns>
        public static string GetDisplayName(this object o, string fieldName)
        {
            return Utils.GetPropertyAttribute(o, fieldName, typeof(DisplayNameAttribute)) as string;
        }

        public static string Getppp(this object o, string fieldName = null)
        {
            AttributeCollection attributes = (fieldName.IsNullOrEmpty() ? TypeDescriptor.GetAttributes(o) : TypeDescriptor.GetProperties(o)[fieldName].Attributes);
            DescriptionAttribute myAttribute = (DescriptionAttribute)attributes[typeof(DescriptionAttribute)];
            return myAttribute.Description;
        }

    }
}
