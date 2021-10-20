using System;
using Random = System.Random;
// enum為valueType無法繼承，所以泛型繼承struct, IConvertible
namespace Main.Util
{
    public static class Enum
    {
        private static int Random(int start, int end) =>
            new Random().Next(start, end);

        private static System.Array GetArray<T>() where T : struct, IConvertible =>
            System.Enum.GetValues(typeof(T));

        public static int GetSize<T>() where T : struct, IConvertible =>
            GetArray<T>().Length;

        public static int GetValue<T>(this T @enum) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum) throw new ArgumentException($"{typeof(T)} is not enum");
            return (int)(object)@enum;
        }

        public static int GetIndex<T>(this T @enum) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum) throw new ArgumentException($"{typeof(T)} is not enum");
            return System.Array.IndexOf(GetArray<T>(), @enum);
        }
        
        /// index to enum
        public static T GetEnum<T>(this int index) where T : struct, IConvertible
        {
            var array = System.Enum.GetValues(typeof(T));
            return (T)array.GetValue(index);
        }

        /// [0,Length)
        public static T Random<T>() where T : struct, IConvertible =>
            Random(0, GetSize<T>()).GetEnum<T>();

        /// [start,end]
        public static T Random<T>(T start, T end) where T : struct, IConvertible =>
            Random<T>(start.GetIndex(), end.GetIndex() + 1);

        /// [start,end)
        public static T Random<T>(int start, int end) where T : struct, IConvertible
        {
            var size = GetSize<T>();
            if (start < 0) start = 0;
            if (end > size) end = size;
            return Random(start, end).GetEnum<T>();
        }
    }
}