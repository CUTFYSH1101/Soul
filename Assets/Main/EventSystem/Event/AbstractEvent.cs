using System;
using Main.EventSystem.Event.Attribute;
using Main.EventSystem.Util;
using Time = Main.Util.Time;

// using Time = UnityEngine.Time;

namespace Main.EventSystem.Event
{
    public abstract class AbstractEvent : IInterruptible
    {
        protected EnumState State { get; set; } = EnumState.Free;
        public bool Finished => State == EnumState.Free;
        protected readonly EventAttr EventAttr;
        protected abstract bool FinCauseEnter();
        protected abstract bool FinCauseExit();
        protected abstract void Invoke();

        public virtual float CdTime
        {
            get => EventAttr.CdTime;
            set => EventAttr.CdTime = value;
        }

        protected AbstractEvent(EventAttr eventAttr = default)
        {
            eventAttr ??= new EventAttr();
            EventAttr = eventAttr;
        }

        protected float TimeDeltaTime => Time.DeltaTime;


        // 使可中途中斷事件
        protected UnityCoroutine Coroutine;
        public Action PreWork { get; set; }
        public Action PostWork { get; set; }
        protected Action FinalEvent;
        private bool _switch = true;

        public bool Switch
        {
            get => _switch;
            set
            {
                _switch = value;
                if (!value) InterruptAndClose();
            }
        }

        protected virtual void InterruptAndClose()
        {
            /*
            當interrupt = true
                不允許執行任何Action
                不允許外界呼叫Invoke
                中斷任何正在執行的Action
                使整個協程關閉
                執行5.InterruptAndExit.Action
                State = EnumState.Free; // 回到一開始
            */
            Coroutine?.InterruptCoroutine();
            FinalEvent?.Invoke();
            PostWork?.Invoke();
            State = EnumState.Free;
        }
    }
}