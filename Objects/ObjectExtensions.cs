namespace StaticAndExtensionsCSharp.Objects
{
    using System;

    public static class ObjectExtensions
    {
        /// <summary>
        /// Turns any object to Exception. Very useful!
        /// </summary>
        /// <code>
        /// Object o = new Object();
        /// throw o.ToException();
        /// </code>
        /// <param name="o">The object</param>
        /// <returns></returns>
        public static Exception ToException(this object obj) => new Exception(obj?.ToString());
    }
}