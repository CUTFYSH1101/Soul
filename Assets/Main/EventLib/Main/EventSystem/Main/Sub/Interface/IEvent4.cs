using System;
using Main.EventLib.Main.EventSystem.Main.Interface;

namespace Main.EventLib.Main.EventSystem.Main.Sub.Interface
{
    public interface IEvent4 : IEvent
    {
        void First();
        void Act2();
        void Act3();
        void Act4();
        Func<bool> FilterIn { get; }
        Func<bool> ToAct2 { get; } // 同*
        Func<bool> ToAct3 { get; } // 同*
        Func<bool> ToAct4 { get; } // 同*
        Func<bool> ToInterrupt { get; }
    }
}