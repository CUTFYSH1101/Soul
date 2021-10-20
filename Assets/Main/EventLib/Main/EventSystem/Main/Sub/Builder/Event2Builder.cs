using Main.EventLib.Main.EventSystem.Main.EventBuilder;
using Main.EventLib.Main.EventSystem.Main.Sub.Interface;
using Main.Game.Coroutine;

namespace Main.EventLib.Main.EventSystem.Main.Sub.Builder
{
    public class Event2Builder : AbsBuilder
    {
        public Event2Builder(IEvent2 @event) : base(@event)
        {
        }
        public override void CreateEvent()
        {
            var builder = this;
            if (Event is not IEvent2 e) return;
            builder.Coroutine = new UnityCoroutine().Create(
                new ActionData(builder.FinFilterIn,() =>
                    {
                        e.State = EnumState.InProcess;
                        e.EventAttr.DurationCondition.Reset();
                        e.EventAttr.CdCondition.Reset();
                        e.PreWork?.Invoke();
                        e.Enter();
                    }
                ),
                new ActionData(builder.FinFilterOut, () =>
                    {
                        e.Exit();
                        InterruptAndClose();
                        /*e.PostWork?.Invoke();
                        e.State = EnumState.Free;*/
                    }
                )
            );
        }
    }
}