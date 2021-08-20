using System;
using System.Numerics;
using JetBrains.Annotations;
using Main.AnimAndAudioSystem.Main.Common;
using Main.Entity;
using Main.Entity.Creature;
using Main.EventSystem.Cause;
using Main.EventSystem.Common;
using Main.EventSystem.Event.CreatureEventSystem.Life;
using Main.EventSystem.Event.CreatureEventSystem.MoveEvent;
using Main.EventSystem.Event.CreatureEventSystem.Skill;
using Main.EventSystem.Event.UIEvent.QTE;
using Main.Input;

namespace Main.EventSystem.Event.CreatureEventSystem
{
    public class CreatureBehaviorInterface
    {
        private readonly (NormalAttack normalAttack, SpurAttack spurAttack, JumpAttack jumpAttack, DiveAttack diveAttack
            ) _skill;

        private readonly (MovingEvent movingEvent, DashEvent dashEvent, JumpOrWallJump jumpEvent) _move; // todo move to
        private readonly (ParryEvent parryEvent, AbstractQteSkill qteSkill) _other; // todo
        private readonly LifeEvent _life;

        public CreatureBehaviorInterface(AbstractCreature creature)
        {
            _skill.normalAttack = new NormalAttack(creature);
            _skill.spurAttack = new SpurAttack(creature);
            _skill.jumpAttack = new JumpAttack(creature);
            _skill.diveAttack = new DiveAttack(creature);
            _move.movingEvent = new MovingEvent(creature, HotkeySet.Horizontal);
            _move.dashEvent = new DashEvent(creature);
            _move.jumpEvent = new JumpOrWallJump(creature);
            _life = new LifeEvent(creature);
        }

        public CreatureBehaviorInterface InitSkillDeBuff(
            DeBuff normalAttack,
            DeBuff spurAttack,
            DeBuff jumpAttack,
            DeBuff diveAttack = DeBuff.Dizzy)
        {
            _skill.normalAttack.SkillAttr.DeBuff = normalAttack;
            _skill.spurAttack.SkillAttr.DeBuff = spurAttack;
            _skill.jumpAttack.SkillAttr.DeBuff = jumpAttack;
            _skill.diveAttack.SkillAttr.DeBuff = diveAttack;
            return this;
        }

        public CreatureBehaviorInterface InitSkillCd(
            float normalAttack,
            float spurAttack,
            float jumpAttack,
            float diveAttack)
        {
            _skill.normalAttack.SkillAttr.CdTime = normalAttack;
            _skill.spurAttack.SkillAttr.CdTime = spurAttack;
            _skill.jumpAttack.SkillAttr.CdTime = jumpAttack;
            _skill.diveAttack.SkillAttr.CdTime = diveAttack;
            _skill.normalAttack.CdTime = normalAttack;
            _skill.spurAttack.CdTime = spurAttack;
            _skill.jumpAttack.CdTime = jumpAttack;
            _skill.diveAttack.CdTime = diveAttack;
            return this;
        }

        public void Killed() => _life.Killed();

        public void Revival() => _life.Revival();

        /// wallJump + jump
        public void JumpOrWallJump() => _move.jumpEvent.Invoke();

        public void Move() => _move.movingEvent.MoveUpdate();

        // public void MoveTo(Vector2 targetPos) => moveEvent.MoveTo(targetPos);
        public void NormalAttack(EnumSymbol symbol) => _skill.normalAttack.Invoke(symbol); // 玩家專屬normalAttack、音效
        public void SpurAttack(EnumSymbol symbol) => _skill.spurAttack.Invoke(symbol);
        public void JumpAttack(EnumSymbol symbol) => _skill.jumpAttack.Invoke(symbol);
        public void DiveAttack(EnumSymbol symbol) => _skill.diveAttack.Invoke(symbol);

        public void InvokeQTEEvent(EnumQteSymbol symbol)
        {
        }

        public void InvokeParryEvent()
        {
        }
    }
}