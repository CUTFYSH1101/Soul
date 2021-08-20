using System.ComponentModel;

namespace Main.EventSystem.Common
{
    public enum DeBuff
    {
        None,
        [Description("百分比按照時間扣血。Damage over time")]
        Dot,
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