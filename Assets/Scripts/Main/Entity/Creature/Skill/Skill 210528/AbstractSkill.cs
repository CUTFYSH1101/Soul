using Main.Attribute;
using Main.Common;
using Main.Entity.Creature.Skill.Skill_210528;
using UnityEngine;

namespace Main.Entity.Skill_210528
{
    /// 包含Skill的功能，支持更多cause, if-else支持
    public abstract class AbstractSkill : AbstractEvent
    {
        /// 注意可能為空值
        public SkillAttr SkillAttr { get; protected set; } =
            new SkillAttr(SkillName.None, Symbol.None, 0, 0, default!, 0, default, null);

        protected AbstractSkill(MonoBehaviour mono, float cdTime = 0.1f, float maxDuration = 10f) :
            base(mono, cdTime, maxDuration)
        {
        }
    }
}