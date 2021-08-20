namespace Main.AnimAndAudioSystem.Main.Util
{
    public static class Math
    {
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
}