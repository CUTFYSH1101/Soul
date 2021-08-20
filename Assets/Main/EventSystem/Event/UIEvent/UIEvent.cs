using System;
using JetBrains.Annotations;
using Main.EventSystem.Event.Attribute;
using Main.EventSystem.Event.CreatureEventSystem.Skill;

namespace Main.EventSystem.Event.UIEvent
{
    public abstract class UIEvent : AbstractEventA
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
    }
}