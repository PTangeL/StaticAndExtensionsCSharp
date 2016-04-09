namespace StaticAndExtensionsCSharp.Exceptions
{
    using System;

    public static class ExceptionExtensions
    {
        public static void Log(this Exception obj, Func<bool> logAction) => logAction.Invoke();
    }
}
