namespace Main.EventSystem.Event
{
    public interface IInterruptible
    {
        // public UnityCoroutine Coroutine { get; set; }
        // public Action PreWork { get; set; }
        // public Action PostWork { get; set; }
        // public Action FinalEvent { get; set; }
        public bool Switch { get; set; }
        // void InterruptAndClose();
    }
}
/*
當interrupt = true
    不允許執行任何Action
    不允許外界呼叫Invoke
    中斷任何正在執行的Action
    使整個協程關閉
    執行5.InterruptAndExit.Action
    State = EnumState.Free; // 回到一開始
*/
