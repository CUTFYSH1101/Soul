using System.Linq;
using Main.Util;

namespace Main.Extension.Util
{
    public static class String
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="start">最小值0</param>
        /// <param name="end">最大值source.Length+1</param>
        /// <returns></returns>
        public static string SubString(this string source, int start, int end)
        {
            if (start <= 0) start = 0;
            if (end >= source.Length) end = source.Length;
            var arr = source.ToCharArray();
            string result = "";
            for (var i = start; i < end; i++) result += arr[i];
            return result;
        }

        public static string ToNameString<T>(this T source) =>
            typeof(T).Name + source;

        public static string GetIsNotNullString<T>(this T source) =>
            typeof(T).Name + " 是否不為空？\t" + (source != null);

        public static string GetNotZeroString(this double source) =>
            source.GetType().Name + " 是否不為零？\t" + (source != 0);

        public static string GetNotZeroString(this float source) =>
            GetNotZeroString((double) source);

        public static string GetNotZeroString(this int source) =>
            GetNotZeroString((double) source);

        public static string[] GetMembersToNameString(this object source) =>
            /*source.GetFieldInfosToNameString().Concat(
                source.GetPropertiesInfosToNameString()).ToArray();*/
            source.GetFieldInfosToNameString();

        public static string[] GetFieldInfosToNameString(this object source) =>
            source.GetFieldInfos().Select(p => p.Name + "\t" + p.GetValue(source)).ToArray();

        public static string[] GetPropertiesInfosToNameString(this object source) =>
            source.GetPropertiesInfos().Select(p => p.Name + "\t" + p.GetValue(source)).ToArray();
    }
}