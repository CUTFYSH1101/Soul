using System;
using UnityEngine;

namespace Main.Input.DataType
{
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
}