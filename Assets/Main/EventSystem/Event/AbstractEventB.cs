using Main.EventSystem.Event.Attribute;
using Main.EventSystem.Util;

namespace Main.EventSystem.Event
{
    public abstract class AbstractEventB : AbstractEvent
    {
        protected abstract void EnterAction1();
        protected abstract void Action2();
        protected abstract void Action3();
        protected abstract void ExitAction4();


        protected FuncCause
            CauseEnter, // 如果沒有就直接成立*
            CauseInterrupt, // 如果沒有就始終不成立
            CauseToAction2, // 同*
            CauseToAction3, // 同*
            CauseToAction4; // 同*


        protected override bool FinCauseEnter() =>
            EventAttr.CauseCd.OrCause() &&
            CauseEnter.OrCause;

        protected override bool FinCauseExit() =>
            EventAttr.CauseMaxDuration.AndCause() ||
            CauseInterrupt.AndCause;

        protected override void Invoke()
        {
            if (!Switch) return;

            if (State != EnumState.Free) return;


            Coroutine = new UnityCoroutine().CreateActionB(
                causeEnter: FinCauseEnter, () =>
                {
                    State = EnumState.InProcess1St; // 開始
                    // 避免一開始就滿足條件，而提早結束
                    EventAttr.CauseMaxDuration.Reset();
                    EventAttr.CauseCd.Reset(); // 開始計算CD時間
                    PreWork?.Invoke();
                    EnterAction1(); // 蓄力動畫
                },
                () => CauseToAction2.OrCause, () =>
                {
                    State = EnumState.InProgress2Nd;
                    Action2();
                },
                () => CauseToAction3.OrCause, () =>
                {
                    State = EnumState.InProgress3Rd;
                    Action3();
                },
                () => CauseToAction4.OrCause, () =>
                {
                    State = EnumState.InProgress4Th;
                    ExitAction4();
                    PostWork?.Invoke();
                    State = EnumState.Free; // 結束，回到一開始
                });

            new UnityCoroutineObserver().Create(Coroutine,
                FinCauseExit, () => // 必須要有條件才能打斷
                {
                    State = EnumState.InProgress4Th;
                    ExitAction4();
                    PostWork?.Invoke();
                    State = EnumState.Free; // 回到一開始
                });
        }

        protected AbstractEventB(EventAttr eventAttr = default) : base(eventAttr)
        {
        }
    }
}