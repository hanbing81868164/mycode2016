using System.Collections.Concurrent;

namespace System
{
    /// <summary>
    /// 线程安全的轻量泛型类提供了从一组键到一组值的映射。
    /// </summary>
    /// <typeparam name="TKey">字典中的键的类型</typeparam>
    /// <typeparam name="TValue">字典中的值的类型</typeparam>
    public class GenericCache<TKey, TValue>
    {
        #region Fields
        /// <summary>
        /// 内部的 Dictionary 容器
        /// </summary>
        public ConcurrentDictionary<TKey, TValue> Dictionary = new ConcurrentDictionary<TKey, TValue>();

        /// <summary>
        /// 一个 TimeSpan，用于指定超时时间。 
        /// </summary>
        private readonly TimeSpan lockTimeOut = TimeSpan.FromMilliseconds(100);
        #endregion

        #region Methods
        /// <summary>
        /// 将指定的键和值添加到字典中。
        /// Exceptions：
        ///     ArgumentException - Dictionary 中已存在具有相同键的元素。
        /// </summary>
        /// <param name="key">要添加的元素的键。</param>
        /// <param name="value">添加的元素的值。对于引用类型，该值可以为 空引用</param>
        public void Add(TKey key, TValue value)
        {
            try
            {
                if (!Dictionary.ContainsKey(key))
                    Dictionary.TryAdd(key, value);
            }
            finally { }
        }

        public void Update(TKey key, TValue value)
        {
            try
            {
                if (Dictionary.ContainsKey(key))
                    Dictionary.TryUpdate(key, value, value);
            }
            finally { }
        }

        public void Remove(TKey key)
        {
            try
            {
                TValue value;
                if (Dictionary.ContainsKey(key))
                {
                    Dictionary.TryRemove(key, out value);
                }
            }
            finally { }
        }

        /// <summary>
        /// 获取与指定的键相关联的值。 
        /// Exceptions：
        ///     ArgumentException - Dictionary 中已存在具有相同键的元素。
        /// </summary>
        /// <param name="key">要添加的元素的键。</param>
        /// <param name="value">当此方法返回值时，如果找到该键，便会返回与指定的键相关联的值；
        /// 否则，则会返回 value 参数的类型默认值。该参数未经初始化即被传递</param>
        /// <returns>如果 Dictionary 包含具有指定键的元素，则为 true；否则为 false</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            bool result;
            try
            {
                result = Dictionary.TryGetValue(key, out value);
            }
            finally {  }
            return result;
        }

        /// <summary>
        /// 从 Dictionary 中移除所有的键和值。
        /// </summary>
        public void Clear()
        {
            if (Dictionary.Count > 0)
            {
                try
                {
                    Dictionary.Clear();
                }
                finally {  }
            }
        }

        /// <summary>
        /// 确定 Dictionary 是否包含指定的键。 
        /// </summary>
        /// <param name="key">要在 Dictionary 中定位的键。</param>
        /// <returns>如果 Dictionary 包含具有指定键的元素，则为 true；否则为 false。</returns>
        public bool ContainsKey(TKey key)
        {
            if (Dictionary.Count <= 0) return false;
            bool result;
            try
            {
                result = Dictionary.ContainsKey(key);
            }
            finally { }
            return result;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 获取包含在 Dictionary 中的键/值对的数目。
        /// </summary>
        public int Count
        {
            get { return Dictionary.Count; }
        }
        #endregion
    }
}
