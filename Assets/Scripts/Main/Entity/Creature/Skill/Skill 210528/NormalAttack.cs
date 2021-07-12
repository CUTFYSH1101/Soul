using Main.Attribute;
using Main.Common;
using Main.Extension.Util;
using UnityEngine;

namespace Main.Entity.Skill_210528
{
    public class NormalAttack : AbstractSkill
    {
        private readonly SkillTemplate skillTemplate;

        public NormalAttack(AbstractCreature abstractCreature, float cdTime = 0.2f) :
            base(abstractCreature.MonoClass(), cdTime)
        {
            skillTemplate = new SkillTemplate(abstractCreature);
            SkillAttr = new SkillAttr(SkillName.NormalAttack, Symbol.None,
                .1f, cdTime, () => Vector2.zero, 0);
            // 等動畫播放完才可再次攻擊
            // 注意state裡面不可把狀態設為attack，
            causeEnter = () => !skillTemplate.IsTag("Attack") && skillTemplate.GetCreatureAttr().AttackableDyn;
            // 偵測動畫是否撥放完
            causeExit = () => !skillTemplate.IsTag("Attack"); // min duration

            onEnter = () =>
            {
                skillTemplate.MindState = MindState.Attack;
                skillTemplate.SkillName = SkillName.NormalAttack;
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
            // 撥放音效
            skillTemplate.Play(SkillAttr.Symbol);
            skillTemplate.GetAnimator().Attack(SkillAttr.Symbol);
        }

        protected override void Update()
        {
        }

        protected override void Exit()
        {
        }
    }
}