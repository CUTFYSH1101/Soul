using Main.AnimAndAudioSystem.Main.Common;
using Main.Entity.Creature;
using Main.EventSystem.Common;
using Main.EventSystem.Event.CreatureEventSystem.Life;
using Main.EventSystem.Event.CreatureEventSystem.MoveEvent;
using Main.EventSystem.Event.CreatureEventSystem.Skill;
using Main.EventSystem.Event.UIEvent.QTE;
using Main.Input;
using Vector2 = UnityEngine.Vector2;

namespace Main.EventSystem.Event.CreatureEventSystem
{
    public class CreatureBehaviorInterface
    {
        private readonly (NormalAttack normalAttack, SpurAttack spurAttack, JumpAttack jumpAttack, DiveAttack diveAttack
            ) _skill;

        public (NormalAttack normalAttack, SpurAttack spurAttack, JumpAttack jumpAttack, DiveAttack diveAttack
            ) Skill => _skill;

        private readonly (MovingEvent movingEvent, DashEvent dashEvent, JumpOrWallJump jumpEvent) _move; // todo move to
        private readonly (ParryEvent parryEvent, AbstractQteSkill qteSkill) _other; // todo
        private readonly LifeEvent _life;

        public CreatureBehaviorInterface(Creature creature)
        {
            _skill.normalAttack = new NormalAttack(creature);
            _skill.spurAttack = new SpurAttack(creature);
            _skill.jumpAttack = new JumpAttack(creature);
            _skill.diveAttack = new DiveAttack(creature);
            _move.movingEvent = new MovingEvent(creature, HotkeySet.Horizontal);
            _move.dashEvent = new DashEvent(creature);
            var behavior = new BaseBehaviorInterface(creature);
            _move.jumpEvent = new JumpOrWallJump(creature).InitAction(behavior.Jump, behavior.WallJump);
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
            float normalAttack = 0,
            float spurAttack = 0,
            float jumpAttack = 0,
            float diveAttack = 0)
        {
            /*
            _skill.normalAttack.SkillAttr.CdTime = normalAttack;
            _skill.spurAttack.SkillAttr.CdTime = spurAttack;
            _skill.jumpAttack.SkillAttr.CdTime = jumpAttack;
            _skill.diveAttack.SkillAttr.CdTime = diveAttack;
            */
            _skill.normalAttack.CdTime = normalAttack;
            _skill.spurAttack.CdTime = spurAttack;
            _skill.jumpAttack.CdTime = jumpAttack;
            _skill.diveAttack.CdTime = diveAttack;
            return this;
        }

        public CreatureBehaviorInterface InitKnockback(
            float normalAttack = 0,
            float spurAttack = 0,
            float jumpAttack = 0,
            float diveAttack = 0)
        {
            /*
            _skill.normalAttack.SkillAttr.CdTime = normalAttack;
            _skill.spurAttack.SkillAttr.CdTime = spurAttack;
            _skill.jumpAttack.SkillAttr.CdTime = jumpAttack;
            _skill.diveAttack.SkillAttr.CdTime = diveAttack;
            */
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
        /// 兩種方向，1往右，-1往左
        /// <param name="dir">[-1,1]</param>
        public void Dash(int dir) => _move.dashEvent.Invoke(new Vector2(dir, 0));
        public void MoveUpdate() => _move.movingEvent.MoveUpdate();

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