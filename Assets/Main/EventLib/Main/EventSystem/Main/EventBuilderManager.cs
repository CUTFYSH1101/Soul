using System.Diagnostics.CodeAnalysis;
using Main.Entity.Creature;
using Main.EventLib.Main.EventSystem.EventOrderbySystem;
using Main.EventLib.Main.EventSystem.Main.EventBuilder;
using Main.EventLib.Main.EventSystem.Main.Interface;
using Main.EventLib.Sub.CreatureEvent;

namespace Main.EventLib.Main.EventSystem.Main
{
    public static class EventBuilderManager
    {
        public static IEventDirector Build(this IEvent @event)
        {
            // 預設 EventAttr, CreatureInterfaceForSkill
            return new EventBuilder.Director(@event);
        }

        public static IEventDirector Build(this IEvent @event, [NotNull] Creature creature, EnumOrder order,
            EnumCreatureEventTag tag)
        {
            // 預設 EventAttr, CreatureInterfaceForSkill
            return new OrderByEventDirector(@event, creature)
                .InitCreatureEventOrder(tag, order);
        }

        public static IEventDirector Build(this IEvent @event, [NotNull] Creature creature, EnumCreatureEventTag tag,
            EnumOrder order) => @event.Build(creature, order, tag);
    }
}