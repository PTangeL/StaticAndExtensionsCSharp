using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StaticAndExtensionsCSharpStandard.Enumerables
{
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
        /// Get the description attribute of an enumerable. 
        /// </summary>
        /// <param name="self">
        /// The enumerable you want to get the description. 
        /// </param>
        /// <returns>
        /// Returns the description attribute. 
        /// </returns>
        public static string ToName(this Enum self) =>
            Enum.GetName(self.GetType(), self);

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

        /// <summary>
        /// Parses a string into an Enum
        /// </summary>
        /// <typeparam name="T">The type of the Enum</typeparam>
        /// <param name="value">String value to parse</param>
        /// <param name="ignorecase">Ignore the case of the string being parsed</param>
        /// <returns>The Enum corresponding to the stringExtensions</returns>
        public static T ToEnum<T>(this string value, bool ignorecase = false)
        {
            if (string.IsNullOrEmpty(value))
                return default(T);

            value = value.Trim();

            var t = typeof(T);
            if (!t.IsEnum)
                throw new ArgumentException("Type provided must be an Enum.", "T");

            return (T)Enum.Parse(t, value, ignorecase);
        }

        public static TEnum ToEnum<TEnum>(this int item) => (TEnum)Enum.ToObject(typeof(TEnum), item);

        /// <summary>
        /// Checks string object's value to array of string values
        /// </summary>
        /// <param name="stringValues">Array of string values to compare</param>
        /// <returns>Return true if any string value matches</returns>
        public static bool In(this string value, params string[] stringValues) => 
            stringValues.Any(otherValue => string.CompareOrdinal(value, otherValue) == 0);
    }
}
