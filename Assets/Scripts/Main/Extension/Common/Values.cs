namespace Main.Common
{
    /// <summary>
    /// 自定義數值
    /// </summary>
    public struct Values
    {
        /// <summary>
        /// 9999
        /// </summary>
        public static int Infinity => 9999;
        /// <summary>
        /// .1f
        /// </summary>
        public static float Min => .1f;
        /// <summary>
        /// 0
        /// </summary>
        public static float Zero => 0;
    }

    /// <summary>
    /// 獲取LayerMask的value
    /// </summary>
    public struct LayerMask
    {
        /// <summary>
        /// 獲取地板的layerMask value
        /// </summary>
        public static int Ground => 1024;

        /// <summary>
        /// 獲取Creature的layerMask value
        /// </summary>
        public static int Creature => 2048;
    }

    /// <summary>
    /// 獲取Layer的index值
    /// </summary>
    public struct Layer
    {
        public static int
            Default = 0,
            TransparentFX = 1,
            IgnoreRaycast = 2,
            Water = 4,
            UI = 5,
            Ground = 10,
            Creature = 11,
            Item = 12,
            Attack = 13,
            TargetSensor = 14;
    }

    public enum Symbol
    {
        None,   // 類似null
        Direct, // 直接傷害
        Square,
        Cross,
        Circle,
        AltAll, // 所有特殊符號
        AltSquare,
        AltCross,
        AltCircle,
    }

    public enum Team
    {
        Peace,
        Evil,
        Player,
        Enemy
    }
}