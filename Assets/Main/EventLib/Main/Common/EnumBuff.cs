using System.ComponentModel;

namespace Main.EventLib.Common
{
    public enum EnumBuff
    {
        None,
        [Description("百分比按照時間回血。Healing over time")]
        Hot,
        [Description("嘲諷")] Taunt,
        [Description("免疫傷害")] DamageImmunity,
        [Description("兩倍傷害")] DoubleDamage
    }
}