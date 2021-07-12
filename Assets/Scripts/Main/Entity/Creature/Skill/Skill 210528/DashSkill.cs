using System;
using Main.Entity.Creature.Skill.Skill_210528;
using Main.Extension.Util;
using UnityEngine;
using static Main.Attribute.DictionaryAudioPlayer.Key;
using Math = System.Math;

namespace Main.Entity.Skill_210528
{
    //TODO 有時無法觸發
    public class DashSkill : AbstractEvent
    {
        [Tooltip("冷卻時間")] private const float CdTime = 1.5f;
        [Tooltip("等待使用者雙擊時長")] private const float WaitTime = 0.3f;
        private readonly SkillTemplate skillTemplate;
        private bool inited;
        private Vector2 force;

        private bool NotMove()
        {
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
        public DashSkill(AbstractCreature creature, float maxDuration = 0.15f,
            Action onEnter = null, Action onExit = null) :
            base(creature.MonoClass(), CdTime, maxDuration)
        {
            skillTemplate = new SkillTemplate(creature);

            causeEnter = () => inited && skillTemplate.GetCreatureAttr().MovableDyn; // 防呆。事件執行順序：SetForce->Enter
            causeExit = NotMove;
            // causeExit1 = new ComponentCollision<Transform>(creature.GetTransform(), 3f,LayerMask.GetMask("Ground"));

            this.onEnter = onEnter;
            this.onExit = onExit;
        }

        public void Invoke(Vector2 force)
        {
            if (state == State.Waiting) // 不一定執行成功
            {
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