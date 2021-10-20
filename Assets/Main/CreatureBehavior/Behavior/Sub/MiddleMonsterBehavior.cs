using System.Collections.Generic;
using Main.Entity;
using Main.Entity.Creature;
using Main.EventLib.Sub.CreatureEvent.Skill.Attribute;
using Main.EventLib.Sub.CreatureEvent.Skill.Common;
using Main.EventLib.Sub.UIEvent.QTE;
using UnityEngine;

namespace Main.CreatureBehavior.Behavior.Sub
{
    public class MiddleMonsterBehavior : IBehavior
    {
        private readonly MonsterBehavior _decoratedBehavior;
        public EnumDataTag Tag => EnumDataTag.Behavior;

        private readonly SkillDictionary _skillDictionary = new();

        public Dictionary<EnumSkillTag, SkillAttr> SkillAttrDictionary =>
            _skillDictionary.SkillAttrDictionary;

        // 希望趕快支持interface default！！
        public SkillAttr FindSkillAttrByTag(EnumSkillTag tag) =>
            _skillDictionary.FindSkillAttrByTag(tag);

        public void AppendToSkillList(SkillAttr newSkill) =>
            _skillDictionary.AppendToSkillList(newSkill);

        public MiddleMonsterBehavior(Creature creature)
        {
            _decoratedBehavior = new MonsterBehavior(creature);
            _qteSkill = new QteSkill.Builder(creature)
                .InitSkillAttr(EnumSkillTag.AttackQte)
                .InitAttr()
                .InitVFXEvent(_decoratedBehavior.NormalAttack)
                .GetSkill();

            AppendToSkillList(_decoratedBehavior.FindSkillAttrByTag(EnumSkillTag.AtkNormal)); // normal attack
            AppendToSkillList(_qteSkill.SkillAttr); // qte
        }

        public void NormalAttack() => _decoratedBehavior.NormalAttack();
        private readonly QteSkill _qteSkill;

        public void QteAttack(Creature target)
        {
            if (target == null) return;
            var random = Util.Enum.Random(EnumQteShape.Square, EnumQteShape.Circle);
            _qteSkill.Execute(target, random);
        }
        
        public void MoveTo(Vector2 targetPos) => _decoratedBehavior.MoveTo(targetPos);
        public void StopMoveTo() => _decoratedBehavior.StopMoveTo();
    }
}