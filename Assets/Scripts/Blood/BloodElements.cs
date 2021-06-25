namespace Blood {
    public static class BloodElements {
        private static readonly BloodElement Circle = new BloodElement ().SetElementName (0, "Circle");
        private static readonly BloodElement Square = new BloodElement ().SetElementName ( 1,"Square");
        private static readonly BloodElement Cross = new BloodElement ().SetElementName ( 2, "Cross");
        private static readonly BloodElement CircleEx = new BloodElement ().SetElementName (3, "CircleEX");
        private static readonly BloodElement SquareEx = new BloodElement ().SetElementName ( 4,"SquareEX");
        private static readonly BloodElement CrossEx = new BloodElement ().SetElementName (5, "CrossEX");

        private static readonly BloodElement[] AllElement = { Circle, Square, Cross, CircleEx, SquareEx, CrossEx };
        
        public static BloodElement GetCircle () => Circle;
        public static BloodElement GetSquare () => Square;
        public static BloodElement GetCross () => Cross;
        public static BloodElement GetCircleEx () => CircleEx;
        public static BloodElement GetSquareEx () => SquareEx;
        public static BloodElement GetCrossEx () => CrossEx;
        /// <summary>
        ///   <para>取得所有血元素資料，以陣列型式回傳。</para>
        /// </summary>
        public static BloodElement[] GetAllElement () => AllElement;
    }
}
