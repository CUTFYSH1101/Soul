namespace Main.Res.Script.Util
{
    public static class Math
    {
        public static float Remap(float outX, float outY, float inX, float inY, float inValue)
        {
            if ((inY - inX) == 0 || (outY - outX) == 0)
                return 0; // 避免分母為零的錯誤
            var differ1 = outY - outX;
            var differ2 = inY - inX;
            var rate = differ1 / differ2;
            return outX + (inValue - inX) * rate;
        }
    }
}