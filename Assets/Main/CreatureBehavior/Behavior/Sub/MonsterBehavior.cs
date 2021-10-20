using System.Collections.Generic;
using Main.Entity;
using Main.EventLib.Sub.CreatureEvent;
using Main.EventLib.Sub.CreatureEvent.Skill.Attribute;
using Main.EventLib.Sub.CreatureEvent.Skill.Common;
using Main.Res.Script;
using UnityEngine;

namespace Main.CreatureBehavior.Behavior.Sub
{
    public class MonsterBehavior : IBehavior
    {
        private readonly CreatureBehaviorInterface _interface;
        public EnumDataTag Tag => EnumDataTag.Behavior;
        private readonly SkillDictionary _skillDictionary = new();
        public Dictionary<EnumSkillTag, SkillAttr> SkillAttrDictionary => 
            _skillDictionary.SkillAttrDictionary;

        // 希望趕快支持interface default！！
        public SkillAttr FindSkillAttrByTag(EnumSkillTag tag) =>
            _skillDictionary.FindSkillAttrByTag(tag);

        public void AppendToSkillList(SkillAttr newSkill) => 
            _skillDictionary.AppendToSkillList(newSkill);
        
        public MonsterBehavior(Entity.Creature.Creature creature)
        {
            _interface = new CreatureBehaviorInterface(creature)
                // .InitSkillCd(normalAttack: 2) // 每兩秒攻擊一次
                .InitMoveCd(moveTo: 0.5f); // 每兩秒更新一次目標位置

            AppendToSkillList(_interface.AtkSkill.normal.SkillAttr);
        }
        public void NormalAttack() => _interface.NormalAttack(EnumShape.Direct);

        public void MoveTo(Vector2 targetPos) => _interface.MoveTo(targetPos);
        public void StopMoveTo() => _interface.StopMoveTo();
    }
}