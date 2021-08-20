using State = Main.EventSystem.Event.EnumState;

namespace Main.EventSystem.Event
{
    public enum EnumState
    {
        Free, // 等待呼叫
        InProcess, // 執行中...
        InProcess1St,
        InProgress2Nd,
        InProgress3Rd,
        InProgress4Th,
        Finished // 完成
    }
}