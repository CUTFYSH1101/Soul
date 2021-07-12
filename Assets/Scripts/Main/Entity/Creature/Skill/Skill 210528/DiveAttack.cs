using Main.Attribute;
using Main.Common;
using Main.Entity.Skill_210613;
using Main.Event;
using Main.Extension.Util;
using Main.Game.Collision;
using UnityEngine;

namespace Main.Entity.Skill_210528
{
    /*public class DiveAttack : AbstractSkill
    {
        [Tooltip("冷卻時間")] private const float CdTime = 0.2f;
        [Tooltip("持續時間")] private const float Duration = .5f;
        private readonly SkillTemplate skillTemplate;
        private const float Height = 1.5f;

        private bool MoreThenGroundClearance =>
            skillTemplate.GetCreature().GetTransform().GroundClearance() > Height;

        public DiveAttack(AbstractCreature creature) :
            base(creature.MonoClass(), CdTime, Duration)
        {
            skillTemplate = new SkillTemplate(creature);
            SkillAttr = new SkillAttr(SkillName.DiveAttack, Symbol.None,
                .5f, CdTime, () => new Vector2(skillTemplate.GetDirX, 0), 
                skillTemplate.GetCreatureAttr().DiveForce); //(1,0),(-1,0)
            SkillAttr.DeBuffBuff = DeBuff.Dizzy; // 帶有暈眩效果
            var attr = skillTemplate.GetCreatureAttr();

            causeEnter = () =>
                !skillTemplate.IsTag("Attack") &&
                !attr.Grounded && MoreThenGroundClearance && // mindState = 空中時，允許俯衝
                attr.MovableDyn;
            causeExit = () => attr.Grounded;

            onEnter = () =>
            {
                skillTemplate.MindState = MindState.Attack;
                skillTemplate.SkillName = SkillAttr.SkillName;
            };
            onExit = () =>
            {
                skillTemplate.MindState = MindState.Idle;
                skillTemplate.SkillName = SkillName.None;
            }; // 注意exit是否from knockback
        }

        public void Invoke(Symbol symbol)
        {
            // Debug.Log(creature.GetAnimator().HasParameter(UnityAnimID.DiveAttacking));// 注意不要呼叫anim中沒有的參數
            // 等動畫播放完才可再次攻擊
            if (state == State.Waiting)
            {
                SkillAttr.Symbol = symbol;
                base.Invoke();
            }
            // skillTemplate.GetCreature().GetTransform().GroundClearance().LogLine();
        }

        protected override void Enter()
        {
            Vector2 dir = new Vector2(skillTemplate.GetDirX * 0.7f, -0.7f).normalized;
            // 切換動畫
            skillTemplate.GetAnimator().DiveAttack(SkillAttr.Symbol, true); // 注意需要先更改skillAttr的數值，才有辦法呼叫
            // 添加力
            skillTemplate.AddForce_OnActive(dir * skillTemplate.GetCreatureAttr().DiveForce, ForceMode2D.Impulse);
        }

        protected override void Update()
        {
        }

        protected override void Exit()
        {
            // 切換動畫
            skillTemplate.GetAnimator().DiveAttack(SkillAttr.Symbol, false);
        }
    }*/

    public class DiveAttack : AbstractSkillWrapper
    {
        [Tooltip("冷卻時間")] private const float CdTime = 0.2f;
        [Tooltip("持續時間")] private const float Duration = .5f;
        private readonly SkillTemplate skillTemplate;
        private const float Height = 1.5f;
        private readonly CdCause totalDuration;

        private bool MoreThenGroundClearance =>
            skillTemplate.GetCreature().GetTransform().GroundClearance() > Height;

        public DiveAttack(AbstractCreature creature, float cdTime = 0.2f) :
            base(creature.MonoClass(), cdTime)
        {
            skillTemplate = new SkillTemplate(creature);
            SkillAttr = new SkillAttr(SkillName.DiveAttack, Symbol.None,
                .5f, CdTime, () => new Vector2(skillTemplate.GetDirX, 0),
                skillTemplate.GetCreatureAttr().DiveForce*0.5f); //避免飛太遠 todo 根據對方相對位置
            SkillAttr.DeBuffBuff = DeBuff.Dizzy; // 帶有暈眩效果
            var attr = skillTemplate.GetCreatureAttr();
            totalDuration = new CdCause(Duration);

            CauseEnter.cause = () =>
                !skillTemplate.IsTag("Attack") &&
                !attr.Grounded && MoreThenGroundClearance && // mindState = 空中時，允許俯衝
                attr.MovableDyn;
            CauseToAction2.cause = () => attr.Grounded;
            CauseToAction3.cause = () => !skillTemplate.IsTag("Attack");
            // CauseInterrupt.cause = () => attr.Grounded;
        }

        public void Invoke(Symbol symbol)
        {
            // Debug.Log(creature.GetAnimator().HasParameter(UnityAnimID.DiveAttacking));
            if (state == State.Waiting)
            {
                SkillAttr.Symbol = symbol;
                // skillAttr.Symbol = Direct;
                totalDuration.Reset();
                base.Invoke();
            }
        }

        // Crash
        protected override void EnterAction1()
        {
            skillTemplate.MindState = MindState.Attack;
            skillTemplate.SkillName = SkillAttr.SkillName;

            Vector2 dir = new Vector2(skillTemplate.GetDirX * 0.7f, -0.7f).normalized;
            // 切換動畫
            skillTemplate.GetAnimator().DiveAttack(SkillAttr.Symbol, true); // 注意需要先更改skillAttr的數值，才有辦法呼叫
            // 避免衝刺錯誤 todo
            skillTemplate.GetRigidbody2D().instance.gravityScale = 1;
            skillTemplate.GetRigidbody2D().SetActiveX(0);
            // 添加力
            skillTemplate.AddForce_OnActive(dir * skillTemplate.GetCreatureAttr().DiveForce, ForceMode2D.Impulse);
        }

        // Landing
        protected override void Action2()
        {
            // 切換動畫
            skillTemplate.GetAnimator().DiveAttack(SkillAttr.Symbol, false);
        }

        // Idle
        protected override void Action3()
        {
            skillTemplate.MindState = MindState.Idle;
            skillTemplate.SkillName = SkillName.None;
            // 注意exit是否from knockback
        }

        protected override void ExitAction4()
        {
        }
    }
}