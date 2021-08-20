using System;

namespace Main.AnimAndAudioSystem.Audios.Scripts.Attribute
{
    [Serializable] //为了显示在Inspect面板上需要将结构体 进行序列化操作
    public struct RangedFloat
    {
        public float minValue;
        public float maxValue;
    }

    /// <summary>
    /// 自定义属性 用于控制 取值范围
    /// </summary>
    public class MinMaxRangeAttribute : System.Attribute
    {
        public MinMaxRangeAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float Min { get; private set; }
        public float Max { get; private set; }
    }
}