using System;
using System.Collections.Generic;
using System.Text;

namespace StaticAndExtensionsCSharpStandard.Collections
{
    public static class CSVExtensions
    {
        public static string ToCSV<T>(this IEnumerable<T> instance, char separator)
        {
            if (instance == null) return null;

            var csv = new StringBuilder();
            instance.Each(value => csv.AppendFormat("{0}{1}", value, separator));
            return csv.ToString(0, csv.Length - 1);
        }

        public static string ToCSV<T>(this IEnumerable<T> instance)
        {
            if (instance == null) return null;

            var csv = new StringBuilder();
            instance.Each(v => csv.AppendFormat("{0},", v));
            return csv.ToString(0, csv.Length - 1);
        }
    }
}
