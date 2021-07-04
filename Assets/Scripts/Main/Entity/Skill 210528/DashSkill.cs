using System;
using Main.Extension.Util;
using UnityEngine;
using static Main.Attribute.DictionaryAudioPlayer.Key;
using Math = System.Math;

namespace Main.Entity.Skill_210528
{
    //TODO 有時無法觸發
    public class DashSkill : AbstractSkill
    {
        [Tooltip("等待下一次觸發時長")] private const float CdTime = 1.5f;
        [Tooltip("等待使用者雙擊時長")] private const float WaitTime = 0.3f;
        private readonly SkillTemplate skillTemplate;
        private readonly AbstractCreature abstractCreature;
        private bool inited;
        private Vector2 force;

        private bool NotMove()
        {
            // TODO ERROR
            if (!inited) return false; // 避免一開始就使條件成立
            return Math.Abs(skillTemplate.GetRigidbody2D().GetActiveX()) <= .1f;
        }

        private void SetForce(Vector2 value)
        {
            force = value;
            inited = true;
        }

        // dbClick -> onEnter
        // CauseDuration.IsTimeUp || math.abs rigidbody.velocity.x <= 0.1f -> onExit
        public DashSkill(AbstractCreature abstractCreature, float duration = 0.15f,
            Action onEnter = null, Action onExit = null) :
            base(abstractCreature.MonoClass(), CdTime)
        {
            skillTemplate = new SkillTemplate(abstractCreature, duration);

            causeEnter = () => inited; // 防呆。事件執行順序：SetForce->Enter
            causeExit = () => skillTemplate.DurationCause() || NotMove();
            this.abstractCreature = abstractCreature;
            // causeEnter1 = new DBClick(key, WaitTime);
            // causeExit1 = new ComponentCollision<Transform>(abstractCreature.GetTransform(), 3f,LayerMask.GetMask("Ground"));

            this.onEnter = onEnter;
            this.onExit = onExit;
        }

        public void Invoke(Vector2 force)
        {
            if (state == State.Waiting) // 不一定執行成功
            {
                skillTemplate.Reset();
                SetForce(force);
                base.Invoke();
            }
        }

        protected override void Enter()
        {
            inited = false;
            // 開啟動畫
            skillTemplate.GetAnimator().Dash(true);
            skillTemplate.AddForce_OnActive(force, ForceMode2D.Impulse);
            skillTemplate.GetRigidbody2D().instance.gravityScale = 0;
            skillTemplate.Play(Dash);
        }

        private Vector2 fixPos;

        protected override void Update()
        {
            skillTemplate.GetRigidbody2D().instance.velocity =
                new Vector2(skillTemplate.GetRigidbody2D().Velocity.x, 0); // 強制霸體
        }

        protected override void Exit()
        {
            // 避免在空中會繼續往前移動
            skillTemplate.GetRigidbody2D().SetActiveX(0);
            skillTemplate.GetRigidbody2D().instance.gravityScale = 1;
            // 關閉動畫
            skillTemplate.GetAnimator().Dash(false);
        }
    }
}