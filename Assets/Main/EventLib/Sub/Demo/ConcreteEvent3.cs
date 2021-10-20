using System;
using Main.EventLib.Main.EventSystem.Main;
using Main.Util;
using Main.EventLib.Main.EventSystem.Main.Interface;
using Main.EventLib.Main.EventSystem.Main.Sub.Interface;

namespace Main.EventLib.Sub.Demo
{
    public class ConcreteEvent3 : AbsEventObject, IEvent3
    {
        public ConcreteEvent3()
        {
            this.Build();
            FinalAct = () => $"Exit {FilterIn()} {FilterIn()}".LogLine();
        }

        public void Execute() => Director.CreateEvent();
        public void Enter() => $"Enter {FilterIn()} {FilterIn()}".LogLine();

        public void Update() => $"Update {FilterIn()} {FilterIn()}".LogLine();

        public void Exit() {}
        // public void Exit() => $"Exit {FilterIn()} {FilterIn()}".LogLine();

        public Func<bool> FilterIn { get; }
        public Func<bool> ToInterrupt { get; }
    }
}