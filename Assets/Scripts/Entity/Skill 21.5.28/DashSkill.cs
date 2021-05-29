using System;
using Extension.Entity.Controller;
using Main.Entity.Controller;
using Main.Util;
using Test2.Causes;
using Test2.Timers;
using UnityEngine;
using Rigidbody2D = Main.Entity.Controller.Rigidbody2D;

namespace Test2
{
    //TODO 有時無法觸發
    public class DashSkill : AbstractSkill
    {
        [Tooltip("等待下一次觸發時長")] private const float CdTime = 0.01f;
        [Tooltip("等待使用者雙擊時長")] private const float WaitTime = 0.3f;
        [Tooltip("效果時長")] private const float Duration = 0.5f;
        private static readonly int Dashing = Animator.StringToHash("Dashing");
        private readonly ICreature creature;
        private readonly Rigidbody2D rigidbody2D;
        private readonly ICause duration;
        private bool inited;
        private Vector2 force;

        private bool NotMove()
            => Math.Abs(rigidbody2D.Instance.velocity.x) <= .01f;

        private void SetForce(Vector2 value)
        {
            force = value;
            inited = true;
        }

        // dbClick -> onEnter
        // duration.IsTimeUp || math.abs rigidbody.velocity.x <= 0.1f -> onExit
        public DashSkill(ICreature creature, string key) :
            base(creature.GetTransform().GetOrAddComponent<MonoClass>(), CdTime)
        {
            duration = new CDCause(Duration);
            rigidbody2D = creature.GetRigidbody2D();
            causeEnter = () => inited; // 防呆。事件執行順序：SetForce->Enter
            causeExit = () => duration.Cause() || NotMove();
            this.creature = creature;
            // enterCause = new DBClick(key, WaitTime);
            // exitCause = new ComponentCollision<Transform>(creature.GetTransform(), 3f,LayerMask.GetMask("Ground"));
        }

        public void Invoke(Vector2 force)
        {
            if (duration.Cause()) 
                duration.Reset();
            if (state == State.Waiting)
            {
                SetForce(force);
                base.Invoke();
            }
        }

        protected override void Enter()
        {
            this.LogMethodName();
            inited = false;
            creature.GetRigidbody2D().AddForce(force);
            creature.GetAnimator().SetBool(Dashing, true);
            // 開啟動畫
        }

        protected override void Update()
        {
            this.LogMethodName();//TODO 有時不靈
        }

        protected override void Exit()
        {
            this.LogMethodName();
            this.LogLine();
            duration.Reset();
            creature.GetRigidbody2D().StopForceX();
            creature.GetAnimator().SetBool(Dashing, false);
            // 關閉動畫
        }
    }
}