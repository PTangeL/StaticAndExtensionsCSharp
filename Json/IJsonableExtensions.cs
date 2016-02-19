namespace Library.Json
{
    using Newtonsoft.Json;

    /// <summary>
    /// Extensions for classes that are Jsonable
    /// </summary>
    public static class IJsonableExtensions
    {
        /// <summary>
        /// Serialize the object into a Json type.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static string ToSerialize(this IJsonable item) => JsonConvert.SerializeObject(item);

        /// <summary>
        /// Deserialize a Json, convert and load the T item.
        /// </summary>
        /// <typeparam name="T">The item Type you want to load.</typeparam>
        /// <param name="item">The item.</param>
        /// <param name="itemToLoad">The item you want to load.</param>
        /// <param name="json">The json with the item you want to load.</param>
        public static void ToDeserializeAndLoad<T>(this IJsonable item, ref T itemToLoad, string json) 
            where T : IJsonable =>
            itemToLoad = JsonConvert.DeserializeObject<T>(json);

        /// <summary>
        /// Deserialize the object into a T type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="json">The json you want to Deserialize into a T object.</param>
        /// <returns></returns>
        public static T ToDeserialize<T>(this IJsonable item, string json) where T : IJsonable => 
            JsonConvert.DeserializeObject<T>(json);
    }
}