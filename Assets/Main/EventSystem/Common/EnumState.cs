using System.ComponentModel;

namespace Main.EventSystem.Common
{
    /// 角色心智狀態
    public enum EnumMindState
    {
        [Description("閒置")] Idle,
        [Description("戰鬥狀態")] Aware,
        [Description("移動")] Move,
        [Description("攻擊")] Attack,
        // [Description("僵直，持續0.5秒")] Stiff,
        // [Description("暈眩，眼冒金星，持續1秒")] Dizzy,
        // [Description("昏迷，持續3秒")] Stun,
        [Description("衝刺")] Dash,
        [Description("格檔")] Parry,
        [Description("死亡")] Dead,
        [Description("進入QTE事件")] UnderQteEvent,
    }

    /// 角色環境狀態。
    public enum SurroundingState
    {
        Air,
        Grounded
    }
}