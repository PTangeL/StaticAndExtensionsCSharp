namespace StaticAndExtensionsCSharp.Numbers
{
    public static class IntExtensions
    {
        public static TEnum ToEnum<TEnum>(this int item) => (TEnum)System.Enum.ToObject(typeof(TEnum), item);
    }
}