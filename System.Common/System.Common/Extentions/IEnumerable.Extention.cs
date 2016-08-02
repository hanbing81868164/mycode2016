using System.Collections.Generic;


namespace System
{
    public static partial class Extention
    {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action) where T : class,new()
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }



    }
}
