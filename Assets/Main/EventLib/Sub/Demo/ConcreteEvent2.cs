using System;
using Main.EventLib.Main.EventSystem.Main;
using Main.EventLib.Main.EventSystem;
using Main.EventLib.Main.EventSystem.Main.Interface;
using Main.EventLib.Main.EventSystem.Main.Sub.Interface;

namespace Main.EventLib.Sub.Demo
{
    public class ConcreteEvent2 : AbsEventObject, IEvent2
    {
        public ConcreteEvent2(Action onEnter, Action onExit, float duration = 10)
        {
            this.Build();
            this.EventAttr = new EventAttr(maxDuration: duration);
            PreWork = onEnter;
            PostWork = onExit;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }

        public Func<bool> FilterIn { get; }
        public Func<bool> ToInterrupt { get; }
    }
}