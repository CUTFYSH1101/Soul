using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Debug = UnityEngine.Debug;
using System.Reflection;
using static System.Reflection.BindingFlags;

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

        /// <summary>
        /// 該項目是否為空
        /// </summary>
        public static bool IsEmpty<T>(this T source)
        {
            return (source == null) || (source.Equals(""));
        }

        public static bool IsEmpty<T>(this T[] source)
        {
            return (source == null) || (source.Equals("") || source.Length == 0);
        }

        public static bool IsEmpty(this KeyCode source)
        {
            return (source == null) || (source == default);
        }

        public static bool NotEmpty<T>(this T source)
        {
            return source != null && !source.Equals("");
        }
    }
    /// 獲取腳本成員類相關方法、ToString方法
    public static class Log
    {
        public static void LogLine(this object message)
        {
            Debug.Log(message.ToString());
            Console.WriteLine(message.ToString());
        }

        /// 簡易的獲取所有變數集合，包含Public | NonPublic | instance
        public static FieldInfo[] GetFieldInfos(this object source)
        {
            return
                source.GetType()
                    .GetFields(Public | NonPublic | Instance)
                    .ToArray();
        }

        /// 簡易的獲取所有函式集合，包含Public | NonPublic | instance
        public static PropertyInfo[] GetPropertiesInfos(this object source)
        {
            return
                source.GetType()
                    .GetProperties(Public | NonPublic | Instance)
                    .ToArray();
        }

        /// 獲取指定 條件 的變數集合
        public static FieldInfo[] GetFieldInfos(this object source,
            Func<FieldInfo, bool> predicate)
        {
            return
                source
                    .GetFieldInfos()
                    .Where(predicate).ToArray();
            // output.Length > 0 ? output[0] : null
        }

        /// 獲取指定 類型 的變數集合
        public static FieldInfo[] GetFieldInfos<T>(this object source)
        {
            return source.GetFieldInfos(info => info.FieldType == typeof(T));
        }

        /// 獲取指定 名稱 的變數集合
        public static FieldInfo[] GetFieldInfos(this object source, string name)
        {
            return source.GetFieldInfos(info => info.Name == name);
        }

        /// 獲取指定 名稱 的變數
        public static FieldInfo GetFieldInfo(this object source, string name)
        {
            var output
                = source.GetFieldInfos(info => info.Name == name);
            return output.Length > 0 ? output[0] : null;
        }


        public static string GetIsNotNullToString<T>(this T obj)
        {
            // obj.GetType().Name
            return typeof(T).Name + " 是否不為空？\t" + (obj != null);
        }

        public static string GetNotZeroToString(this double obj)
        {
            return obj.GetType().Name + " 是否不為零？\t" + (obj != 0);
        }

        public static string GetNotZeroToString(this float obj)
        {
            return GetNotZeroToString((double) obj);
        }

        public static string GetNotZeroToString(this int obj)
        {
            return GetNotZeroToString((double) obj);
        }

        public static string GetMembersToString(this object obj)
        {
            string info = obj.GetType().Name;
            info += String.Join("\n",
                obj.GetFieldInfos().Select(p =>
                    (p.Name + "\t" + p.GetValue(obj))).ToArray());
            info += String.Join("\n",
                obj.GetType().GetPropertiesInfos().Select(p =>
                    (p.Name)).ToArray());
            /*
            info += obj.GetFieldInfos().Aggregate(info,
                (current, fieldInfo) => current + ("\n" + fieldInfo.Name + "\t" + fieldInfo.GetValue(obj)));
            info += obj.GetFieldInfos().Aggregate(info,
            (current, propertyInfo) => current + ("\n" + propertyInfo.Name));
            */
            return info;
        }


        public static string LogMethodName(this Debug method)
        {
            var stackTrace = new StackTrace();
            string message = stackTrace.GetFrame(1).GetMethod().Name;
            Debug.Log(message);
            return message;
        }

        public static string LogMethodName(this object method)
        {
            var stackTrace = new StackTrace();
            string message = stackTrace.GetFrame(1).GetMethod().Name;
            Debug.Log(message);
            return message;
        }
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

    public static class Array
    {
        /// 將2維陣列轉為1維
        public static T[] ToSingleArray<T>(this T[][] source)
        {
            if (source.IsEmpty())
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
        /// <summary>
        /// 不重複比對，比對嵌套陣列A內每一個孫元素C。
        /// *假設A為"嵌套陣列"，B為A的子"陣列"，C為B的孫"元素"。
        /// </summary>
        public static string PairwiseCompare<T>(this List<List<T>> origin) where T : Component
        {
            // 父物件 > 元素物件 = 目標物件
            // 蒐集結果
            string result = null;
            // 儲存父物件
            var parentList = origin;
            for (var i = 0; i < parentList.Count - 1; i++)
            {
                // 儲存要比對整個陣列的目標物件
                var targetsList = parentList[i];
                // 儲存目標物件辨識碼
                var targetListId = targetsList.GetHashCode();
                // 遍尋父物件
                for (var j = i + 1; j < parentList.Count; j++)
                {
                    var elementList = parentList[j];
                    // 如果輪到自己則跳過
                    if (targetListId == elementList.GetHashCode())
                    {
                        continue;
                    }

                    Console.WriteLine(elementList.GetHashCode());
                    // 兩個陣列子元素對比
                    foreach (T target in targetsList)
                    {
                        foreach (var element in elementList)
                        {
                            result += target + " + " + element + "\t";
                        }

                        result += "\n";
                    }

                    result += "\n";
                }
            }

            foreach (List<T> list in parentList)
            {
                // 儲存要比對整個陣列的目標物件
                var targetsList = list;
                // 儲存目標物件辨識碼
                var targetListId = targetsList.GetHashCode();
                // 遍尋父物件
                foreach (var elementList in parentList)
                {
                    // 如果輪到自己則跳過
                    if (targetListId == elementList.GetHashCode())
                    {
                        continue;
                    }

                    Console.WriteLine(elementList.GetHashCode());
                    // 兩個陣列子元素對比
                    foreach (T target in targetsList)
                    {
                        foreach (var element in elementList)
                        {
                            result += target + " + " + element + "\t";
                        }

                        result += "\n";
                    }

                    result += "\n";
                }
            }


            return result;
        }

        /// <summary>
        /// 比較目標B子陣列中每一個孫元素C與其他B嵌套陣列每一個孫元素C。
        /// *假設A為"嵌套陣列"，B為A的子"陣列"，C為B的孫"元素"。
        /// </summary>
        /// <param name="origin">嵌套陣列A。想要在其內部進行比對孫元素C的嵌套陣列</param>
        /// <param name="targetsListIndex">想要比較的目標B子陣列</param>
        public static string PairwiseCompare<T>(this List<T[]> origin, int targetsListIndex) where T : Component
        {
            // 父物件 > 元素物件 = 目標物件
            // 蒐集結果
            string result = null;
            // 儲存父物件
            var parentList = origin;
            // 儲存要比對的整個陣列的目標物件
            var targetArray = origin[targetsListIndex];
            // 儲存目標物件辨識碼
            var targetId = targetArray.GetHashCode();
            // 遍尋父物件
            foreach (T[] elementList in parentList)
            {
                // 如果輪到自己則跳過
                if (targetId == elementList.GetHashCode())
                {
                    continue;
                }

                Console.WriteLine(elementList.GetHashCode());
                // 兩個陣列子元素對比
                foreach (T targetChild in targetArray)
                {
                    foreach (var element in elementList)
                    {
                        result += targetChild + " + " + element + "\t";
                    }

                    result += "\n";
                }

                result += "\n";
            }

            return result;
        }

        public static string PairwiseCompare<T>(this List<List<T>> origin, int targetsListIndex) where T : Component
        {
            // 父物件 > 元素物件 = 目標物件
            // 蒐集結果
            string result = null;
            // 儲存父物件
            var parentList = origin;
            // 儲存要比對整個陣列的目標物件
            var targetsList = origin[targetsListIndex];
            // 儲存目標物件辨識碼
            var targetListId = targetsList.GetHashCode();
            // 遍尋父物件
            foreach (var elementList in parentList)
            {
                // 如果輪到自己則跳過
                if (targetListId == elementList.GetHashCode())
                {
                    continue;
                }

                Console.WriteLine(elementList.GetHashCode());
                // 兩個陣列子元素對比
                foreach (T target in targetsList)
                {
                    foreach (var element in elementList)
                    {
                        result += target + " + " + element + "\t";
                    }

                    result += "\n";
                }

                result += "\n";
            }

            return result;
        }

        public static string PairwiseCompareIgnoreCollision<T>(this List<List<T>> origin, bool ignore = true)
            where T : Collider2D
        {
            // 父物件 > 元素物件 = 目標物件
            // 蒐集結果
            string result = null;
            // 儲存父物件
            var parentList = origin;
            for (var i = 0; i < parentList.Count - 1; i++)
            {
                // 儲存要比對整個陣列的目標物件
                var targetsList = parentList[i];
                // 儲存目標物件辨識碼
                var targetListId = targetsList.GetHashCode();
                // 遍尋父物件
                for (var j = i + 1; j < parentList.Count; j++)
                {
                    var elementList = parentList[j];
                    // 如果輪到自己則跳過
                    if (targetListId == elementList.GetHashCode())
                    {
                        continue;
                    }

                    Console.WriteLine(elementList.GetHashCode());
                    // 兩個陣列子元素對比
                    foreach (T target in targetsList)
                    {
                        foreach (var element in elementList)
                        {
                            result += target + " + " + element + "\t";
                            Physics2D.IgnoreCollision(target, element, ignore);
                        }

                        result += "\n";
                    }

                    result += "\n";
                }
            }

            foreach (List<T> list in parentList)
            {
                // 儲存要比對整個陣列的目標物件
                var targetsList = list;
                // 儲存目標物件辨識碼
                var targetListId = targetsList.GetHashCode();
                // 遍尋父物件
                foreach (var elementList in parentList)
                {
                    // 如果輪到自己則跳過
                    if (targetListId == elementList.GetHashCode())
                    {
                        continue;
                    }

                    Console.WriteLine(elementList.GetHashCode());
                    // 兩個陣列子元素對比
                    foreach (T target in targetsList)
                    {
                        foreach (var element in elementList)
                        {
                            result += target + " + " + element + "\t";
                        }

                        result += "\n";
                    }

                    result += "\n";
                }
            }


            return result;
        }

        public static string PairwiseCompareIgnoreCollision<T>(this List<List<T>> origin, int targetsListIndex,
            bool ignore = true)
            where T : Collider2D
        {
            // 父物件 > 元素物件 = 目標物件
            // 蒐集結果
            string result = null;
            // 儲存父物件
            var parentList = origin;
            // 儲存要比對整個陣列的目標物件
            var targetsList = origin[targetsListIndex];
            // 儲存目標物件辨識碼
            var targetListId = targetsList.GetHashCode();
            // 遍尋父物件
            foreach (var elementList in parentList)
            {
                // 如果輪到自己則跳過
                if (targetListId == elementList.GetHashCode())
                {
                    continue;
                }

                Console.WriteLine(elementList.GetHashCode());
                // 兩個陣列子元素對比
                foreach (T target in targetsList)
                {
                    foreach (var element in elementList)
                    {
                        result += target + " + " + element + "\t";
                        Physics2D.IgnoreCollision(target, element, ignore);
                    }

                    result += "\n";
                }

                result += "\n";
            }

            return result;
        }

    }
}