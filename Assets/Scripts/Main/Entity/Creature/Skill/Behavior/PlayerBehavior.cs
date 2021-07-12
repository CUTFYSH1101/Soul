using System;
using JetBrains.Annotations;
using Main.Attribute;
using Main.Common;
using Main.Entity.Creature.Skill.Skill_210528;
using Main.Entity.Skill_210528;
using Main.Game.Input;
using Main.Util;
using UnityEngine;

namespace Main.Entity
{
    [Serializable]
    public class PlayerBehavior : AbstractCreatureBehavior
    {
        [NotNull] private readonly DictionaryAudioPlayer audioAudioPlayer;
        [NotNull] private readonly Func<bool> getGrounded;
        [NotNull] private readonly BaseBehavior baseBehavior;

        public PlayerBehavior(AbstractCreature creature, [NotNull] Func<bool> getGrounded) : base(creature)
        {
            this.getGrounded = getGrounded;
            audioAudioPlayer = UnityAudioTool.GetNormalAttackAudioPlayer();
            baseBehavior = new BaseBehavior(creature, getGrounded, audioAudioPlayer);
            moveController = new MoveController(creature, HotkeySet.Horizontal,
                baseBehavior.Move, baseBehavior.Dash);
            jumpController = new JumpController(creature, baseBehavior);
            normalAttack = new NormalAttack(creature);
            normalAttack.SkillAttr.DeBuffBuff = DeBuff.Stiff;// 給予擊退，搭配event
            diveAttack = new DiveAttack(creature);
            spurAttack = new SpurAttack(creature,
                () => moveController.Switch(false),
                () => moveController.Switch(true));
            spurAttack.SkillAttr.DeBuffBuff = DeBuff.Stiff;
            jumpAttack = new JumpAttack(creature);
            spurAttack.SkillAttr.DeBuffBuff = DeBuff.Stiff;

            AppendAttr(normalAttack.SkillAttr);
            AppendAttr(diveAttack.SkillAttr);
            AppendAttr(spurAttack.SkillAttr);
            AppendAttr(jumpAttack.SkillAttr);
        }

        [NotNull] private readonly JumpController jumpController;

        public override void Jump()
        {
            jumpController.Jump(); // wallJump + jump
        }

        private readonly MoveController moveController;

        /// update
        public void Move()
        {
            moveController.Update();
        }

        public override void MoveTo(bool @switch, Vector2 targetPos)
        {
            baseBehavior.MoveTo(@switch, targetPos);
        }

        public override void Hit(Vector2 direction, float force, Transform vfx = null, Vector2 offsetPos = default)
        {
            baseBehavior.Hit(direction, force, vfx, offsetPos);
        }

        public override void Hit(SkillAttr skillAttr)
        {
            baseBehavior.Hit(skillAttr);
        }

        [NotNull] private readonly NormalAttack normalAttack;

        public void NormalAttack(Symbol symbol)
        {
            normalAttack.Invoke(symbol); // 玩家專屬normalAttack、音效
        }

        [NotNull] private readonly SpurAttack spurAttack;

        public void SpurAttack(Symbol symbol)
        {
            spurAttack.Invoke(symbol); // 玩家專屬
        }

        [NotNull] private readonly DiveAttack diveAttack;

        public void DiveAttack(Symbol symbol)
        {
            diveAttack.Invoke(symbol);
        }

        [NotNull] private readonly JumpAttack jumpAttack;

        public void JumpAttack(Symbol symbol)
        {
            /*if (!getGrounded() && !AnimManager.IsTag("Attack") && Creature.GetTransform().GroundClearance() > 0.7f)
            {
                AnimManager.JumpAttack(symbol);
            }*/
            jumpAttack.Invoke(symbol);
        }
    }
}