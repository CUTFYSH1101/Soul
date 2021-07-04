using System;
using JetBrains.Annotations;

namespace Main.Game.Input
{
    public class InputManager
    {
        [CanBeNull] private static Action AllEventListener { get; set; }

        public InputManager AddEventListener(Event @event, string key, [NotNull] Action callback)
        {
            Action newEvent = @event switch
            {
                Event.Up => () =>
                {
                    if (Input.GetButtonUp(key)) callback.Invoke();
                },
                Event.Ing => () =>
                {
                    if (Input.GetButton(key)) callback.Invoke();
                },
                Event.Down => () =>
                {
                    if (Input.GetButtonDown(key)) callback.Invoke();
                },
                _ => throw new ArgumentOutOfRangeException(nameof(@event), @event, null)
            };
            AllEventListener += newEvent;
            return this;
        }

        public static void UpdateListener() => AllEventListener?.Invoke();
    }
}