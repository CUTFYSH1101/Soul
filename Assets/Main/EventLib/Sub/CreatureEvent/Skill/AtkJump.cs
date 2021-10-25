using System;
using System.ComponentModel;
using Main.Blood;
using Main.EventLib.Main.EventSystem.Main;
using Main.Game.Collision;
using Main.Entity.Creature;
using Main.EventLib.Common;
using Main.EventLib.Main.EventSystem;
using Main.EventLib.Main.EventSystem.EventOrderbySystem;
using Main.EventLib.Main.EventSystem.Main.Interface;
using Main.EventLib.Main.EventSystem.Main.Sub.Interface;
using Main.EventLib.Sub.CreatureEvent.Skill.Attribute;
using Main.EventLib.Sub.CreatureEvent.Skill.Common;
using Main.Res.Script;
using UnityEngine;

namespace Main.EventLib.Sub.CreatureEvent.Skill
{
    /// <summary>
    /// 空中普攻，被擊中者往反方向擊退。
    /// 沒有冷卻，持續時間根據動畫時長，可透過配點加快。
    /// 在以下條件滿足允許觸發：
    /// - 離地1.8m
    /// - 往上跳途中
    /// - 角色處於允許攻擊的狀態。
    /// </summary>
    public class AtkJump : AbsEventObject, IEvent2, ISkill, IWorkOnCreature
    {
        [Description("離地高度")] private const float GroundClearance = 1.8f;

        public AtkJump(Creature creature)
        {
            this.Build(creature, EnumOrder.Attack, EnumCreatureEventTag.AtkJump);
            SkillAttr = new SkillAttr(EnumSkillTag.AtkJump);
            SkillAttr.SetKnockBack(dynDirection: () =>
                new Vector2(CreatureInterface.LookAtAxisX, -1).normalized);
            
            // 在空中一定距離才能觸發攻擊
            FilterIn = () =>
                !CreatureInterface.IsTag("Attack") &&
                CreatureInterface.GetAttr().EnableAttackDyn &&
                !CreatureInterface.GetAttr().Grounded &&
                (CreatureInterface.GetRb2D().GroundClearance() > GroundClearance ||
                 CreatureInterface.GetRb2D().Velocity.y > 0);
            // 偵測動畫是否撥放完
            ToInterrupt = () =>
                !CreatureInterface.IsTag("Attack") ||
                CreatureInterface.GetRb2D().GroundClearance() < 0.1f;
            PreWork += () =>
            {
                CreatureInterface.MindState = EnumMindState.Attacking;
                CreatureInterface.CurrentSkill = EnumSkillTag.AtkJump;
            };
            PostWork += () =>
            {
                CreatureInterface.MindState = default;
                CreatureInterface.CurrentSkill = default;
            };
            FinalAct += CreatureInterface.GetAnim().Interrupt;
        }

        public void Execute(BloodType shape)
        {
            if (State != EnumState.Free) return;

            SkillAttr.BloodType = shape;
            Director.CreateEvent();
        }

        public void Enter()
        {
            CreatureInterface.GetAnim().AtkJump(SkillAttr.BloodType);
        }

        public void Exit()
        {
        }

        public Func<bool> FilterIn { get; }
        public Func<bool> ToInterrupt { get; }
        public SkillAttr SkillAttr { get; set; }
        public CreatureInterface CreatureInterface { get; set; }
    }
    /*public class AtkJump : AbstractCreatureEventC, ISkill
    {
        // 沒有CD、持續時間根據動畫時長，後續配點會變快
        // 空中的攻擊，被擊中者會往對應方向擊退
        // 以下允許觸發：
        // 1.離地1.8m
        // 2.往上跳途中
        [Description("離地高度")] private const float GroundClearance = 1.8f;
        public SkillAttr SkillAttr { get; }

        public AtkJump(Creature creature) : base(creature)
        {
            SkillAttr = new SkillAttr(EnumSkillTag.AtkJump);
            SkillAttr.SetKnockBack(dynDirection: () =>
                new Vector2(CreatureInterfaceForSkill.LookAtAxisX, -1).normalized); // todo 判斷擊退的方向spoiler

            // 在空中一定距離才能觸發攻擊
            CauseEnter = () =>
                !CreatureInterfaceForSkill.IsTag("Attack") &&
                CreatureInterfaceForSkill.GetCreatureAttr().EnableAttackDyn &&
                !CreatureInterfaceForSkill.GetCreatureAttr().Grounded &&
                (CreatureInterfaceForSkill.GetRigidbody2D().GroundClearance() > GroundClearance ||
                 CreatureInterfaceForSkill.GetRigidbody2D().Velocity.y > 0));

            // 偵測動畫是否撥放完
            CauseExit = () =>
                !CreatureInterfaceForSkill.IsTag("Attack") ||
                CreatureInterfaceForSkill.GetRigidbody2D().GroundClearance() < 0.1f);

            PreWork += () =>
            {
                CreatureInterfaceForSkill.MindState = EnumMindState.Attacking;
                CreatureInterfaceForSkill.CurrentSkill = EnumSkillTag.AtkJump;
            };
            PostWork += () =>
            {
                CreatureInterfaceForSkill.MindState = default;
                CreatureInterfaceForSkill.CurrentSkill = default;
            };
            FinalEvent += CreatureInterfaceForSkill.GetAnimManager().Interrupt;
            InitCreatureEventOrder(EnumCreatureEventTag.AtkJump, EnumOrder.Attack);
        }

        public void Execute(Symbol symbol)
        {
            if (State != EnumState.Free) return;

            SkillAttr.Symbol = symbol;
            base.Execute();
        }

        protected override void Enter()
        {
            CreatureInterfaceForSkill.GetAnimManager().AtkJump(SkillAttr.Symbol);
        }

        protected override void Exit()
        {
            CreatureInterfaceForSkill.GetAnimManager().Interrupt();
        }
    }*/
}