using System;
using System.Collections.Generic;
using System.Text;

namespace StaticAndExtensionsCSharpStandard.Numbers
{
    public static class DoubleExtensions
    {
        /// <summary>
        /// Toes the double.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static double ToDouble(this string value, double defaultvalue = 0) => 
            double.TryParse(value, out var result) ? result : defaultvalue;
    }
}
