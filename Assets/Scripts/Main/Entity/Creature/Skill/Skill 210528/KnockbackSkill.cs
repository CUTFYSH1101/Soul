using JetBrains.Annotations;
using Main.Attribute;
using Main.Common;
using Main.Event;
using Main.Extension.Util;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Main.Entity.Skill_210528
{
    public class KnockbackSkill : AbstractSkill
    {
        [Tooltip("冷卻時間")] private const float CdTime = 0.1f;
        [Tooltip("持續時間")] private const float Duration = 0.5f;
        [Tooltip("推力持續時間")] private const float ForceDuration = 0.1f;
        private readonly SkillTemplate skillTemplate;
        private readonly CdCause forceDuration;
        // private readonly CdCause allDuration;
        private Vector2 force;
        private Transform vfx;
        private Vector2 position;

        public KnockbackSkill(AbstractCreature creature) : base(creature.MonoClass(), CdTime, Duration)
        {
            skillTemplate = new SkillTemplate(creature);
            SkillAttr = new SkillAttr(SkillName.KnockbackSkill, Symbol.None, Duration, CdTime, null!, 0); // todo 新增掉血特效
            forceDuration = new CdCause(ForceDuration);
            // allDuration = new CdCause(Duration);
            // causeEnter = () => skillTemplate.GetCreatureAttr().AttackableDyn;
            causeExit = () => forceDuration.Cause();
        }

        private void SubCreateVFX([NotNull] Transform vfx, Vector2 direction, Vector2 position = default)
        {
            if (vfx == null)
                return;

            if (position == default) // 預設為該生物的座標
                position = skillTemplate.GetCreature().GetPosition();

            Object.Instantiate(vfx, position, Quaternion.LookRotation(direction.normalized));
        }

        /// 使用擊退和特效
        public void Invoke(Vector2 direction, float force,
            [CanBeNull] Transform vfx, Vector2 offsetPos = default)
        {
            if (state == State.Waiting)
            {
                forceDuration.Reset();
                // allDuration.Reset();
                
                this.force = direction.normalized * force;
                this.position = skillTemplate.GetCreature().GetPosition() + offsetPos;// 預設為物件位置+偏移量
                this.vfx = vfx;
                base.Invoke();
            }
        }


        protected override void Enter()
        {
            // skillTemplate.MindState = MindState.Stiff;
            // skillTemplate.GetAnimator().Knockback(true);
            if (skillTemplate.GetCreatureAttr().MovableCoeff) // 可以移動之物體才可給予擊退
                skillTemplate.GetRigidbody2D().AddForce_OnPassive(force, ForceMode2D.Impulse);
            if (vfx != null) SubCreateVFX(vfx, force, position);// 用force代替direction
        }

        protected override void Update()
        {
            /*// 注意會重複呼叫
            if (forceDuration.Cause())
                skillTemplate.GetRigidbody2D().SetPassiveX(0);*/
        }

        protected override void Exit()
        {
            // skillTemplate.GetAnimator().Knockback(false);
            // skillTemplate.MindState = MindState.Idle;
            skillTemplate.GetRigidbody2D().SetPassiveX(0);
        }
    }
}