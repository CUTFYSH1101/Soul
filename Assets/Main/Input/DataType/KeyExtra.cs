namespace Main.Input.DataType
{
    /// <summary>區別正向與反向</summary>
    /// <example>方向鍵左代表-1，方向鍵右代表+1</example>
    public enum PorN
    {
        Positive,
        Negative
    }

    public enum Type
    {
        Key,
        Mouse,
        JoyStick
    }

    public enum Event
    {
        Up,
        Ing,
        Down
    }

    public interface IKey
    {
        PorN PorN { get; set; }
        bool IsButton(Event @event);
        public int GetAxisRaw();
        public int GetAxisRawDown();
    }

    /// 後端。目前只支援key和mouse
    public abstract class KeyExtra<T> : IKey
    {
        public PorN PorN { get; set; }
        public Type Type { get; }
        public T Value { get; }
        public abstract bool IsButton(Event @event);

        public virtual int GetAxisRaw()
        {
            bool anyP = PorN == PorN.Positive && IsButton(Event.Ing);
            bool anyN = PorN == PorN.Negative && IsButton(Event.Ing);
            if (anyP)
                return 1;
            else if (anyN)
                return -1;
            else
                return 0;
        }

        public virtual int GetAxisRawDown()
        {
            bool anyP = PorN == PorN.Positive && IsButton(Event.Down);
            bool anyN = PorN == PorN.Negative && IsButton(Event.Down);
            if (anyP)
                return 1;
            else if (anyN)
                return -1;
            else
                return 0;
        }

        protected KeyExtra(PorN porN, Type type, T value)
        {
            PorN = porN;
            Type = type;
            Value = value;
        }
    }
}