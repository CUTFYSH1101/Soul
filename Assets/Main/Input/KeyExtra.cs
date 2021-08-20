using System;
using UnityEngine;

namespace Main.Input
{
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
    }

    /// 後端。目前只支援key和mouse
    public abstract class KeyExtra<T> : IKey
    {
        public PorN PorN { get; set; }
        public Type Type { get; private set; }
        public T Value { get; private set; }
        public abstract bool IsButton(Event @event);

        protected KeyExtra(PorN porN, Type type, T value)
        {
            PorN = porN;
            Type = type;
            Value = value;
        }
    }

    public class ExtraKey : KeyExtra<KeyCode>
    {
        public ExtraKey(PorN porN, KeyCode value) : base(porN, Type.Key, value)
        {
        }

        public override bool IsButton(Event @event)
        {
            return @event switch
            {
                Event.Up => UnityEngine.Input.GetKeyUp(Value),
                Event.Ing => UnityEngine.Input.GetKey(Value),
                Event.Down => UnityEngine.Input.GetKeyDown(Value),
                _ => throw new ArgumentOutOfRangeException(nameof(@event), @event, null)
            };
        }
    }

    public class ExtraMouse : KeyExtra<int>
    {
        public ExtraMouse(PorN porN, int value) : base(porN, Type.Mouse, value)
        {
        }

        public override bool IsButton(Event @event)
        {
            return @event switch
            {
                Event.Up => UnityEngine.Input.GetMouseButtonUp(Value),
                Event.Ing => UnityEngine.Input.GetMouseButton(Value),
                Event.Down => UnityEngine.Input.GetMouseButtonDown(Value),
                _ => throw new ArgumentOutOfRangeException(nameof(@event), @event, null)
            };
        }
    }
}