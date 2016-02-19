/// <summary>
/// This namespace will contain all the utilities we can get for any type of . 
/// </summary>
namespace Library.Enumerables
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// The Enums extensions and utilities are here. 
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Get the description attribute of an enumerable. 
        /// </summary>
        /// <param name="self">
        /// The enumerable you want to get the description. 
        /// </param>
        /// <returns>
        /// Returns the description attribute. 
        /// </returns>
        public static string ToDescription(this Enum self) =>
            ToAttributeOfType<DescriptionAttribute>(self)?.Description;

        /// <summary>
        /// Get the Attribute of the type you want. 
        /// </summary>
        /// <typeparam name="T">
        /// The attribute you want to get from the Enumerable. 
        /// </typeparam>
        /// <param name="self">
        /// The enumerable you want to get the attribute. 
        /// </param>
        /// <returns>
        /// Returns the Attribute that you want. 
        /// </returns>
        public static T ToAttributeOfType<T>(this Enum self) where T : Attribute
        {
            // Get the Attribute Type. 
            var type = self?.GetType();

            // Get the member of the type you want. 
            var memInfo = type?.GetMember(self?.ToString());

            // Try to get on that member the Attribute you want. 
            var attributes = memInfo?[0]?.GetCustomAttributes(typeof(T), false);

            // Try to return the attribute if he find anything, if try to return the default value
            // of the enum Normally the default value its the '0', which normally its NONE, if the
            // enum dont have any 0 value he will return null.
            return ((attributes?.Length > 0) ? (T)attributes?[0] : default(T));
        }
    }
}