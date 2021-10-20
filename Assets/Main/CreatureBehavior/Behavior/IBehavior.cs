using System.Collections.Generic;
using Main.Entity;
using Main.EventLib.Sub.CreatureEvent.Skill.Attribute;
using Main.EventLib.Sub.CreatureEvent.Skill.Common;

namespace Main.CreatureBehavior.Behavior
{
    public interface IBehavior : IData
    {
        Dictionary<EnumSkillTag, SkillAttr> SkillAttrDictionary { get; }
        SkillAttr FindSkillAttrByTag(EnumSkillTag tag);

        #region MyRegion

        #endregion
    }
}