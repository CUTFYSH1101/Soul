using Main.Entity.Creature;
using Main.EventSystem.Event.Attribute;
using Main.EventSystem.Event.CreatureEventSystem.Common;
using Main.EventSystem.Event.CreatureEventSystem.Skill;

namespace Main.EventSystem.Event.CreatureEventSystem.Decorator
{
    public abstract class AbstractCreatureEventA : AbstractEventA
    {
        protected CreatureEvent CreatureEvent { get; }
        public CreatureInterface CreatureInterface { get;  }

        protected AbstractCreatureEventA(Creature creature, EventAttr eventAttr = default) : base(eventAttr)
        {
            CreatureEvent = new CreatureEvent(creature);
            CreatureInterface = new CreatureInterface(creature);
        }
        protected void InitCreatureEventOrder(EnumCreatureEventTag tag, EnumOrder order) =>
            CreatureEvent.InitEvent(tag, order, this);

        protected override void Invoke() => CreatureEvent.AppendToThread(base.Invoke);
    }
}