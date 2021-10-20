using System;
using Main.EventLib.Main.EventSystem.Main.Interface;

namespace Main.EventLib.Main.EventSystem.Main.Sub.Interface
{
    public interface IEvent1 : IEvent
    {
        void Action();
        Func<bool> FilterIn { get; }
    }
}