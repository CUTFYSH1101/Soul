using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Main.Entity;
using Main.Entity.Controller;
using UnityEngine;
using Physics2D = UnityEngine.Physics2D;

namespace Main.Util
{
    public static class Array
    {
        public static T[] Concat<T>([CanBeNull] this T[] x, [CanBeNull] T[] y) where T : AbstractCreatureAI
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

        public static string ArrayToString<T>(this T[] source, bool hasBrackets = true) =>
            hasBrackets
                ? $"[{string.Join(",", source.Select(s => s.ToString()))}]"
                : string.Join(",", source.Select(s => s.ToString()));

        public static string ArrayToString<T>(this T[] source, string separator, bool hasBrackets = true) =>
            hasBrackets
                ? $"[{string.Join(separator, source.Select(s => s.ToString()))}]"
                : string.Join(separator, source.Select(s => s.ToString()));

        public static string ArrayToString<T>(this T[] source, char separator, bool hasBrackets = true) =>
            hasBrackets
                ? $"[{string.Join(separator.ToString(), source.Select(s => s.ToString()))}]"
                : string.Join(separator.ToString(), source.Select(s => s.ToString()));

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
        
        public static object FirstOrNull<T>(this IEnumerable<T> source)
        {
            if (source.IsEmpty())
                return null;
            return source.FirstOrDefault();
        }

        public static T FirstOrNull<T>(this IEnumerable<T> source, Func<T, bool> filter)
        {
            if (source.IsEmpty() || !source.Any(filter))
                return default;
            return source.FirstOrDefault(filter);
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