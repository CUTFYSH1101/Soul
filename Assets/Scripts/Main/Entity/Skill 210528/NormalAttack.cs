using Main.Attribute;
using Main.Common;
using Main.Extension.Util;
using Main.Util.Timers;

namespace Main.Entity.Skill_210528
{
    public class NormalAttack : AbstractSkill
    {
        private readonly SkillAttr skillAttr;
        private readonly SkillTemplate skillTemplate;

        public NormalAttack(AbstractCreature abstractCreature, float cdTime = 0.2f, Stopwatch.Mode mode = Stopwatch.Mode.LocalGame) :
            base(abstractCreature.MonoClass(), cdTime, mode)
        {
            skillAttr = new SkillAttr(Symbol.None, .1f, cdTime);
            skillTemplate = new SkillTemplate(abstractCreature, .1f);
            // 等動畫播放完才可再次攻擊
            causeEnter = () => !skillTemplate.IsTag("Attack");
            // 偵測動畫是否撥放完
            causeExit = () => !skillTemplate.IsTag("Attack") && skillTemplate.DurationCause();// min duration

            onEnter = () => skillTemplate.MindState = MindState.Attack;
            onExit = () => skillTemplate.MindState = MindState.Idle;
        }

        public void Invoke(Symbol symbol)
        {
            if (state == State.Waiting)
            {
                skillTemplate.Reset();
                skillTemplate.GetAnimator().Attack(skillAttr.Symbol = symbol);
                base.Invoke();
            }
        }

        protected override void Enter()
        {
            // 撥放音效
            skillTemplate.Play(skillAttr.Symbol);
        }

        protected override void Update()
        {
        }

        protected override void Exit()
        {
        }
    }
}