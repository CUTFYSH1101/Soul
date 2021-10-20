using Main.EventLib.Main.EventSystem.Main.EventBuilder;
using Main.EventLib.Main.EventSystem.Main.Sub.Interface;

namespace Main.EventLib.Main.EventSystem.Main.Sub.Builder
{
    public class Event1Builder : AbsBuilder
    {
        public Event1Builder(IEvent1 @event) : base(@event)
        {
        }

        public override void CreateEvent()
        {
            var builder = this;
            if (Event is not IEvent1 e) return;
            
            e.Action();
        }
    }
}