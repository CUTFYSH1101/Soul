using System;
using Main.Event;
using Main.Extension.Util;
using Main.Util;
using UnityEngine;
using Object = UnityEngine.Object;
using Rigidbody2D = Main.Entity.Controller.Rigidbody2D;

namespace Main.Entity.Skill_210528
{
    public class KnockbackSkill : AbstractSkill
    {
        [Tooltip("等待下一次觸發時長")] private const float CdTime = 1;
        [Tooltip("效果時長")] private const float Duration = 0.5f;
        [Tooltip("是否被擊退？")] private bool knockedBack;
        private readonly Rigidbody2D rigidbody2D;
        private readonly CreatureAnimManager animator;
        private readonly ICause duration;

        private readonly Action enter, exit;
        // private Vector2 force;

        public KnockbackSkill(AbstractCreature abstractCreature,
            Action enter, Action exit) : base(abstractCreature.MonoClass(), CdTime)
        {
            duration = new CDCause(Duration);
            rigidbody2D = abstractCreature.GetRigidbody2D();
            this.enter = enter;
            this.exit = exit;

            animator = abstractCreature.GetAnimator();
        }

        // 使用擊退和特效
        public void Invoke(Vector2 direction, float force,
            Transform vfx = null, Vector2 position = default)
        {
            if (duration.Cause())
                duration.Reset();
            if (state == State.Waiting)
            {
                direction = direction.normalized;
                // direction = (direction + Vector2.down * direction.y).normalized; //調整方向
                // direction = direction.normalized + Vector2.down * direction.y; //調整方向
                rigidbody2D.AddForce_OnPassive(direction * force, ForceMode2D.Impulse);
                Invoke(vfx, direction, position);
                base.Invoke();
            }
        }

        // 使用擊退特效
        private void Invoke(Transform vfx = null, Vector2 direction = default, Vector2 position = default)
        {
            if (vfx.IsEmpty())
                return;
            Object.Instantiate(vfx, position, Quaternion.LookRotation(direction));
        }

        protected override void Enter()
        {
            knockedBack = true;
            enter?.Invoke();
            animator.Knockback(true);
        }

        protected override void Update()
        {
        }

        protected override void Exit()
        {
            knockedBack = false;
            exit?.Invoke();
            animator.Knockback(false);
            // rigidbody2D.StopForceX();
        }
    }
}