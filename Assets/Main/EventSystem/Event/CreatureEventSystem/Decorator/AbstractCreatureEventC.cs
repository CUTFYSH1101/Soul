using Main.Entity.Creature;
using Main.EventSystem.Event.CreatureEventSystem.Common;
using Main.EventSystem.Event.CreatureEventSystem.Skill;

namespace Main.EventSystem.Event.CreatureEventSystem.Decorator
{
    public abstract class AbstractCreatureEventC : AbstractEventC
    {
        protected CreatureEvent CreatureEvent { get; }
        public CreatureInterface CreatureInterface { get; }
        protected AbstractCreatureEventC(AbstractCreature creature, float cdTime = 0, float duration = 10) :
            base(cdTime, duration)
        {
            CreatureEvent = new CreatureEvent(creature);
            CreatureInterface = new CreatureInterface(creature);
        }

        protected void InitCreatureEventOrder(EnumCreatureEventTag tag, EnumOrder order) =>
            CreatureEvent.InitEvent(tag, order, this);

        protected override void Invoke() => CreatureEvent.AppendToThread(base.Invoke);
    }
}