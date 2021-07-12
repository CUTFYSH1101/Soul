using System.ComponentModel;

namespace Main.Common
{
    public enum SkillName
    {
        [Description("類似null")] None,
        NormalAttack,
        JumpAttack,
        DiveAttack,
        KnockbackSkill,
        SpurAttack
    }

    public enum BuffType
    {
        [Description("更改數值")] SetState,
        [Description("阻礙行動")] SetValue,
        [Description("嘲諷")] Taunt,
        [Description("改變技能CD")] ChangSkillCD
    }

    public enum Buff
    {
        [Description("百分比按照時間回血。Healing over time")]
        HOT,
        [Description("嘲諷")] Taunt,
        [Description("免疫傷害")] DamageImmunity,
        [Description("兩倍傷害")] DoubleDamage
    }

    public enum DeBuff
    {
        None,
        [Description("百分比按照時間扣血。Damage over time")]
        DOT,
        [Description("震驚，持續0.5秒")] Shock,
        [Description("昏迷，持續3秒")] Stun,
        [Description("暈眩，眼冒金星，持續1秒")] Dizzy,
        [Description("僵直，持續0.5秒")]Stiff,// 僵直0.5秒 , 暈眩 1秒
        [Description("混亂")] Chaos,
        [Description("仇恨")] Hatred,
        [Description("使角色打不中對方")] Blind,
        [Description("使角色無法使用除了普通攻擊以外的技能")] SkillBlock
    }
}