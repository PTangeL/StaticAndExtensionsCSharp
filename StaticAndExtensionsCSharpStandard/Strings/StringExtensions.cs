using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaticAndExtensionsCSharpStandard.Strings
{
    public static class StringExtensions
    {
        /// <summary>
        /// Add quotes to the current string.
        /// </summary>
        /// <param name="item">Current string.</param>
        /// <returns></returns>
        public static string AddQuotes(this string item) => '"' + item + '"';

        /// <summary>
        /// Checks string object's value to array of string values
        /// </summary>
        /// <param name="stringValues">Array of string values to compare</param>
        /// <returns>Return true if any string value matches</returns>
        public static bool In(this string value, params string[] stringValues) => 
            stringValues.Any(otherValue => string.CompareOrdinal(value, otherValue) == 0);

        /// <summary>
        /// Remove from the end of the string how many chars you want
        /// </summary>
        /// <param name="instr">The current string.</param>
        /// <param name="number">How many you want to remove from the end.</param>
        /// <returns></returns>
        public static string RemoveLast(this string instr, int number = 1) => instr.Substring(0, instr.Length - number);

        /// <summary>
        /// Remove from the start of the string how many chars you want
        /// </summary>
        /// <param name="instr">The current string.</param>
        /// <param name="number">How many you want to remove from the start.</param>
        /// <returns></returns>
        public static string RemoveFirst(this string instr, int number = 1) => instr.Substring(number);

        /// <summary>
        /// Reverse a String
        /// </summary>
        /// <param name="input">The string to Reverse</param>
        /// <returns>The reversed String</returns>
        public static string Reverse(this string input)
        {
            var array = input.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }

        /// <summary>
        /// Truncates the string to a specified length and replace the truncated to a ...
        /// </summary>
        /// <param name="maxLength">total length of characters to maintain before the truncate happens</param>
        /// <returns>truncated string</returns>
        public static string Truncate(this string text, int maxLength)
        {
            // replaces the truncated string to a ...
            const string suffix = "...";
            var truncatedString = text;

            if (maxLength <= 0) return truncatedString;
            var strLength = maxLength - suffix.Length;

            if (strLength <= 0) return truncatedString;

            if (text == null || text.Length <= maxLength) return truncatedString;

            truncatedString = text.Substring(0, strLength);
            truncatedString = truncatedString.TrimEnd();
            truncatedString += suffix;
            return truncatedString;
        }
    }
}
