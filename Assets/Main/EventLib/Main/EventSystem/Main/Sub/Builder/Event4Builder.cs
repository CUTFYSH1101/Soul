using Main.EventLib.Main.EventSystem.Main.EventBuilder;
using Main.EventLib.Main.EventSystem.Main.Sub.Interface;
using Main.Game.Coroutine;

namespace Main.EventLib.Main.EventSystem.Main.Sub.Builder
{
    public class Event4Builder : AbsBuilder
    {
        public Event4Builder(IEvent4 @event) : base(@event)
        {
        }

        public override void CreateEvent()
        {
            var builder = this;
            if (Event is not IEvent4 e) return;

            void FinalAct()
            {
                e.Act4();
                InterruptAndClose();
                /*e.FinalAct?.Invoke();
                e.PostWork?.Invoke();
                e.State = EnumState.Free; // 結束，回到一開始*/
            }

            builder.Coroutine = new UnityCoroutine().Create(
                new ActionData(builder.FinFilterIn, () =>
                    {
                        e.State = EnumState.InProcess;
                        // 避免一開始就滿足條件，而提早結束
                        e.EventAttr.DurationCondition.Reset();
                        e.EventAttr.CdCondition.Reset(); // 開始計算CD時間
                        e.PreWork?.Invoke();
                        e.First(); // 蓄力動畫
                    }
                ),
                new ActionData(e.ToAct2.OrCause, e.Act2),
                new ActionData(e.ToAct3.OrCause, e.Act3),
                new ActionData(e.ToAct4.OrCause, FinalAct)
            );
            /*new UnityCoroutineObserver().CoroutineCheckDoneAndDoFinAct(Coroutine, e.ToAct4.AndCause,
                FinalAction);*/
            new UnityCoroutineObserver().CoroutineCheckDoneAndDoFinAct(Coroutine, e.ToInterrupt.AndCause,
                FinalAct);
        }
    }
}