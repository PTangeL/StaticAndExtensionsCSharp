namespace Library.Numbers
{
    using System.Globalization;

    /// <summary>
    /// Used for every double values.
    /// </summary>
    public static class DoubleExtensions
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
