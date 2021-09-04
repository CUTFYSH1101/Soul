using System.Collections.Generic;
using Main.Entity;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Attribute;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Common;

namespace Main.CreatureAndBehavior.Behavior
{
    public interface IBehavior : IData
    {
        Dictionary<EnumSkillTag, SkillAttr> SkillAttrDictionary { get; }
        bool ContainTag(EnumSkillTag tag);
        SkillAttr FindSkillAttrByTag(EnumSkillTag tag);

        // /// <param name="newSkill">內含有EnumSkillTag屬性</param>
        /// 注意新的資料tag不可與字典內容任一個重複
        /// <code>
        /// public void Append(SkillAttr newSkill){
        ///     if (SkillAttrDictionary.ContainsKey(newSkill.SkillTag)){
        ///         Debug.LogError("字典中已含有相同key值");
        ///         return;
        ///     }
        /// 
        ///     SkillAttrDictionary.Add(newSkill.SkillTag, newSkill);
        /// }
        /// </code>
        void AppendToSkillList(SkillAttr newSkill);
    }
}