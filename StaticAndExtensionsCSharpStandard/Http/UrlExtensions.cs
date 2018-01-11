using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;

namespace StaticAndExtensionsCSharpStandard.Http
{
    public static class UrlExtensions
    {
        /// <summary>
        /// Determines whether it is a valid URL.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is valid URL] [the specified text]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidUrl(this string text) => new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?").IsMatch(text);

        /// <summary>
        /// Converts to a HTML-encoded string
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string HtmlEncode(this string data) => HttpUtility.HtmlEncode(data);

        /// <summary>
        /// Converts the HTML-encoded string into a decoded string
        /// </summary>
        public static string HtmlDecode(this string data) => HttpUtility.HtmlDecode(data);

        /// <summary>
        /// Parses a query string into a System.Collections.Specialized.NameValueCollection
        /// using System.Text.Encoding.UTF8 encoding.
        /// </summary>
        public static NameValueCollection ParseQueryString(this string query) => HttpUtility.ParseQueryString(query);

        /// <summary>
        /// Encode an Url string
        /// </summary>
        public static string UrlEncode(this string url) => HttpUtility.UrlEncode(url);

        /// <summary>
        /// Converts a string that has been encoded for transmission in a URL into a
        /// decoded string.
        /// </summary>
        public static string UrlDecode(this string url) => HttpUtility.UrlDecode(url);

        /// <summary>
        /// Encodes the path portion of a URL string for reliable HTTP transmission from
        /// the Web server to a client.
        /// </summary>
        public static string UrlPathEncode(this string url) => HttpUtility.UrlPathEncode(url);
    }
}