using Main.Entity.Creature;
using Main.EventSystem.Common;
using Main.EventSystem.Event.CreatureEventSystem.Common;
using Main.EventSystem.Event.CreatureEventSystem.Decorator;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Attribute;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Common;
using Main.Game.Collision;
using UnityEngine;
using Symbol = Main.AnimAndAudioSystem.Main.Common.EnumSymbol;

namespace Main.EventSystem.Event.CreatureEventSystem.Skill
{
    public class JumpAttack : AbstractCreatureEventC, ISkill
    {
        // 沒有CD、持續時間根據動畫時長，後續配點會變快
        // 空中的攻擊，被擊中者會往對應方向擊退
        [Tooltip("離地高度")] private const float GroundClearance = 1.8f;
        public SkillAttr SkillAttr { get; }

        public JumpAttack(Creature creature) : base(creature)
        {
            SkillAttr = new SkillAttr(EnumSkillTag.JumpAttack);
            SkillAttr.SetKnockBack(dynDirection: () =>
                new Vector2(CreatureInterface.LookAtAxisX, -1).normalized); // todo 判斷擊退的方向spoiler

            // 在空中一定距離才能觸發攻擊
            CauseEnter = new FuncCause(() =>
                !CreatureInterface.GetCreatureAttr().Grounded &&
                !CreatureInterface.IsTag("Attack") &&
                CreatureInterface.GetRigidbody2D().GroundClearance() > GroundClearance &&
                CreatureInterface.GetCreatureAttr().AttackableDyn);

            // 偵測動畫是否撥放完
            CauseExit = new FuncCause(() =>
                !CreatureInterface.IsTag("Attack") ||
                CreatureInterface.GetRigidbody2D().GroundClearance() < 0.1f);

            PreWork += () =>
            {
                CreatureInterface.MindState = EnumMindState.Attack;
                CreatureInterface.CurrentSkill = EnumSkillTag.JumpAttack;
            };
            PostWork += () =>
            {
                CreatureInterface.MindState = default;
                CreatureInterface.CurrentSkill = default;
            };
            FinalEvent += CreatureInterface.GetAnimManager().Interrupt;
            InitCreatureEventOrder(EnumCreatureEventTag.JumpAttack, EnumOrder.Attack);
        }

        public void Invoke(Symbol symbol)
        {
            if (State != EnumState.Free) return;

            SkillAttr.Symbol = symbol;
            base.Invoke();
        }

        protected override void Enter()
        {
            CreatureInterface.GetAnimManager().JumpAttack(SkillAttr.Symbol);
        }

        protected override void Exit()
        {
            CreatureInterface.GetAnimManager().Interrupt();
        }
    }
}