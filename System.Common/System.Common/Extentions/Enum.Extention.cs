using System.ComponentModel;



namespace System
{
    public static partial class Extention
    {

        /// <summary>
        /// 获取枚举Description描述特性值 string description = Fruit.Orange.GetDescription();
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumerationValue">枚举值</param>
        /// <returns>枚举值的描述/returns>
        public static string GetDescription<TEnum>(this Enum enumerationValue)
        {
            return Utils.GetEnumAttribute(enumerationValue, typeof(DescriptionAttribute)) as string;
        }

        /// <summary>
        /// 获取枚举DefaultValue描述特性值 string description = Fruit.Orange.GetDefaultValue();
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumerationValue">枚举值</param>
        /// <returns>枚举值的描述/returns>
        public static object GetDefaultValue<EnumTEnum>(this Enum enumerationValue)
        {
            return Utils.GetEnumAttribute(enumerationValue, typeof(DefaultValueAttribute));
        }

        /// <summary>
        /// 获取枚举DefaultValue描述特性值 string description = Fruit.Orange.GetDefaultValue();
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumerationValue">枚举值</param>
        /// <returns>枚举值的描述/returns>
        public static string GetDisplayName<TEnum>(this Enum enumerationValue)
        {
            return Utils.GetEnumAttribute(enumerationValue, typeof(DisplayNameAttribute)) as string;
        }





    }
}
