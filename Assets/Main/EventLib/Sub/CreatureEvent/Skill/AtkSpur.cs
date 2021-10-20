using System;
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
using UnityEngine;

// ReSharper disable PossibleNullReferenceException
// 推力+角色動畫 -> 確認是否ForceTime.IsTimeUp，是則停止移動 -> 確認是否動畫結束，是則結束
// 如果動畫提早結束，也會中斷Force
// [Description("受力持續時間")] private const float ForceTime = 0.01f;
namespace Main.EventLib.Sub.CreatureEvent.Skill
{
    public class AtkSpur : AbsEventObject, IEvent3, IWorkOnCreature, ISkill
    {
        private readonly CdCondition _forceDuration = new(0.01f);
        private float _originDrag;

        public AtkSpur(Creature creature)
        {
            Director = this.Build(creature, EnumOrder.Attack, EnumCreatureEventTag.AtkSpur);
            SkillAttr = new SkillAttr(EnumSkillTag.AtkSpur)
                .SetKnockBack(dynDirection: () =>
                    CreatureInterface.LookAt);

            // 等動畫播放完才可再次攻擊
            FilterIn = () =>
                !CreatureInterface.IsTag("Attack") &&
                CreatureInterface.GetAttr().EnableMoveDyn &&
                CreatureInterface.GetAttr().EnableAttackDyn;
            // 偵測動畫是否撥放完
            ToInterrupt = () => !CreatureInterface.IsTag("Attack"); // min duration

            PreWork += () =>
            {
                CreatureInterface.MindState = EnumMindState.Attacking;
                CreatureInterface.CurrentSkill = EnumSkillTag.AtkSpur;
            };
            PostWork += () =>
            {
                CreatureInterface.MindState = default;
                CreatureInterface.CurrentSkill = default;
            };
            FinalAct += () =>
            {
                CreatureInterface.GetAnim().Interrupt();
                var rb2D = CreatureInterface.GetRb2D();
                // rb2D.ActiveX = 0;
                rb2D.Drag = _originDrag;
            };
        }

        public void Execute(EnumShape shape)
        {
            if (State == EnumState.Free)
            {
                SkillAttr.Shape = shape;
                Director.CreateEvent();
            }
        }

        public void Enter()
        {
            _forceDuration.Reset();
            CreatureInterface.GetAnim().AtkSpur(SkillAttr.Shape);
            // 等於擊退的方向和速度
            var rb2D = CreatureInterface.GetRb2D();
            _originDrag = rb2D.Drag;
            rb2D.ResetX();
            rb2D.AddForce_OnActive(SkillAttr.Knockback.FinForce, ForceMode2D.Impulse);
            rb2D.Drag = 100;
        }

        public void Update()
        {
            // 注意會重複呼叫
            if (_forceDuration.OrCause())
                CreatureInterface.GetRb2D().ActiveX = 0;
        }

        public void Exit()
        {
        }

        public Func<bool> FilterIn { get; }
        public Func<bool> ToInterrupt { get; }
        public SkillAttr SkillAttr { get; set; }
        public CreatureInterface CreatureInterface { get; set; }
    }
    /*public class AtkSpur : AbstractCreatureEventA, ISkill
    {
        // 推力+角色動畫->確認是否ForceTime.IsTimeUp，是則停止移動->確認是否動畫結束，是則結束
        // 如果動畫提早結束，也會中斷Force
        // [Description("受力持續時間")] private const float ForceTime = 0.01f;
        private readonly CdCondition _forceDuration = new CdCondition(0.01f);
        public SkillAttr SkillAttr { get; }
        private float originDrag;

        public AtkSpur(Creature creature) : base(creature)
        {
            SkillAttr = new SkillAttr(EnumSkillTag.AtkSpur)
                .SetKnockBack(dynDirection: () =>
                    CreatureInterfaceForSkill.LookAt);

            // _forceDuration = new CdCondition(ForceTime);

            // 等動畫播放完才可再次攻擊
            CauseEnter = () =>
                !CreatureInterfaceForSkill.IsTag("Attack") && CreatureInterfaceForSkill.GetCreatureAttr().EnableMoveDyn &&
                CreatureInterfaceForSkill.GetCreatureAttr().EnableAttackDyn);
            // 偵測動畫是否撥放完
            CauseExit = () => !CreatureInterfaceForSkill.IsTag("Attack")); // min duration

            PreWork += () =>
            {
                CreatureInterfaceForSkill.MindState = EnumMindState.Attacking;
                CreatureInterfaceForSkill.CurrentSkill = EnumSkillTag.AtkSpur;
            };
            PostWork += () =>
            {
                CreatureInterfaceForSkill.MindState = default;
                CreatureInterfaceForSkill.CurrentSkill = default;
            };
            FinalEvent += () =>
            {
                CreatureInterfaceForSkill.GetAnimManager().Interrupt();
                var rb2D = CreatureInterfaceForSkill.GetRigidbody2D();
                // rb2D.ActiveX = 0;
                rb2D.Drag = 0;
            };
            InitCreatureEventOrder(EnumCreatureEventTag.AtkSpur, EnumOrder.Attack);
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
            _forceDuration.Reset();
            CreatureInterfaceForSkill.GetAnimManager().AtkSpur(SkillAttr.Symbol);
            // 等於擊退的方向和速度
            var rb2D = CreatureInterfaceForSkill.GetRigidbody2D();
            originDrag = rb2D.Drag;
            rb2D.ResetX();
            rb2D.AddForce_OnActive(SkillAttr.Knockback.FinForce, ForceMode2D.Impulse);
            rb2D.Drag = 100;
        }

        protected override void Update()
        {
            /*
            // 注意會重複呼叫
            if (_forceDuration.OrCause())
                CreatureInterface.GetRigidbody2D().ActiveX = 0;
        #1#
        }

        protected override void Exit()
        {
            FinalEvent?.Invoke();
        }
    }*/
}