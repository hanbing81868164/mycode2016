using System.Reflection;

namespace System.Reflect
{
    /// <summary>
    /// 属性访问缓存（实现Create）
    /// </summary>
    public class PropertyAccessorCache : FastReflectionCache<PropertyInfo, IPropertyAccessor>
    {
        protected override IPropertyAccessor Create(PropertyInfo key)
        {
            return new PropertyAccessor(key);
        }
    }
}
