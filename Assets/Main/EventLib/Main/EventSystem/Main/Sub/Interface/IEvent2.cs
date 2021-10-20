using System;
using Main.EventLib.Main.EventSystem.Main.Interface;

namespace Main.EventLib.Main.EventSystem.Main.Sub.Interface
{
    public interface IEvent2 : IEvent
    {
        void Enter();
        void Exit();
        Func<bool> FilterIn { get; }
        Func<bool> ToInterrupt { get; }
    }
}