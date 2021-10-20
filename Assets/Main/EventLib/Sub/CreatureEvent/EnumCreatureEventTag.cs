using System.ComponentModel;

namespace Main.EventLib.Sub.CreatureEvent
{
    /// 辨識用
    public enum EnumCreatureEventTag
    {
        [Description("類似null")] None,
        AtkNormal,
        AtkSpur,
        AtkJump,
        AtkDive,
        Life,
        Knockback,
        Debuff,
        Dash,
        Move,
        Jump,
    }
}