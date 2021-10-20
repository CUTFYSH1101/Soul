using System.ComponentModel;

namespace Main.EventLib.Common
{
    /// 角色心智狀態
    public enum EnumMindState
    {
        [Description("閒置")] Idle,
        [Description("戰鬥狀態")] Aware,
        [Description("移動")] Moving,
        [Description("衝刺")] Dashing,
        [Description("攻擊")] Attacking,
        [Description("格檔")] Parry,
        [Description("進入QTE事件")] UnderQteEvent,
    }

    /// 角色環境狀態。
    public enum SurroundingState
    {
        Air,
        Grounded
    }
}