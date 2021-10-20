using System;

namespace Main.Input.DataType
{
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