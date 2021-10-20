using Main.EventLib.Main.EventSystem.Main.Interface;
using Main.EventLib.Main.EventSystem.Main.Sub.Interface;
using Main.Game.Coroutine;

namespace Main.EventLib.Main.EventSystem.Main.EventBuilder
{
    public abstract class AbsBuilder
    {
        public IEvent Event { get; }
        public UnityCoroutine Coroutine { get; protected set; }// 在子類中實例化，主循環程式執行驅
        protected AbsBuilder(IEvent @event) => Event = @event;// 繼承當下馬上實例化

        public bool PreCreateCheck()
        {
            if (!Event.IsOpen) return false;
            if (Event.State != EnumState.Free || !FinFilterIn()) return false;

            return true;
        }

        public abstract void CreateEvent();

        protected bool FinFilterIn()
        {
            return Event switch
            {
                IEvent1 e => e.EventAttr.CdCondition.OrCause() && e.FilterIn.OrCause(),
                IEvent2 e => e.EventAttr.CdCondition.OrCause() && e.FilterIn.OrCause(),
                IEvent3 e => e.EventAttr.CdCondition.OrCause() && e.FilterIn.OrCause(),
                IEvent4 e => e.EventAttr.CdCondition.OrCause() && e.FilterIn.OrCause(),
                _ => false
            };
        }

        protected bool FinFilterOut()
        {
            return Event switch
            {
                IEvent1 e => true,
                IEvent2 e => e.EventAttr.DurationCondition.AndCause() || e.ToInterrupt.AndCause(),
                IEvent3 e => e.EventAttr.DurationCondition.AndCause() || e.ToInterrupt.AndCause(),
                IEvent4 e => e.EventAttr.DurationCondition.AndCause() || e.ToAct4.AndCause(),
                _ => false
            };
        }
        
        internal void InterruptAndClose()
        {
            /*
             if IsOpen == false (關閉事件)
                不允許外界呼叫Execute
                中斷停止任何正在執行的Act / 協程
                執行FinalAct
                State = EnumState.Free; // 回到一開始
            */
            Coroutine?.InterruptCoroutine();
            Event.FinalAct?.Invoke();
            Event.PostWork?.Invoke();
            Event.State = EnumState.Free;
        }
    }
}