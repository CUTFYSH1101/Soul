using System;
using Main.Attribute;
using Main.Common;
using Main.Event;
using Main.Extension.Util;
using UnityEngine;

namespace Main.Entity.Skill_210528
{
    public class SpurAttack : AbstractSkill
    {
        private const float CdTime = 1f;
        private readonly SkillAttr skillAttr;
        private readonly SkillTemplate skillTemplate; // 避免滑步太遠
        private readonly CDCause minDuration;

        public SpurAttack(AbstractCreature abstractCreature, Action onEnter, Action onExit) : base(abstractCreature.MonoClass(), CdTime)
        {
            skillAttr = new SkillAttr(Symbol.None, 0.2f, CdTime);
            skillTemplate = new SkillTemplate(abstractCreature, 0.2f);
            minDuration = new CDCause(.1f);

            // 等動畫播放完才可再次攻擊
            causeEnter = () => !skillTemplate.IsTag("Attack");
            // 偵測動畫是否撥放完
            causeExit = () => !skillTemplate.IsTag("Attack") && minDuration.Cause(); // min duration

            this.onEnter = () =>
            {
                skillTemplate.MindState = MindState.Attack;
                onEnter?.Invoke();
            };
            this.onExit = () =>
            {
                skillTemplate.MindState = MindState.Idle;
                onExit?.Invoke();
            };
        }

        public void Invoke(Symbol symbol)
        {
            if (state == State.Waiting)
            {
                skillTemplate.Reset();
                if (minDuration.Cause())
                    minDuration.Reset();
                skillTemplate.GetAnimator().SpurAttack(skillAttr.Symbol = symbol);
                base.Invoke();
            }
        }

        protected override void Enter()
        {
            var dir = new Vector2(skillTemplate.GetDirX, 0);
            skillTemplate.AddForce_OnActive(dir * 40, ForceMode2D.Impulse);
        }

        protected override void Update()
        {
            if (skillTemplate.DurationCause())
                skillTemplate.GetRigidbody2D().SetActiveX(0);
        }

        protected override void Exit()
        {
        }
    }
}