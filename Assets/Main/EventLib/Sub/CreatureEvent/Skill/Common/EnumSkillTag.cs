using System.ComponentModel;

namespace Main.EventLib.Sub.CreatureEvent.Skill.Common
{
    /// 辨識用
    public enum EnumSkillTag
    {
        [Description("類似null")] None,
        AtkNormal,
        AtkSpur,
        AtkJump,
        AtkDive,
        AttackQte,
        Knockback
    }
}