using System;
using Main.Common;
using Main.Entity.Controller;
using Main.Util;
using Test2.Causes;
using Main.Util.Timers;
using UnityEngine;
using Rigidbody2D = Main.Entity.Controller.Rigidbody2D;

namespace Test2
{
    //TODO 有時無法觸發
    public class DashSkill : AbstractSkill
    {
        [Tooltip("等待下一次觸發時長")] private const float CdTime = 0.01f;
        [Tooltip("等待使用者雙擊時長")] private const float WaitTime = 0.3f;
        private static readonly int Dashing = Animator.StringToHash("Dashing");
        private readonly ICreature creature;
        private readonly Rigidbody2D rigidbody2D;
        private readonly Animator animator;
        private readonly ICause CauseDuration;
        private bool inited;
        private Vector2 force;

        private bool NotMove()
        {
            // TODO ERROR
            if (!inited)
            {
                return false; // 避免一開始就使條件成立
            }

            return Math.Abs(rigidbody2D.GetActiveX()) <= .1f;
        }

        private void SetForce(Vector2 value)
        {
            force = value;
            inited = true;
        }

        // dbClick -> onEnter
        // CauseDuration.IsTimeUp || math.abs rigidbody.velocity.x <= 0.1f -> onExit
        public DashSkill(ICreature creature, string key, float duration = 0.25f,
            Action onEnter = null, Action onExit = null) :
            base(creature.GetTransform().GetOrAddComponent<MonoClass>(), CdTime)
        {
            CauseDuration = new CDCause(duration);
            rigidbody2D = creature.GetRigidbody2D();
            animator = creature.GetAnimator();
            causeEnter = () => inited; // 防呆。事件執行順序：SetForce->Enter
            causeExit = () => CauseDuration.Cause() || NotMove();
            this.creature = creature;
            // causeEnter1 = new DBClick(key, WaitTime);
            // causeExit1 = new ComponentCollision<Transform>(creature.GetTransform(), 3f,LayerMask.GetMask("Ground"));

            this.onEnter = onEnter;
            this.onExit = onExit;
        }

        public void Invoke(Vector2 force)
        {
            if (CauseDuration.Cause())
                CauseDuration.Reset();
            
            if (state == State.Waiting)
            {
                SetForce(force);
                base.Invoke();
            }
        }

        protected override void Enter()
        {
            inited = false;
            rigidbody2D.AddForce_OnActive(force, ForceMode2D.Impulse);
            // creature.GetRigidbody2D().AddForce_OnActive(force);
            animator.SetBool(Dashing, true);
            // 開啟動畫
        }

        protected override void Update()
        {
        }

        protected override void Exit()
        {
            CauseDuration.Reset();
            // creature.GetRigidbody2D().StopForceX();
            animator.SetBool(Dashing, false);
            // 關閉動畫
        }
    }
}