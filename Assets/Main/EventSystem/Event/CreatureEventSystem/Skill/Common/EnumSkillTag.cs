using System.ComponentModel;

namespace Main.EventSystem.Event.CreatureEventSystem.Skill.Common
{
    /// 辨識用
    public enum EnumSkillTag
    {
        [Description("類似null")] None,
        NormalAttack,
        SpurAttack,
        JumpAttack,
        DiveAttack,
        Knockback
    }
}