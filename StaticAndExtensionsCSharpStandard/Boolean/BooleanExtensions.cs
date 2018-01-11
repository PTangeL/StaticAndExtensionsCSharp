using System;
using System.Collections.Generic;
using System.Text;

namespace StaticAndExtensionsCSharpStandard.Boolean
{
    public static class BooleanExtensions
    {
        /// <summary>
        /// Converts a string value to bool value, supports "T" and "F" conversions.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns>A bool based on the string value</returns>
        public static bool? ToBoolean(this string value)
        {
            if (string.Compare("T", value, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return true;
            }
            if (string.Compare("F", value, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return false;
            }

            if (bool.TryParse(value, out var result))
            {
                return result;
            }

            return null;
        }
    }
}
