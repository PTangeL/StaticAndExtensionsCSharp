using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace StaticAndExtensionsCSharpStandard.Collections
{
    public static class CollectionExtensions
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
            foreach (var t in item)
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
        public static IEnumerable<RT> ForEach<T, RT>(this IEnumerable<T> array, Func<T, RT> func) =>
            array.Select(func).Where(obj => obj != null).ToList();

        public static void Each<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }
        
        /// <summary>
        /// Checks string object's value to array of string values
        /// </summary>
        /// <param name="stringValues">Array of string values to compare</param>
        /// <returns>Return true if any string value matches</returns>
        public static bool In(this string value, params string[] stringValues)
        {
            foreach (string otherValue in stringValues)
                if (string.Compare(value, otherValue) == 0)
                    return true;

            return false;
        }
    }
}
