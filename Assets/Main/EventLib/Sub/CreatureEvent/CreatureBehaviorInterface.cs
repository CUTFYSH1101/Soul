using Main.Entity.Creature;
using Main.EventLib.Common;
using Main.EventLib.Main.EventSystem.Main.Interface;
using Main.EventLib.Sub.CreatureEvent.Life;
using Main.EventLib.Sub.CreatureEvent.MoveEvent;
using Main.EventLib.Sub.CreatureEvent.Skill;
using Main.EventLib.Sub.CreatureEvent.StateEvent;
using Main.EventLib.Sub.UIEvent.QTE;
using Main.Input;
using Main.Res.Script;
using Vector2 = UnityEngine.Vector2;

namespace Main.EventLib.Sub.CreatureEvent
{
    public class CreatureBehaviorInterface
    {
        private readonly (AtkNormal normal, AtkSpur spur, AtkJump jump, AtkDive dive) _atkSkill;
        public (AtkNormal normal, AtkSpur spur, AtkJump jump, AtkDive dive) AtkSkill => _atkSkill;

        private readonly (MovingEvent moving, MoveToEvent moveTo, DashEvent dash, JumpOrWallJump jump) _moveEvent;

        private readonly (object parryEvent, QteSkill qteSkill) _qte; // todo
        private readonly LifeEvent _life;

        public CreatureBehaviorInterface(Creature creature)
        {
            _atkSkill.normal = new AtkNormal(creature);
            _atkSkill.spur = new AtkSpur(creature);
            _atkSkill.jump = new AtkJump(creature);
            _atkSkill.dive = new AtkDive(creature);
            _moveEvent.moving = new MovingEvent(creature, HotkeySet.Horizontal);
            _moveEvent.moveTo = new MoveToEvent(creature);
            _moveEvent.dash = new DashEvent(creature);
            var behavior = new BaseBehaviorInterface(creature);
            _moveEvent.jump = new JumpOrWallJump(creature).InitAction(behavior.Jump, behavior.WallJump);
            _life = new LifeEvent(creature);
        }
        public CreatureBehaviorInterface SetSkillDebuff(
            EnumDebuff normalAttack,
            EnumDebuff spurAttack,
            EnumDebuff jumpAttack,
            EnumDebuff diveAttack = EnumDebuff.Dizzy)
        {
            _atkSkill.normal.SetDebuff(normalAttack);
            _atkSkill.spur.SetDebuff(spurAttack);
            _atkSkill.jump.SetDebuff(jumpAttack);
            _atkSkill.dive.SetDebuff(diveAttack);
            return this;
        }
        public CreatureBehaviorInterface SetSkillCd(
            float normalAttack = 0,
            float spurAttack = 0,
            float jumpAttack = 0,
            float diveAttack = 0)
        {
            _atkSkill.normal.SetCd(normalAttack);
            _atkSkill.spur.SetCd(spurAttack);
            _atkSkill.jump.SetCd(jumpAttack);
            _atkSkill.dive.SetCd(diveAttack);
            return this;
        }
        public CreatureBehaviorInterface InitMoveCd(
            float moving = 0,
            float jump = 0,
            float moveTo = 0.2f,
            float dash = 1.5f)
        {
            _moveEvent.moving.SetCd(moving);
            _moveEvent.moveTo.SetCd(moveTo);
            _moveEvent.jump.CdTime = jump;
            _moveEvent.dash.SetCd(dash);
            return this;
        }

        public CreatureBehaviorInterface SetSkillKnockback(
            KnockbackAttr normalAttack = default,
            KnockbackAttr spurAttack = default,
            KnockbackAttr jumpAttack = default,
            KnockbackAttr diveAttack = default)
        {
            _atkSkill.normal.SetKnockback(normalAttack);
            _atkSkill.spur.SetKnockback(spurAttack);
            _atkSkill.jump.SetKnockback(jumpAttack);
            _atkSkill.dive.SetKnockback(diveAttack);
            return this;
        }

        public void Killed() => _life.Killed();

        public void Revival() => _life.Revival();

        /// wallJump + jump
        public void JumpOrWallJump() => _moveEvent.jump.Invoke();

        /// 兩種方向，1往右，-1往左
        /// <param name="dir">[-1,1]</param>
        public void Dash(int dir) => _moveEvent.dash.Invoke(new Vector2(dir, 0));

        /*public void Dash2(int force, float duration = 0.15f)
        {
            _move.dashEvent.SetDash(Math.Abs(force), duration);
            _move.dashEvent.Invoke(new Vector2(Math.Sign(force), 0));
        }*/

        public void MoveUpdate() => _moveEvent.moving.MoveUpdate();

        public void MoveTo(Vector2 targetPos) => _moveEvent.moveTo.Invoke(targetPos);
        public void StopMoveTo() => _moveEvent.moveTo.IsOpen = false;
        public void NormalAttack(EnumShape shape) => _atkSkill.normal.Execute(shape); // 玩家專屬normalAttack、音效
        public void SpurAttack(EnumShape shape) => _atkSkill.spur.Execute(shape);
        public void JumpAttack(EnumShape shape) => _atkSkill.jump.Execute(shape);
        public void DiveAttack(EnumShape shape) => _atkSkill.dive.Execute(shape);
        public void InvokeParryEvent()
        {
        }
    }
}