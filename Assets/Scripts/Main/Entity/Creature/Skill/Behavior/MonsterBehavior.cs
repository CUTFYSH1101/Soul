using System;
using JetBrains.Annotations;
using Main.Attribute;
using Main.Common;
using Main.Entity.Skill_210528;
using Main.Game.Input;
using Main.Util;
using UnityEngine;

namespace Main.Entity
{
    public class MonsterBehavior : AbstractCreatureBehavior
    {
        [NotNull] private readonly DictionaryAudioPlayer audioAudioPlayer;
        [NotNull] private readonly Func<bool> getGrounded;
        [NotNull] private readonly BaseBehavior baseBehavior;

        public MonsterBehavior(AbstractCreature creature, [NotNull] Func<bool> getGrounded) : base(creature)
        {
            this.getGrounded = getGrounded;
            audioAudioPlayer = UnityAudioTool.GetNormalAttackAudioPlayer();
            baseBehavior = new BaseBehavior(creature, getGrounded, audioAudioPlayer);
            jumpController = new JumpController(creature, baseBehavior);
            moveController = new MoveController(creature, HotkeySet.Horizontal,
                baseBehavior.Move, baseBehavior.Dash);
            normalAttack = new NormalAttack(creature, 2); // 設定每兩秒才能攻擊一次
            // normalAttack.SkillAttr.DeBuffBuff = DeBuff.Stiff;
            AppendAttr(normalAttack.SkillAttr);
        }

        private readonly JumpController jumpController;

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

        private readonly NormalAttack normalAttack;
        public void NormalAttack()
        {
            // 每兩秒攻擊一次
            normalAttack.Invoke(Symbol.Direct);
        }
    }
}