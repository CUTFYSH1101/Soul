using Main.Entity.Creature;
using Main.EventLib.Main.EventSystem.EventOrderbySystem;
using Main.EventLib.Main.EventSystem.Main.Interface;
using Main.EventLib.Sub.CreatureEvent;
using Main.EventLib.Sub.CreatureEvent.Skill;

namespace Main.EventLib.Main.EventSystem.Main.EventBuilder
{
    public class OrderByEventDirector : IEventDirector
    {
        private readonly Director _director;
        private readonly Element _element;

        public OrderByEventDirector(IEvent @event, Creature creature)
        {
            _director = new Director(@event);
            
            _element = new Element(creature);
            if (@event is IWorkOnCreature eventOnCreature)
                eventOnCreature.CreatureInterface = new CreatureInterface(creature); // 幫忙初始化creature介面
            if (@event is AbsEventObject absEventObject) absEventObject.Director = this;
        }
        
        public void CreateEvent() =>
            _element.AppendToThread(_director.CreateEvent);

        public OrderByEventDirector InitCreatureEventOrder(EnumCreatureEventTag tag, EnumOrder order)
        {
            _element.InitEvent(tag, order, _director.Builder.Event);
            return this;
        }
        
        public void InterruptAndClose() => _director.Builder.InterruptAndClose();
    }
}