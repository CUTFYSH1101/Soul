using UnityEngine;

namespace Main.Util
{
    public static class Math
    {
        /// <summary>
        /// 使某數往某數方向減少。
        /// 用法： int num = num.Less(1);
        /// </summary>
        public static int Less(this ref int num, int minus, int middle = 0)
        {
            // 把less轉為正數
            minus = System.Math.Abs(minus);
            // 數字大於中間數，則減少，反之增加
            num -= num > middle ? minus : -minus;
            // 當小於一定值則歸零
            num = System.Math.Abs(num) < .1f ? middle : num;
            return num;
        }

        /// <summary>
        /// 使某數往某數方向減少
        /// 用法： int num = num.Less(1);
        /// </summary>
        public static float Less(this ref float num, float minus, float middle = 0)
        {
            // 把less轉為正數
            minus = System.Math.Abs(minus);
            // 數字大於中間數，則減少，反之增加
            num += num > middle ? -minus : minus;
            // 當小於一定值則歸零
            num = System.Math.Abs(num) < .1f ? middle : num;
            return num;
        }

        /// <summary>
        /// 使某數往某數方向減少
        /// 用法： int num = num.Less(1);
        /// </summary>
        public static double Less(this ref double num, double minus, double middle = 0)
        {
            // 把less轉為正數
            minus = System.Math.Abs(minus);
            // 數字大於中間數，則減少，反之增加
            num -= num > middle ? minus : -minus;
            // 當小於一定值則歸零
            num = System.Math.Abs(num) < .1f ? middle : num;
            return num;
        }

        /// <code>
        /// 公式: x + (value-X) * rate;
        /// rate = [(y-x)/(Y-X)]
        /// </code>
        /// <param name="x">映射後輸出的最小值</param>
        /// <param name="y">映射後輸出的最大值</param>
        /// <param name="X">映射前最小值</param>
        /// <param name="Y">映射前最大值</param>
        /// <param name="value">要被映射的值</param>
        /// <returns>回傳結果</returns>
        public static double Remap(double x, double y, double X, double Y, double value)
        {
            if (Y == X || y == x)
                return 0; // 避免分母為零的錯誤
            var differ1 = y - x;
            var differ2 = Y - X;
            var rate = differ1 / differ2;
            return x + (value - X) * rate;
        }
        public static float Remap(float x, float y, float X, float Y, float value)
        {
            if (Y == X || y == x)
                return 0; // 避免分母為零的錯誤
            var differ1 = y - x;
            var differ2 = Y - X;
            var rate = differ1 / differ2;
            return x + (value - X) * rate;
        }
    }

    public static class Vector2Extension
    {
        /// <summary>
        /// 使某向量往某個向量減少
        /// </summary>
        public static Vector2 Less(this Vector2 num, Vector2 less, Vector2 middle = default)
        {
            // 把less轉為正數
            less.x = System.Math.Abs(less.x);
            less.y = System.Math.Abs(less.y);
            // 數字大於中間數，則減少，反之增加
            num.x -= num.x > middle.x ? less.x : -less.x;
            num.y -= num.y > middle.y ? less.y : -less.y;
            return num;
        }

        /// <summary>
        /// 回傳A到B最短距離
        /// </summary>
        public static Vector2 Distance(this Component from, Component to)
        {
            return to.transform.position - from.transform.position;
        }

        /// <summary>
        /// 旋轉某一個向量，以弧度為單位。
        /// </summary>
        public static Vector2 Rotate(this Vector2 source, float radians)
        {
            return new Vector2(
                source.x * Mathf.Cos(radians) - source.y * Mathf.Sin(radians),
                source.x * Mathf.Sin(radians) + source.y * Mathf.Cos(radians)
            );
        }

        /// <summary>
        /// 回傳向量
        /// </summary>
        public static Vector2 Normalize(this Vector2 from, Vector2 to)
        {
            return (to - from).normalized;
        }

        /// <summary>
        /// 回傳起點至終點的歸一化向量
        /// </summary>
        /// <param name="from">起點</param>
        /// <param name="to">終點</param>
        /// <returns></returns>
        public static Vector2 Normalize(this Component from, Component to)
        {
            return (to.transform.position - from.transform.position).normalized;
        }
    }
}