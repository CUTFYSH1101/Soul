using Main.EventLib.Common;
using Main.EventLib.Sub.CreatureEvent.Skill.Attribute;
using Main.EventLib.Sub.CreatureEvent.StateEvent;

namespace Main.EventLib.Sub.CreatureEvent.Skill
{
    /// 包含Skill的功能，支持更多cause, if-else支持
    public interface ISkill
    {
        /// 注意可能為空值
        SkillAttr SkillAttr { get; set; }

        EnumDebuff Debuff
        {
            get => SkillAttr.Debuff;
            set => SkillAttr.Debuff = value;
        }

        KnockbackAttr Knockback
        {
            get => SkillAttr.Knockback;
            set => SkillAttr.Knockback = value;
        }
    }

    public static class SkillExtensions
    {
        public static void SetDebuff(this ISkill skill, EnumDebuff attr) =>
            skill.Debuff = attr;

        public static EnumDebuff GetDebuff(this ISkill skill) =>
            skill.Debuff;
        
        public static void SetKnockback(this ISkill skill, KnockbackAttr attr) =>
            skill.Knockback = attr;

        public static KnockbackAttr GetKnockback(this ISkill skill) =>
            skill.Knockback;
    }
}
/*
 * invoke skill時
 * 呼叫cameraShaking
 * 呼叫特定類啟用畫面icon cd時間
*/