using System.Collections.Generic;
using Main.Entity;
using Main.Entity.Creature;
using Main.EventLib.Sub.CreatureEvent.MoveEvent;
using Main.EventLib.Sub.CreatureEvent.Skill.Attribute;
using Main.EventLib.Sub.CreatureEvent.Skill.Common;
using UnityEngine;

namespace Main.CreatureBehavior.Behavior.Sub
{
    public class FlyMonsterBehavior : IBehavior
    {
        private readonly SkillDictionary _skillDictionary = new();
        public EnumDataTag Tag => EnumDataTag.Behavior;

        public Dictionary<EnumSkillTag, SkillAttr> SkillAttrDictionary =>
            _skillDictionary.SkillAttrDictionary;

        public SkillAttr FindSkillAttrByTag(EnumSkillTag tag) =>
            _skillDictionary.FindSkillAttrByTag(tag);

        public void AppendToSkillList(SkillAttr newSkill) =>
            _skillDictionary.AppendToSkillList(newSkill);

        public FlyMonsterBehavior(Creature creature)
        {
            _flyToEvent = new FlyToEvent(creature);
            _monsterBehavior = new MonsterBehavior(creature);

            AppendToSkillList(_monsterBehavior.FindSkillAttrByTag(EnumSkillTag.AtkNormal)); // 都是兩秒間隔
        }

        private readonly FlyToEvent _flyToEvent;
        public void FlyTo(Vector2 target) => _flyToEvent.Invoke(target);

        private readonly MonsterBehavior _monsterBehavior;
        public void NormalAttack() => _monsterBehavior.NormalAttack();
    }
}