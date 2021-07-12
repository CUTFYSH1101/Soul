using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Main.Util
{
    /// 常見
    public static class Extension
    {
        /// <summary>
        /// Get the array slice between the two indexes.
        /// ... Inclusive for start index, exclusive for end index.
        /// </summary>
        public static T[] Slice<T>(this T[] source, int start, int end)
        {
            // Handles negative ends.
            if (end < 0)
            {
                end = source.Length + end;
            }

            int len = end - start;

            // Return new array.
            T[] res = new T[len];
            for (int i = 0; i < len; i++)
            {
                res[i] = source[i + start];
            }

            return res;
        }

        /// 該項目是否為空
        // ReSharper disable once AssignNullToNotNullAttribute
        public static bool IsEmpty<T>(this T source) =>
            source == null || source.Equals("");

        /// 該項目是否為空
        // ReSharper disable once AssignNullToNotNullAttribute
        public static bool IsEmpty<T>(this T[] source) =>
            source == null || source.Length == 0;

        /// 該項目是否為空
        // ReSharper disable once AssignNullToNotNullAttribute
        public static bool IsEmpty<T>(this IEnumerable<T> source) =>
            source == null || !source.Any();

        /// 該項目是否為空
        // ReSharper disable once AssignNullToNotNullAttribute
        public static bool IsEmpty<T>(this List<T> source) =>
            source == null || !source.Any();

        /// 該項目是否為空
        // ReSharper disable once AssignNullToNotNullAttribute
        public static bool IsEmpty(this KeyCode source) =>
            source == default;

        /// 該項目是否為空
        // ReSharper disable once AssignNullToNotNullAttribute
        public static bool NotEmpty<T>(this T source) =>
            source != null && !source.Equals("");
    }

    /// 轉換
    public static class Convert
    {
        public static float ToFloat(this object source)
        {
            return float.Parse(source.ToString());
        }

        public static int ToInt(this object source)
        {
            return int.Parse(source.ToString());
        }
    }
}