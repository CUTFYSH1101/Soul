using Main.Common;
using UnityEngine;

namespace Main.Util.MyVector2
{
    /// <summary>
    /// 直接取用EX: MyVector2.InfinityRight => new Vector2(Value.Infinity,0);
    /// </summary>
    public struct MyVector2
    {
        public float X;

        public float Y;

        /// <summary>
        ///   <para>Shorthand for writing Vector2(0, 0).</para>
        /// </summary>
        public static Vector2 Zero => Vector2.zero;

        /// <summary>
        ///   <para>Shorthand for writing Vector2(1, 1).</para>
        /// </summary>
        public static Vector2 One => Vector2.one;

        /// <summary>
        ///   <para>Shorthand for writing Vector2(0, 1).</para>
        /// </summary>
        public static Vector2 Up => Vector2.up;

        /// <summary>
        ///   <para>Shorthand for writing Vector2(0, -1).</para>
        /// </summary>
        public static Vector2 Down => Vector2.down;

        /// <summary>
        ///   <para>Shorthand for writing Vector2(-1, 0).</para>
        /// </summary>
        public static Vector2 Left => Vector2.left;

        /// <summary>
        ///   <para>Shorthand for writing Vector2(1, 0).</para>
        /// </summary>
        public static Vector2 Right => Vector2.right;

        /// <summary>
        ///   <para>Shorthand for writing Vector2(float.PositiveInfinity, float.PositiveInfinity).</para>
        /// </summary>
        public static Vector2 PositiveInfinity => new Vector2(Values.Infinity, Values.Infinity);

        /// <summary>
        ///   <para>Shorthand for writing Vector2(float.NegativeInfinity, float.NegativeInfinity).</para>
        /// </summary>
        public static Vector2 NegativeInfinity =>
            new Vector2(-Values.Infinity, -Values.Infinity);


        /// <summary>
        ///   <para>Shorthand for writing Vector2(float.PositiveInfinity, float.PositiveInfinity).</para>
        /// </summary>
        public static Vector2 InfinityRight => new Vector2(Values.Infinity, 0);

        /// <summary>
        ///   <para>Shorthand for writing Vector2(float.PositiveInfinity, float.PositiveInfinity).</para>
        /// </summary>
        public static Vector2 InfinityUp => new Vector2(0, Values.Infinity);

        /// <summary>
        ///   <para>Shorthand for writing Vector2(float.NegativeInfinity, float.NegativeInfinity).</para>
        /// </summary>
        public static Vector2 InfinityLeft => new Vector2(-Values.Infinity, 0);

        /// <summary>
        ///   <para>Shorthand for writing Vector2(float.NegativeInfinity, float.NegativeInfinity).</para>
        /// </summary>
        public static Vector2 InfinityDown => new Vector2(0, -Values.Infinity);
    }
}