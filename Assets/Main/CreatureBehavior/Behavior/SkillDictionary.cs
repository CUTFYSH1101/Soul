using System.Collections.Generic;
using Main.Util;
using Main.Entity;
using Main.EventLib.Sub.CreatureEvent.Skill.Attribute;
using Main.EventLib.Sub.CreatureEvent.Skill.Common;

namespace Main.CreatureBehavior.Behavior
{
    public class SkillDictionary : IBehavior
    {
        public EnumDataTag Tag => EnumDataTag.Behavior;
        public Dictionary<EnumSkillTag, SkillAttr> SkillAttrDictionary { get; } =
            new();

        // 希望趕快支持interface default！！
        public SkillAttr FindSkillAttrByTag(EnumSkillTag tag)
        {
            if (SkillAttrDictionary.ContainsKey(tag))
                return SkillAttrDictionary[tag];

            return null;
        }

        public void AppendToSkillList(SkillAttr newSkill)
        {
            if (SkillAttrDictionary.ContainsKey(newSkill.SkillTag))
                "字典中已含有相同key值".LogErrorLine();

            SkillAttrDictionary.Add(newSkill.SkillTag, newSkill);
        }
    }
}