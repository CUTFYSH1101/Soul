using Main.AnimAndAudioSystem.Main.Common;
using Main.Entity.Creature;
using Main.EventSystem.Common;
using Main.EventSystem.Event.CreatureEventSystem.Common;
using Main.EventSystem.Event.CreatureEventSystem.Decorator;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Attribute;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Common;

namespace Main.EventSystem.Event.CreatureEventSystem.Skill
{
    public class NormalAttack : AbstractCreatureEventC, ISkill
    {
        public SkillAttr SkillAttr { get; }



        public NormalAttack(AbstractCreature creature) :
            base(creature)
        {
            SkillAttr = new SkillAttr(EnumSkillTag.NormalAttack);
            // 等動畫播放完才可再次攻擊
            // 注意state裡面不可把狀態設為attack
            CauseEnter = new FuncCause(() =>
                !CreatureInterface.IsTag("Attack") && CreatureInterface.GetCreatureAttr().AttackableDyn);
            // 偵測動畫是否撥放完
            CauseExit = new FuncCause(() =>
                !CreatureInterface.IsTag("Attack")); // min duration

            PreWork += () =>
            {
                CreatureInterface.MindState = EnumMindState.Attack;
                CreatureInterface.CurrentSkill = EnumSkillTag.NormalAttack;
            };
            PostWork += () =>
            {
                CreatureInterface.MindState = default;
                CreatureInterface.CurrentSkill = default;
            };
            FinalEvent += CreatureInterface.GetAnimManager().Interrupt;
            InitCreatureEventOrder(EnumCreatureEventTag.NormalAttack, EnumOrder.Attack);
        }

        public void Invoke(EnumSymbol symbol)
        {
            if (State == EnumState.Free)
            {
                SkillAttr.Symbol = symbol;
                base.Invoke();
            }
        }

        protected override void Enter()
        {
            // 撥放音效
            CreatureInterface.Play(SkillAttr.Symbol);
            CreatureInterface.GetAnimManager().Attack(SkillAttr.Symbol);
        }

        protected override void Exit()
        {
        }
    }
}