using System;
using Main.EventSystem.Event.Attribute;
using Main.EventSystem.Util;

namespace Main.EventSystem.Event
{
    public abstract class AbstractEventA : AbstractEvent
    {
        protected abstract void Enter();
        protected abstract void Update();
        protected abstract void Exit();

        protected Action PreUpdate;
        protected FuncCause CauseEnter, CauseExit;

        // cd null允許進，enter null 允許進
        protected override bool FinCauseEnter() =>
            EventAttr.CauseCd.OrCause() &&
            CauseEnter.OrCause;

        // maxDuration null默認沒有，exit null默認沒有，兩者null則程式除非停止，否則持續運作
        protected override bool FinCauseExit() =>
            EventAttr.CauseMaxDuration.AndCause() ||
            CauseExit.AndCause;

        protected override void Invoke()
        {
            if (!Switch) return;
            
            if (State != EnumState.Free) return;

            Coroutine = new UnityCoroutine().CreateActionA(
                FinCauseEnter, () =>
                {
                    State = EnumState.InProcess;
                    EventAttr.CauseMaxDuration.Reset();
                    EventAttr.CauseCd.Reset();
                    PreWork?.Invoke();
                    Enter();
                },
                FinCauseExit, () =>
                {
                    Exit();
                    PostWork?.Invoke();
                    State = EnumState.Free;
                },
                () =>
                {
                    PreUpdate?.Invoke();
                    Update();
                });
        }

        protected AbstractEventA(EventAttr eventAttr = default) : base(eventAttr)
        {
        }
    }
}