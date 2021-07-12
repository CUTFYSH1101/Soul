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
        [Tooltip("冷卻時間")] private const float CdTime = 0.1f;
        [Tooltip("受力持續時間")] private const float ForceTime = 0.1f;
        [Tooltip("擊退力大小")] private const float Force = 20;
        private readonly SkillTemplate skillTemplate; // 避免滑步太遠
        private readonly CdCause forceDuration;

        public SpurAttack(AbstractCreature creature, Action onEnter, Action onExit) : base(creature.MonoClass(), CdTime)
        {
            skillTemplate = new SkillTemplate(creature);
            SkillAttr = new SkillAttr(SkillName.SpurAttack, Symbol.None,
                .1f, CdTime, () => new Vector2(skillTemplate.GetDirX, 0), Force);
            forceDuration = new CdCause(ForceTime);

            // 等動畫播放完才可再次攻擊
            causeEnter = () => !skillTemplate.IsTag("Attack") && skillTemplate.GetCreatureAttr().MovableDyn;
            // 偵測動畫是否撥放完
            causeExit = () => !skillTemplate.IsTag("Attack"); // min duration

            this.onEnter = () =>
            {
                onEnter?.Invoke(); // 注意次序，避免moveController誤設為idle
                skillTemplate.MindState = MindState.Attack;
                skillTemplate.SkillName = SkillName.SpurAttack;
            };
            this.onExit = () =>
            {
                skillTemplate.MindState = MindState.Idle;
                skillTemplate.SkillName = SkillName.None;
                onExit?.Invoke();
            };
        }

        public void Invoke(Symbol symbol)
        {
            if (state == State.Waiting)
            {
                forceDuration.Reset();
                SkillAttr.Symbol = symbol;
                base.Invoke();
            }
        }

        protected override void Enter()
        {
            var dir = new Vector2(skillTemplate.GetDirX, 0);
            skillTemplate.GetAnimator().SpurAttack(SkillAttr.Symbol);
            skillTemplate.AddForce_OnActive(dir * SkillAttr.Knockback, ForceMode2D.Impulse);
        }

        protected override void Update()
        {
            // 注意會重複呼叫
            if (forceDuration.Cause())
                skillTemplate.GetRigidbody2D().SetActiveX(0);
        }

        protected override void Exit()
        {
        }
    }
}