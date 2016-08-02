using System.Xml.Serialization;


namespace System
{
    /// <summary>
    /// Implements a serializer cache that is used by XmlSection
    /// </summary>
    internal static class SerializerCache
    {
        private static GenericCache<string, XmlSerializer> cache = new GenericCache<string, XmlSerializer>();

        /// <summary>
        /// Generates the key for the serializer
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="rootElementName">The name of the root element</param>
        /// <returns></returns>
        public static string GenerateKey(Type type, string rootElementName)
        {
            return type.Namespace + "." + type.Name + ":" + rootElementName;
        }

        /// <summary>
        /// Generates a serializer, caches and returns it
        /// </summary>
        /// <param name="type"></param>
        /// <param name="rootElementName"></param>
        /// <returns></returns>
        public static XmlSerializer GenerateSerializer(Type type, string rootElementName)
        {
            return GenerateSerializer(type, rootElementName, GenerateKey(type, rootElementName));
        }

        /// <summary>
        /// Generates a serializer, caches and returns it
        /// </summary>
        /// <param name="type"></param>
        /// <param name="rootElementName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static XmlSerializer GenerateSerializer(Type type, string rootElementName, string key)
        {
            XmlRootAttribute root = new XmlRootAttribute(rootElementName);
            XmlSerializer serializer = new XmlSerializer(type, root);

            Add(key, serializer);

            return serializer;
        }

        /// <summary>
        /// Adds a serializer to the cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="serializer"></param>
        public static void Add(string key, XmlSerializer serializer)
        {
            cache.Add(key, serializer);
        }

        /// <summary>
        /// Gets a serializer from the cahce based on the key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>An XmlSerializer object corresponding to the key, null if no serializer is present</returns>
        public static XmlSerializer Get(string key)
        {
            XmlSerializer serializer = null;
            if (cache.TryGetValue(key, out serializer))
                return serializer;
            else return null;
        }

        /// <summary>
        /// Clears the serializer cache
        /// </summary>
        public static void Clear()
        {
            cache.Clear();
        }

        /// <summary>
        /// Returns a serializer for the given type and root element name
        /// from the cache. In case a serializer is not found, it creates
        /// caches and returns a new serializer.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="rootElementName"></param>
        /// <returns></returns>
        public static XmlSerializer Load(Type type, string rootElementName)
        {
            string key = SerializerCache.GenerateKey(type, rootElementName);
            XmlSerializer cahcedSerializer = SerializerCache.Get(key);
            if (null == cahcedSerializer)
                return SerializerCache.GenerateSerializer(type, rootElementName, key);
            else
                return cahcedSerializer;
        }
    }
}
