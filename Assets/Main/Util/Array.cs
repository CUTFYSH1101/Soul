using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Arr = System.Array;

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

        public static T[] Concat<T, TY>([CanBeNull] this T[] x, [CanBeNull] TY[] y)
        {
            var z = new T[x.Length + y.Length];
            x.CopyTo(z, 0);
            y.CopyTo(z, x.Length);
            return z;
        }

        public static string EnumArrayToString<T>(this T[] source, bool hasBrackets = true)
            where T : struct, IConvertible
        {
            if (source.IsEmpty()) return "[]";
            return hasBrackets
                ? $"[{string.Join(",", source.Filter(arg => arg != default))}]"
                : string.Join(",", source.Filter(arg => arg != default));
        }

        public static string ArrayToString<T>(this T[] source, bool hasBrackets = true)
            where T : class =>
            hasBrackets
                ? $"[{string.Join(",", source.Select(s => s.ToString()).Filter(arg => arg != ""))}]"
                : string.Join(",", source.Select(s => s.ToString()).Filter(arg => arg != ""));

        public static string ArrayToString<T>(this T[] source, string separator, bool hasBrackets = true)
            where T : class =>
            hasBrackets
                ? $"[{string.Join(separator, source.Select(s => s.ToString()).Filter(arg => arg != ""))}]"
                : string.Join(separator, source.Select(s => s.ToString()).Filter(arg => arg != ""));

        public static string ArrayToString<T>(this T[] source, char separator, bool hasBrackets = true)
            where T : class =>
            hasBrackets
                ? $"[{string.Join(separator.ToString(), source.Select(s => s.ToString()).Filter(arg => arg != ""))}]"
                : string.Join(separator.ToString(), source.Select(s => s.ToString()).Filter(arg => arg != ""));

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

        public static T FirstOrNull<T>(this T[] source) where T : class =>
            source.IsEmpty() ? null : source.FirstOrDefault();

        public static T FirstOrNull<T>(this List<T> source) where T : class =>
            source.IsEmpty() ? null : source.FirstOrDefault();

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
            var queue = new Queue<T>();
            for (var i = 0; i < @in.Length; i++)
                if (filter(@in[i]))
                    queue.Enqueue(@in[i]);
            return queue.ToArray();
        }

        public static T[] Filter<T>(this List<T> @in, Func<T, bool> filter)
        {
            if (@in == null || filter == null) return null;
            var queue = new Queue<T>();
            for (var i = 0; i < @in.Count; i++)
                if (filter(@in[i]))
                    queue.Enqueue(@in[i]);
            return queue.ToArray();
        }

        public static T[] Filter<T>(this IEnumerable<T> @in, Func<T, bool> filter)
        {
            if (@in == null || filter == null) return null;
            var queue = new Queue<T>();
            foreach (var t in @in)
                if (filter(t))
                    queue.Enqueue(t);
            return queue.ToArray();
        }
        public static TY[] Get<T, TY>(this T[] @in, Func<T, TY> filter)
        {
            if (@in == null || filter == null) return null;
            var queue = new Queue<TY>();
            for (var i = 0; i < @in.Length; i++)
                if (filter(@in[i]) != null)
                    queue.Enqueue(filter(@in[i]));
            return queue.ToArray();
        }

        public static void Foreach<T>(this T[] @in, Action<T> action)
        {
            for (var i = 0; i < @in.Length; i++)
                action?.Invoke(@in[i]);
        }

        public static T QueryFirst<T>(this T[] @in, Func<T, bool> filter) where T : class
        {
            if (@in == null || filter == null) return null;
            return @in.Any(filter) ? @in.Filter(filter).First() : null;
        }
        
        /// 注意不會更改原本數值。
        /// 範例（√）：string[] arr; arr = arr.Remove(2);
        /// 不是（X）：string[] arr; arr.Remove(2);
        public static T[] Remove<T>(this T[] array, int index)
        {
            if (!array.Any() || index >= array.Length) return array;
            var _ = new T[array.Length - 1];
            Arr.Copy(array, 0, _, 0, index);
            Arr.Copy(array, index + 1, _, index, array.Length - index - 1);

            return _;
        }

        public static T[] Remove<T>(this T[] array, T target)
        {
            if (target is string and (null or "")) return array;
            // if (target is List<T> or IEnumerable<T>) return array; // 不要在陣列中加入奇怪的東西
            if (target == null || !Arr.Exists(array, obj => Equals(target, obj))) return array;

            return Remove(array, Arr.IndexOf(array, target));
        }

        public static T[] RemoveFirst<T>(this T[] array)
        {
            if (!array.Any()) return array;

            var _ = new T[array.Length - 1];
            Arr.Copy(array, 1, _, 0, array.Length - 1);
            return _;
        }

        public static T[] Add<T>(this T[] array, T append)
        {
            if (append is string and (null or "")) return array;
            if (append == null) return array;
            array ??= Arr.Empty<T>();

            var _ = new T[array.Length + 1];
            Arr.Copy(array, _, array.Length);
            _[array.Length] = append;
            return _;
        }

        public static bool Equals<T>(T obj1, T obj2) =>
            EqualityComparer<T>.Default.Equals(obj1, obj2);
    }
}