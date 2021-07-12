using Main.Attribute;
using Main.Common;
using Main.Entity.Skill_210528;
using Main.Extension.Util;
using Main.Game.Collision;
using UnityEngine;

namespace Main.Entity.Creature.Skill.Skill_210528
{
    public class JumpAttack : AbstractSkill
    {
        [Tooltip("持續時間")] private const float Duration = 0.2f;
        [Tooltip("離地高度")] private const float GroundClearance = 2f;
        private readonly SkillTemplate skillTemplate;

        public JumpAttack(AbstractCreature creature, float cdTime = 0.1f) :
            base(creature.MonoClass(), cdTime, Duration)
        {
            skillTemplate = new SkillTemplate(creature);
            SkillAttr = new SkillAttr(SkillName.JumpAttack, Symbol.None,
                Duration, cdTime, () => new Vector2(skillTemplate.GetDirX * 0.7f, -0.7f)); // todo 先設為45度角
            
            // 在空中一定距離才能觸發攻擊
            causeEnter = () =>
                !skillTemplate.GetCreatureAttr().Grounded &&
                !skillTemplate.IsTag("Attack") &&
                skillTemplate.GetRigidbody2D().GroundClearance() > GroundClearance &&
                skillTemplate.GetCreatureAttr().AttackableDyn;
            // 偵測動畫是否撥放完
            causeExit = () =>
                !skillTemplate.IsTag("Attack");

            onEnter = () =>
            {
                skillTemplate.MindState = MindState.Attack;
                skillTemplate.SkillName = SkillName.JumpAttack;
            };
            onExit = () =>
            {
                skillTemplate.MindState = MindState.Idle;
                skillTemplate.SkillName = default;
            };
        }

        public void Invoke(Symbol symbol)
        {
            if (state == State.Waiting)
            {
                SkillAttr.Symbol = symbol;
                base.Invoke();
            }
        }

        protected override void Enter()
        {
            skillTemplate.GetAnimator().JumpAttack(SkillAttr.Symbol);
        }

        protected override void Update()
        {
        }

        protected override void Exit()
        {
        }
    }
}