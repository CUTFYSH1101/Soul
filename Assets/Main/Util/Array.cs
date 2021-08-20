using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Main.Util
{
    public static class Array
    {
        public static T[] Concat<T>([CanBeNull] this T[] x, [CanBeNull] T[] y) where T : class
        {
            var z = new T[x.Length + y.Length];
            x.CopyTo(z, 0);
            y.CopyTo(z, x.Length);
            return z;
        }

        public static T[] Concat<T, Y>([CanBeNull] this T[] x, [CanBeNull] Y[] y)
        {
            var z = new T[x.Length + y.Length];
            x.CopyTo(z, 0);
            y.CopyTo(z, x.Length);
            return z;
        }

        public static string ArrayToString<T>(this T[] source, bool hasBrackets = true) where T : class =>
            hasBrackets
                ? $"[{string.Join(",", source.Select(s => s.ToString()).Where(arg => arg != ""))}]"
                : string.Join(",", source.Select(s => s.ToString()).Where(arg => arg != ""));

        public static string ArrayToString<T>(this T[] source, string separator, bool hasBrackets = true)
            where T : class =>
            hasBrackets
                ? $"[{string.Join(separator, source.Select(s => s.ToString()).Where(arg => arg != ""))}]"
                : string.Join(separator, source.Select(s => s.ToString()).Where(arg => arg != ""));

        public static string ArrayToString<T>(this T[] source, char separator, bool hasBrackets = true)
            where T : class =>
            hasBrackets
                ? $"[{string.Join(separator.ToString(), source.Select(s => s.ToString()).Where(arg => arg != ""))}]"
                : string.Join(separator.ToString(), source.Select(s => s.ToString()).Where(arg => arg != ""));

        /// 將2維陣列轉為1維
        public static T[] ToSingleArray<T>(this T[][] source)
        {
            if (source.Any())
            {
                Debug.LogError("陣列為空!!");
                return null;
            }

            var newArray = new List<T>();
            foreach (T[] array in source)
            {
                foreach (T element in array)
                {
                    newArray.Add(element);
                }
            }

            return newArray.ToArray();
        }

        public static T FirstOrNull<T>(this IEnumerable<T> source) where T : class
        {
            var enumerable = source as T[] ?? source.ToArray();
            return enumerable.IsEmpty() ? null : enumerable.FirstOrDefault();
        }

        public static T FirstOrNull<T>(this IEnumerable<T> source, Func<T, bool> filter) where T : class
        {
            var enumerable = source as T[] ?? source.ToArray();
            if (enumerable.IsEmpty() || !enumerable.Any(filter))
                return null;
            return enumerable.FirstOrDefault(filter);
        }

        public static T Filter<T>(this T @in, Func<T, bool> filter) where T : class
        {
            if (@in == null || filter == null) return null;
            return filter(@in) ? @in : null;
        }

        public static T[] Filter<T>(this T[] @in, Func<T, bool> filter)
        {
            if (@in == null || filter == null) return null;
            return @in.Where(filter).ToArray();
        }

        public static T QueryFirst<T>(this T[] @in, Func<T, bool> filter) where T : class
        {
            if (@in == null || filter == null) return null;
            return @in.Any(filter) ? @in.Where(filter).First() : null;
        }
    }
}