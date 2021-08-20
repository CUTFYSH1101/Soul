using System;

namespace Main.EventSystem.Event.Demo.EventC
{
    public class ConcreteEventC : AbstractEventC
    {
        public ConcreteEventC(Action onEnter, Action onExit, float duration = 10) : base(0, duration)
        {
            // CauseEnter = new FuncCause();
            PreWork = onEnter;
            PostWork = onExit;
        }

        public new void Invoke() => base.Invoke();
        protected override void Enter()
        {
        }

        protected override void Exit()
        {
        }
    }
}