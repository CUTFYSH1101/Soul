using System;
using System.Diagnostics.CodeAnalysis;
using Main.Blood;
using Main.EventLib.Main.EventSystem.Main;
using Main.Entity.Creature;
using Main.EventLib.Common;
using Main.EventLib.Condition;
using Main.EventLib.Main.EventSystem;
using Main.EventLib.Main.EventSystem.EventOrderbySystem;
using Main.EventLib.Main.EventSystem.Main.Interface;
using Main.EventLib.Main.EventSystem.Main.Sub.Interface;
using Main.EventLib.Sub.CreatureEvent.Skill.Attribute;
using Main.EventLib.Sub.CreatureEvent.Skill.Common;
using Main.Res.Script;
using Main.Util;

namespace Main.EventLib.Sub.CreatureEvent.Skill
{
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class AtkNormal : AbsEventObject, IEvent2, ISkill, IWorkOnCreature
    {
        private CdCondition _min = new(0.2f, Stopwatch.Mode.RealWorld);

        public AtkNormal(Creature creature)
        {
            this.Build(creature, EnumOrder.Attack, EnumCreatureEventTag.AtkNormal); // 搭配IWorkOnCreature, 
            SkillAttr = new SkillAttr(EnumSkillTag.AtkNormal);
            // CreatureInterfaceForSkill = new CreatureInterfaceForSkill(creature);
            
            PreWork += () =>
            {
                CreatureInterface.MindState = EnumMindState.Attacking;
                CreatureInterface.CurrentSkill = EnumSkillTag.AtkNormal;
            };
            PostWork += () =>
            {
                CreatureInterface.MindState = default;
                CreatureInterface.CurrentSkill = default;
            };
            // 等動畫播放完才可再次攻擊
            // 注意state裡面不可把狀態設為attack
            FilterIn = () =>
                !CreatureInterface.IsTag("Attack") &&
                CreatureInterface.GetAttr().EnableAttackDyn;
            // 偵測動畫是否撥放完
            ToInterrupt = () => !CreatureInterface.IsTag("Attack") && _min.AndCause(); // min duration
            FinalAct += CreatureInterface.GetAnim().Interrupt; // 搭配interruptable
        }

        public void Execute(BloodType shape)
        {
            if (State == EnumState.Free)
            {
                SkillAttr.BloodType = shape;
                _min.Reset();
                Director.CreateEvent();
            }
        }

        public void Enter()
        {
            // 撥放音效
            CreatureInterface.Play(SkillAttr.BloodType);
            CreatureInterface.GetAnim().Interrupt();
            CreatureInterface.GetAnim().AtkNormal(SkillAttr.BloodType);
        }

        public void Exit()
        {
        }

        public Func<bool> FilterIn { get; }
        public Func<bool> ToInterrupt { get; }
        public SkillAttr SkillAttr { get; set; }
        public CreatureInterface CreatureInterface { get; set; }
    }
    /*public class AtkNormal : AbstractCreatureEventC, ISkill
    {
        public SkillAttr SkillAttr { get; }


        public AtkNormal(Creature creature) :
            base(creature)
        {
            SkillAttr = new SkillAttr(EnumSkillTag.AtkNormal);
            // 等動畫播放完才可再次攻擊
            // 注意state裡面不可把狀態設為attack
            CauseEnter = () =>
                !CreatureInterfaceForSkill.IsTag("Attack") && CreatureInterfaceForSkill.GetCreatureAttr().EnableAttackDyn);
            // 偵測動畫是否撥放完
            CauseExit = () =>
                !CreatureInterfaceForSkill.IsTag("Attack") && _min.AndCause()); // min duration

            PreWork += () =>
            {
                CreatureInterfaceForSkill.MindState = EnumMindState.Attacking;
                CreatureInterfaceForSkill.CurrentSkill = EnumSkillTag.AtkNormal;
            };
            PostWork += () =>
            {
                CreatureInterfaceForSkill.MindState = default;
                CreatureInterfaceForSkill.CurrentSkill = default;
            };
            FinalEvent += CreatureInterfaceForSkill.GetAnimManager().Interrupt;
            InitCreatureEventOrder(EnumCreatureEventTag.AtkNormal, EnumOrder.Attack);
        }

        public void Invoke(EnumSymbol symbol)
        {
            if (State == EnumState.Free)
            {
                SkillAttr.Symbol = symbol;
                _min.Reset();
                base.Invoke();
            }
        }

        private CdCondition _min = new CdCondition(0.1f, Stopwatch.Mode.RealWorld);

        protected override void Enter()
        {
            // 撥放音效
            CreatureInterfaceForSkill.Play(SkillAttr.Symbol);
            CreatureInterfaceForSkill.GetAnimManager().Interrupt();
            CreatureInterfaceForSkill.GetAnimManager().Attack(SkillAttr.Symbol);
        }

        protected override void Exit()
        {
        }
    }*/
}