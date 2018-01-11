using System;
using System.Collections.Generic;
using System.Text;

namespace StaticAndExtensionsCSharpStandard.Numbers
{
    public static class IntegerExtensions
    {
        /// <summary>
        /// Toes the integer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static int ToInteger(this string value, int defaultvalue = 0) => 
            (int)value.ToDouble(defaultvalue);
    }
}
