using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Main.Util
{
    public static class Algorithm
    {
        /// 交換陣列中兩個元素的位置
        public static T[] Swap<T>(this T[] a,
            int i, int j)
        {
            var temp = a[i];
            a[i] = a[j];
            a[j] = temp;
            return a;
        }

        private static void SubPermute<T>(T[] arr,
            int l, int r)
        {
            if (l == r)
                arr.ArrayToString().LogLine();
            else
            {
                for (int i = l; i <= r; i++)
                {
                    arr = Swap(arr, l, i);
                    SubPermute(arr, l + 1, r);
                    arr = Swap(arr, l, i);
                }
            }
        }

        /// 全排列
        public static void Permute<T>(this T[] arr)
        {
            SubPermute(arr, 0, arr.Length - 1);
        }

        private static void SubCombination<T>(ref List<T[]> list, T[] arr, T[] data,
            int start, int end,
            int index, int r,
            [CanBeNull] Func<T, bool> filter = null)
        {
            if (index == r)
            {
                List<T> temp = new List<T>();
                foreach (var value in data) temp.Add(value);
                list.Add(temp.ToArray());
                return;
            }

            // replace index with all
            // possible elements. The
            // condition "end-i+1 >=
            // r-index" makes sure that
            // including one element
            // at index will make a
            // combination with remaining
            // elements at remaining positions
            if (index != r)
            {
                for (int i = start;
                    i <= end && end - i + 1 >= r - index;
                    i++)
                {
                    if (filter != null && !filter(arr[i]))
                        continue;
                    data[index] = arr[i];
                    SubCombination(ref list, arr, data, i + 1,
                        end, index + 1, r, filter);
                }
            }
        }

        public static T[][] Combination<T>(this T[] arr,
            int n, int r, [CanBeNull] Func<T, bool> filter = null)
        {
            T[] data = new T[r];
            List<T[]> list = new List<T[]>();
            if (filter == null) SubCombination(ref list, arr, data, 0, n - 1, 0, r);
            else SubCombination(ref list, arr, data, 0, n - 1, 0, r, filter);
            /*foreach (var value in list)
            {
                value.ArrayToString().LogLine();
            }*/
            return list.ToArray();
        }

        /// 兩兩不重複比對的排列，順序會被打亂
        public static T[][] CombinationBinary<T>(this T[] array)
        {
            Queue<T[]> list = new Queue<T[]>();
            Stack<T> _list = new Stack<T>();
            for (var i = 0; i < array.Length - 1; i++)
            {
                var objA = array[i];
                _list.Clear();
                _list.Push(objA);
                for (var j = i + 1; j < array.Length; j++)
                {
                    var objB = array[j];
                    _list.Push(objB);
                    list.Enqueue(_list.ToArray());
                    _list.Pop();
                }
            }

            return list.ToArray();
        }

        /// 兩兩不重複比對的排列，順序會被打亂
        public static T[][] CombinationBinary<T>(this T[] array, [NotNull] Func<T, bool> filter)
        {
            Queue<T[]> list = new Queue<T[]>();
            Stack<T> _list = new Stack<T>();
            for (var i = 0; i < array.Length - 1; i++)
            {
                var objA = array[i];
                if (!filter(objA)) continue;
                _list.Clear();
                _list.Push(objA);
                for (var j = i + 1; j < array.Length; j++)
                {
                    var objB = array[j];
                    if (!filter(objB)) continue;
                    _list.Push(objB);
                    list.Enqueue(_list.ToArray());
                    _list.Pop();
                }
            }

            return list.ToArray();
        }
    }
}