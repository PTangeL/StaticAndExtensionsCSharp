/// <summary>
/// This namespace will contain all the utilities we can get for any type of lists.
/// </summary>
namespace StaticAndExtensionsCSharp.Lists
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// This class contains every Utilities you could want for your lists / enumerables / Arrays.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Sets all values.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the list that will be modified.</typeparam>
        /// <param name="array">The one-dimensional, zero-based list.</param>
        /// <param name="value">The value.</param>
        /// <returns>A reference to the changed list.</returns>
        public static IEnumerable<T> SetAllValues<T>(this IEnumerable<T> array, T value) =>
            array.ForEach(item => item = value);

        /// <summary>
        /// ForEach for IEnumerables.
        /// </summary>
        /// <typeparam name="T">The Type of the object you have.</typeparam>
        /// <param name="list">The IEnumerable list you have.</param>
        /// <param name="action">The action you want to do.</param>
        /// <returns></returns>
        public static int ForEach<T>(this IEnumerable<T> list, Action<int, T> action)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (action == null) throw new ArgumentNullException("action");

            var index = 0;

            foreach (var elem in list)
                action(index++, elem);

            return index;
        }

        /// <summary>
        ///   Returns all combinations of a chosen amount of selected elements in the sequence.
        /// </summary>
        /// <example>
        /// </code>
        /// int[] numbers = new[] { 0, 1 };
        /// var result = numbers.Combinations(2, true);
        /// // result == {{0, 0}, {0, 1}, {1, 0}, {1, 1}}
        /// </code>
        /// </example>
        /// <typeparam name = "T">The type of the elements of the input sequence.</typeparam>
        /// <param name = "source">The source for this extension method.</param>
        /// <param name = "select">The amount of elements to select for every combination.</param>
        /// <param name = "repetition">True when repetition of elements is allowed.</param>
        /// <returns>All combinations of a chosen amount of selected elements in the sequence.</returns>
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> source, int select, bool repetition = false)
        {
            Contract.Requires(source != null);
            Contract.Requires(select >= 0);

            return select == 0
                ? new[] { new T[0] }
                : source.SelectMany((element, index) =>
                   source
                       .Skip(repetition ? index : index + 1)
                       .Combinations(select - 1, repetition)
                       .Select(c => new[] { element }.Concat(c)));
        }

        /// <summary>
        /// Shuffles an IEnumerable list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list)
        {
            var r = new Random((int)DateTime.Now.Ticks);
            var shuffledList = list.Select(x => new { Number = r.Next(), Item = x }).OrderBy(x => x.Number).Select(x => x.Item);
            return shuffledList.ToList();
        }

        /// <summary>
        /// Adds the specified item / items to an enumerable.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the item you want to add.
        /// </typeparam>
        /// <param name="source">
        /// The enumerable list you have.
        /// </param>
        /// <param name="target">
        /// The list you want to add the items.
        /// </param>
        /// <param name="item">
        /// The item/items you want to add to the list.
        /// </param>
        public static void Add<T>(this IEnumerable<T> source, ref IEnumerable<T> target, params T[] item)
        {
            source = source ?? Enumerable.Empty<T>();
            target = target ?? Enumerable.Empty<T>();

            if (item == null)
            {
                target = source;
                return;
            }

            target = source.Concat(item);
        }

        /// <summary>
        /// Validate if the item /items was already added to the list, if not add it.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the item you want to add.
        /// </typeparam>
        /// <param name="source">
        /// The enumerable list you have.
        /// </param>
        /// <param name="target">
        /// The list you want to add the items.
        /// </param>
        /// <param name="item">
        /// The item/items you want to add to the list.
        /// </param>
        public static void AddUnique<T>(this IEnumerable<T> source, ref IEnumerable<T> target, params T[] item)
        {
            foreach (T t in item)
            {
                // If already contains will not add.
                if (target.Contains(t))
                    continue;

                source.Add(ref target, t);
            }
        }

        /// <summary>
        /// Check if this IEnumerable is null, if it is just create an empty IEnumerable.
        /// </summary>
        /// <example>
        /// MyList.EmptyIfNull().Where(....)
        /// </exampley>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> item) => item ?? Enumerable.Empty<T>();

        /// <summary>
        /// <code>
        /// string[] names = new string[] { "C#", "Java" };
        /// names.ForEach(i => Console.WriteLine(i));
        ///
        /// IEnumerable<int> namesLen = names.ForEach(i => i.Length);
        /// namesLen.ForEach(i => Console.WriteLine(i));
        /// </code>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> array, Action<T> act)
        {
            foreach (var i in array)
                act(i);
            return array;
        }

        /// <summary>
        /// <code>
        /// string[] names = new string[] { "C#", "Java" };
        /// names.ForEach(i => Console.WriteLine(i));
        ///
        /// IEnumerable<int> namesLen = names.ForEach(i => i.Length);
        /// namesLen.ForEach(i => Console.WriteLine(i));
        /// </code>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable arr, Action<T> act) => arr.Cast<T>().ForEach(act);

        /// <summary>
        /// <code>
        /// string[] names = new string[] { "C#", "Java" };
        /// names.ForEach(i => Console.WriteLine(i));
        ///
        /// IEnumerable<int> namesLen = names.ForEach(i => i.Length);
        /// namesLen.ForEach(i => Console.WriteLine(i));
        /// </code>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="RT"></typeparam>
        /// <param name="array"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<RT> ForEach<T, RT>(this IEnumerable<T> array, Func<T, RT> func)
        {
            var list = new List<RT>();
            foreach (var i in array)
            {
                var obj = func(i);
                if (obj != null)
                    list.Add(obj);
            }
            return list;
        }

#if Office

        /// <summary>
        /// Extension method to write list data to excel.
        /// The PROPERTY HAVE TO HAVE DisplatNameAttribute.
        /// </summary>
        /// <typeparam name="T">Ganeric list</typeparam>
        /// <param name="list"></param>
        /// <param name="PathToSave">Path to save file.</param>
        public static void ToExcel<T>(this IEnumerable<T> list, string PathToSave)
        {
        #region Declarations

            if (string.IsNullOrEmpty(PathToSave))
            {
                throw new Exception("Invalid file path.");
            }
            else if (PathToSave.ToLower().Contains("") == false)
            {
                throw new Exception("Invalid file path.");
            }

            if (list == null)
            {
                throw new Exception("No data to export.");
            }

            Microsoft.Office.Interop.Excel.Application excelApp = null;
            Microsoft.Office.Interop.Excel.Workbooks books = null;
            Microsoft.Office.Interop.Excel._Workbook book = null;
            Microsoft.Office.Interop.Excel.Sheets sheets = null;
            Microsoft.Office.Interop.Excel._Worksheet sheet = null;
            Microsoft.Office.Interop.Excel.Range range = null;
            Microsoft.Office.Interop.Excel.Font font = null;
            // Optional argument variable
            object optionalValue = Missing.Value;

            string strHeaderStart = "A2";
            string strDataStart = "A3";

        #endregion Declarations

        #region Processing

            try
            {
        #region Init Excel app.

                excelApp = new Microsoft.Office.Interop.Excel.Application();
                books = excelApp.Workbooks;
                book = books.Add(optionalValue);
                sheets = book.Worksheets;
                sheet = sheets.get_Item(1);

        #endregion Init Excel app.

        #region Creating Header

                Dictionary<string, string> objHeaders = new Dictionary<string, string>();

                PropertyInfo[] headerInfo = typeof(T).GetProperties();

                foreach (var property in headerInfo)
                {
                    var attribute = property.GetCustomAttributes(typeof(DisplayNameAttribute), false)
                                            .Cast<DisplayNameAttribute>().FirstOrDefault();
                    objHeaders.Add(property.Name, attribute == null ?
                                        property.Name : attribute.DisplayName);
                }

                range = sheet.get_Range(strHeaderStart, optionalValue);
                range = range.get_Resize(1, objHeaders.Count);

                range.set_Value(optionalValue, objHeaders.Values.ToArray());
                range.BorderAround(Type.Missing, Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);

                font = range.Font;
                font.Bold = true;
                range.Interior.Color = Color.LightGray.ToArgb();

        #endregion Creating Header

        #region Writing data to cell

                int count = list.Count();
                object[,] objData = new object[count, objHeaders.Count];

                for (int j = 0; j < count; j++)
                {
                    var item = list.ElementAt(j);
                    int i = 0;
                    foreach (KeyValuePair<string, string> entry in objHeaders)
                    {
                        var y = typeof(T).InvokeMember(entry.Key.ToString(), BindingFlags.GetProperty, null, item, null);
                        objData[j, i++] = (y == null) ? string.Empty : y.ToString();
                    }
                }

                range = sheet.get_Range(strDataStart, optionalValue);
                range = range.get_Resize(count, objHeaders.Count);

                range.set_Value(optionalValue, objData);
                range.BorderAround(Type.Missing, Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin, Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);

                range = sheet.get_Range(strHeaderStart, optionalValue);
                range = range.get_Resize(count + 1, objHeaders.Count);
                range.Columns.AutoFit();

        #endregion Writing data to cell

        #region Saving data and Opening Excel file.

                if (string.IsNullOrEmpty(PathToSave) == false)
                    book.SaveAs(PathToSave);

                excelApp.Visible = true;

        #endregion Saving data and Opening Excel file.

        #region Release objects

                try
                {
                    if (sheet != null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(sheet);
                    sheet = null;

                    if (sheets != null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(sheets);
                    sheets = null;

                    if (book != null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(book);
                    book = null;

                    if (books != null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(books);
                    books = null;

                    if (excelApp != null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                    excelApp = null;
                }
                catch
                {
                    sheet = null;
                    sheets = null;
                    book = null;
                    books = null;
                    excelApp = null;
                }
                finally
                {
                    GC.Collect();
                }

        #endregion Release objects
            }
            catch (Exception ex)
            {
                throw ex;
            }

        #endregion Processing
        }
#endif

        public static void Each<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable)
            {
                action(item);
            }
        }

        public static string ToCSV<T>(this IEnumerable<T> instance, char separator)
        {
            StringBuilder csv;
            if (instance != null)
            {
                csv = new StringBuilder();
                instance.Each(value => csv.AppendFormat("{0}{1}", value, separator));
                return csv.ToString(0, csv.Length - 1);
            }
            return null;
        }

        public static string ToCSV<T>(this IEnumerable<T> instance)
        {
            StringBuilder csv;
            if (instance != null)
            {
                csv = new StringBuilder();
                instance.Each(v => csv.AppendFormat("{0},", v));
                return csv.ToString(0, csv.Length - 1);
            }
            return null;
        }
    }
}