using System;
using Main.AnimAndAudioSystem.Main.Common;
using Main.Entity.Creature;
using Main.EventSystem.Common;
using Main.EventSystem.Cause;
using Main.EventSystem.Event.CreatureEventSystem.Common;
using Main.EventSystem.Event.CreatureEventSystem.Decorator;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Attribute;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Common;
using UnityEngine;

namespace Main.EventSystem.Event.CreatureEventSystem.Skill
{
    public class SpurAttack : AbstractCreatureEventA, ISkill
    {
        // 推力+角色動畫->確認是否ForceTime.IsTimeUp，是則停止移動->確認是否動畫結束，是則結束
        // 如果動畫提早結束，也會中斷Force
        [Tooltip("受力持續時間")] private const float ForceTime = 0.1f;
        [Tooltip("擊退力大小")] private const float Force = 8;
        private readonly CdCause _forceDuration;
        public SkillAttr SkillAttr { get; }

        public SpurAttack(AbstractCreature creature) : base(creature)
        {
            SkillAttr = new SkillAttr(EnumSkillTag.SpurAttack)
                .SetKnockBack(Force, () =>
                new Vector2(CreatureInterface.GetDirX, 0));

            _forceDuration = new CdCause(ForceTime);

            // 等動畫播放完才可再次攻擊
            CauseEnter = new FuncCause(() =>
                !CreatureInterface.IsTag("Attack") && CreatureInterface.GetCreatureAttr().MovableDyn &&
                CreatureInterface.GetCreatureAttr().AttackableDyn);
            // 偵測動畫是否撥放完
            CauseExit = new FuncCause(() => !CreatureInterface.IsTag("Attack")); // min duration

            PreWork += () =>
            {
                CreatureInterface.MindState = EnumMindState.Attack;
                CreatureInterface.CurrentSkill = EnumSkillTag.SpurAttack;
            };
            PostWork += () =>
            {
                CreatureInterface.MindState = default;
                CreatureInterface.CurrentSkill = default;
            };
            FinalEvent += () =>
            {
                CreatureInterface.GetRigidbody2D().ActiveX = 0;
                CreatureInterface.GetAnimManager().Interrupt();
            };
            InitCreatureEventOrder(EnumCreatureEventTag.SpurAttack,EnumOrder.Attack);
        }

        public void Invoke(EnumSymbol symbol)
        {
            if (State == EnumState.Free)
            {
                _forceDuration.Reset();
                SkillAttr.Symbol = symbol;
                base.Invoke();
            }
        }

        protected override void Enter()
        {
            CreatureInterface.GetAnimManager().SpurAttack(SkillAttr.Symbol);
            // 等於擊退的方向和速度
            CreatureInterface.GetRigidbody2D().ResetX();
            CreatureInterface.AddForce_OnActive(SkillAttr.Knockback.FinForce, ForceMode2D.Impulse);
        }

        protected override void Update()
        {
            // 注意會重複呼叫
            if (_forceDuration.OrCause())
                CreatureInterface.GetRigidbody2D().ActiveX = 0;
        }

        protected override void Exit()
        {
            CreatureInterface.GetRigidbody2D().ActiveX = 0;
        }
    }
    /*public class SpurAttack : IEventB, ISkill
    {
        // 推力+角色動畫->確認是否ForceTime.IsTimeUp，是則停止移動->確認是否動畫結束，是則結束
        // 如果動畫提早結束，也會中斷Force
        [Tooltip("受力持續時間")] private const float ForceTime = 0.1f;
        [Tooltip("擊退力大小")] private const float Force = 8;
        private readonly CdCause forceDuration;
        public SkillAttr SkillAttr { get; }
        public SkillTemplate SkillTemplate { get; }

        public SpurAttack(Creature.Creature creature, Action onEnter, Action onExit) : base(default)
        {
            SkillTemplate = new SkillTemplate(creature);

            SkillAttr = new SkillAttr(EnumSkillName.SpurAttack);
            SkillAttr.SetKnockBack(Force, () =>
                new Vector2(SkillTemplate.GetDirX, 0));

            forceDuration = new CdCause(ForceTime);

            // 等動畫播放完才可再次攻擊
            CauseEnter = new FuncCause(() =>
                !SkillTemplate.IsTag("Attack") && SkillTemplate.GetCreatureAttr().MovableDyn &&
                SkillTemplate.GetCreatureAttr().AttackableDyn);
            // 時間到就停止力量
            CauseToAction2 = new FuncCause(forceDuration.OrCause);
            // 偵測動畫是否撥放完
            CauseInterrupt = new FuncCause(() => !SkillTemplate.IsTag("Attack")); // min duration

            BeforeEnter += () =>
            {
                onEnter?.Invoke(); // 注意次序，避免moveController誤設為idle
                SkillTemplate.MindState = EnumMindState.Attack;
                SkillTemplate.SkillName = EnumSkillName.SpurAttack;
            };
            AfterExit += () =>
            {
                SkillTemplate.MindState = default;
                SkillTemplate.SkillName = default;
                onExit?.Invoke();
            };
        }

        public void Invoke(EnumSymbol symbol)
        {
            if (State == EnumState.Free)
            {
                forceDuration.Reset();
                SkillAttr.Symbol = symbol;
                base.Invoke();
            }
        }

        protected override void EnterAction1()
        {
            SkillTemplate.GetAnimManager().SpurAttack(SkillAttr.Symbol);
            // 等於擊退的方向和速度
            SkillTemplate.GetRigidbody2D().ResetX();
            SkillTemplate.AddForce_OnActive(SkillAttr.Knockback.finForce, ForceMode2D.Impulse);
        }

        protected override void Action2()
        {
            SkillTemplate.GetRigidbody2D().ActiveX = 0;
        }

        protected override void Action3()
        {
        }

        protected override void ExitAction4()
        {
            SkillTemplate.GetRigidbody2D().ActiveX = 0;
        }
    }*/
}