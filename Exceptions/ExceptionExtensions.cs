namespace Library.Exceptions
{
    using System;

    public static class ExceptionExtensions
    {
        public static void Log(this Exception obj, Func<bool> logAction) => logAction.Invoke();

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
