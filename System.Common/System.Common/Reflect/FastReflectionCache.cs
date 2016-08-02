using System.Collections.Generic;

namespace System.Reflect
{
    /// <summary>
    /// 快速反射缓存类（实现Get方法）
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public abstract class FastReflectionCache<TKey, TValue> : IFastReflectionCache<TKey, TValue>
    {
        private Dictionary<TKey, TValue> m_cache = new Dictionary<TKey, TValue>();

        /// <summary>
        /// <code>
        /// return FastReflectionCaches.MethodInvokerCache.Get(methodInfo).Invoke(instance, parameters);
        /// </code>
        /// </summary>
        public TValue Get(TKey key)
        {
            TValue value = default(TValue);
            if (this.m_cache.TryGetValue(key, out value))
            {
                return value;
            }
            //lock (key)
            //{
            if (!this.m_cache.TryGetValue(key, out value))
            {
                value = this.Create(key);
                this.m_cache[key] = value;
            }
            //}
            return value;
        }
        protected abstract TValue Create(TKey key);
    }
}
