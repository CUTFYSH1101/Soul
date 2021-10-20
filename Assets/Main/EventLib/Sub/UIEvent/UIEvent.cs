using System;
using JetBrains.Annotations;
using Main.EventLib.Main.EventSystem;
using Main.EventLib.Main.EventSystem.Main.Interface;
using Main.EventLib.Main.EventSystem.Main.Sub.Interface;

namespace Main.EventLib.Sub.UIEvent
{
    public abstract class UIEvent : AbsEventObject, IEvent3
    {
        public virtual UIEvent AppendToCdTime([NotNull] IEvent @event)
        {
            @event.PreWork += () =>
            {
                if (State == EnumState.Free)
                    // EventAttr.MaxDuration = @event.CdTime;
                    this.SetDuration(@event.EventAttr.CdTime);
                Director.CreateEvent();
            };
            return this;
        }

        public virtual UIEvent AppendOnEventEnter([NotNull] IEvent @event)
        {
            @event.PreWork += Director.CreateEvent;
            return this;
        }

        public virtual UIEvent AppendOnEventExit([NotNull] IEvent @event)
        {
            @event.PostWork += Director.CreateEvent;
            return this;
        }

        public abstract void Enter();

        public abstract void Update();

        public abstract void Exit();

        public Func<bool> FilterIn { get; set; }
        public Func<bool> ToInterrupt { get; set; }

        public float Duration
        {
            get => EventAttr.MaxDuration;
            set => EventAttr.MaxDuration = value;
        }

        public float CdTime
        {
            get => EventAttr.CdTime;
            set => EventAttr.CdTime = value;
        }
    }
    /*public abstract class UIEvent : AbstractEventA
    {
        protected UIEvent(EventAttr eventAttr = default) : base(eventAttr)
        {
        }
        public virtual UIEvent AppendToCdTime([NotNull] AbstractEvent @event)
        {
            @event.PreWork += () =>
            {
                if (State == EnumState.Free)
                    EventAttr.MaxDuration = @event.CdTime;
                Invoke();
            };
            return this;
        }
        public virtual UIEvent AppendOnEventEnter([NotNull] AbstractEvent @event)
        {
            @event.PreWork += Invoke;
            return this;
        }
        public virtual UIEvent AppendOnEventExit([NotNull] AbstractEvent @event)
        {
            @event.PostWork += Invoke;
            return this;
        }
    }*/
}