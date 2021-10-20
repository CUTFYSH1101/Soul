namespace Main.EventLib.Main.EventSystem.Main.EventBuilder
{
    public interface IEventDirector
    {
        /// 執行事件
        public void CreateEvent();
        /// 中止事件進行
        public void InterruptAndClose();
    }
}