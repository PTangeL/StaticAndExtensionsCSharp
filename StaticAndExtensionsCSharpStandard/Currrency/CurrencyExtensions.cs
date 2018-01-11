using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace StaticAndExtensionsCSharpStandard.Currrency
{
    public static class CurrencyExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        /// double test = 154.20;
        /// string testString = test.ToCurrency("en-US"); // $154.20
        /// </example>
        /// <param name="value"></param>
        /// <param name="cultureName"></param>
        /// <returns></returns>
        public static string ToCurrency(this double value, string cultureName)
        {
            CultureInfo currentCulture = new CultureInfo(cultureName);
            return (string.Format(currentCulture, "{0:C}", value));
        }

        /// <summary>
        /// Current Culture of the where the code is.
        /// </summary>
        /// <example>
        /// double test = 154.20;
        /// string testString = test.ToCurrency(); // $154.20
        /// </example>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToCurrency(this double value)
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            return (string.Format(currentCulture, "{0:C}", value));
        }
    }
}
