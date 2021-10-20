namespace Main.Res.Script.Util
{
    /// 轉換
    public static class Convert
    {
        public static float ToFloat(this object source)
        {
            return float.Parse(source.ToString());
        }

        public static int ToInt(this object source)
        {
            return int.Parse(source.ToString());
        }
    }
}