namespace Main.EventLib.Main.EventSystem
{
    public interface IInterruptible
    {
        public bool IsOpen { get; set; }

        #region MyRegion
        // public Action PreWork { get; set; }
        // public Action PostWork { get; set; }
        // public Action FinalEvent { get; set; }
        
        // public EnumState State { get; }
        // public UnityCoroutine Coroutine { get; set; }
        // void InterruptAndClose(); 
        #endregion
    }
}
/*
 if IsOpen == false (關閉事件)
    不允許外界呼叫Execute
    中斷停止任何正在執行的Act / 協程
    執行FinalAct
    State = EnumState.Free; // 回到一開始
*/