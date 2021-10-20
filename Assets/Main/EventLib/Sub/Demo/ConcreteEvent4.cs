using System;
using Main.EventLib.Main.EventSystem.Main;
using Main.Util;
using Main.EventLib.Condition;
using Main.EventLib.Main.EventSystem.Main.Interface;
using Main.EventLib.Main.EventSystem.Main.Sub.Interface;

namespace Main.EventLib.Sub.Demo
{
    public class ConcreteEvent4 : AbsEventObject, IEvent4
    {
        private CdCondition wait = new(2);
        public ConcreteEvent4()
        {
            this.Build();

            FilterIn = () => true;
            // CauseInterrupt = () => false;
            ToAct2 = () => true;
            ToAct3 = () => wait.OrCause();
            ToAct4 = () => true;
        }

        public void Execute() => Director.CreateEvent();
        public void First() => "EnterAction1".LogLine();

        public void Act2()
        {
            $"Action2 {State}".LogLine();
            wait.Reset();
        }

        public void Act3() => "Action3".LogLine();

        public void Act4() => "ExitAction4".LogLine();

        public Func<bool> FilterIn { get; }
        public Func<bool> ToInterrupt { get; }
        public Func<bool> ToAct2 { get; }
        public Func<bool> ToAct3 { get; }
        public Func<bool> ToAct4 { get; }
    }
}