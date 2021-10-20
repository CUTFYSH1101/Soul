using System;

namespace Main.Input.DataType
{
    public class ExtraJoystick : KeyExtra<EnumJoyStick>
    {
        private readonly Direction _direction;

        public ExtraJoystick(PorN porN, EnumJoyStick value, Direction direction = Direction.Horizontal) : base(porN,
            Type.JoyStick, value)
        {
            _direction = direction;
        }

        public override int GetAxisRaw() => Value.GetAxisRaw(_direction);
        
        public override bool IsButton(Event @event)
        {
            return @event switch
            {
                Event.Up => Value.GetKeyUp(),
                Event.Ing => Value.GetKey(),
                Event.Down => Value.GetKeyDown(),
                _ => throw new ArgumentOutOfRangeException(nameof(@event), @event, null)
            };
        }
    }
}