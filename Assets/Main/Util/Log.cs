using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Debug = UnityEngine.Debug;

namespace Main.Util
{
    /// 獲取腳本成員類相關方法、ToString方法
    public static class Log
    {
        public static void LogLine(this object message)
        {
            Debug.Log(message.ToString());
            Console.WriteLine(message.ToString());
        }
        public static void LogErrorLine(this object message)
        {
            Debug.LogError(message.ToString());
            Console.WriteLine(message.ToString());
        }
        /// 簡易的獲取所有變數集合，包含Public | NonPublic | instance
        public static FieldInfo[] GetFieldInfos(this object source)
        {
            return
                source.GetType()
                    .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .ToArray();
        }

        /// 簡易的獲取所有函式集合，包含Public | NonPublic | instance
        public static PropertyInfo[] GetPropertiesInfos(this object source)
        {
            return
                source.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .ToArray();
        }

        /// 獲取指定 條件 的變數集合
        public static FieldInfo[] GetFieldInfos(this object source,
            Func<FieldInfo, bool> filter)
        {
            return
                source
                    .GetFieldInfos()
                    .Filter(filter).ToArray();
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

        public static string LogMethodName(this object method)
        {
            var stackTrace = new StackTrace();
            string message = stackTrace.GetFrame(1).GetMethod().Name;
            Debug.Log(message);
            return message;
        }
        /// Log一個List內所有元素
        public static string LogList<T>(this List<T> source)
        {
            string text = null;
            text += $"陣列名： {nameof(source)}\n";
            foreach (var element in source)
                text += $"\t第 {source.IndexOf(element)} 個 {element.ToNameString()}\n";
            Debug.Log(text);
            return text;
        }

        /// Log一個嵌套List內所有元素
        public static string Log2DList<T>(this List<List<T>> source)
        {
            string text = null;
            foreach (var list in source)
            {
                text += "第 " + source.IndexOf(list) + " 個鎮列有 " + list.Count + " 個元素\n";
                foreach (var element in list)
                    text += $"\t第 {list.IndexOf(element)} 個 " + element + "\n";
                text += "\n";
            }

            Debug.Log(text);
            return text;
        }
    }
}