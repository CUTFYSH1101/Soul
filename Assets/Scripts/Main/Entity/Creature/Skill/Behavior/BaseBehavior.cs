using System;
using JetBrains.Annotations;
using Main.Attribute;
using Main.Common;
using Main.Entity.Skill_210528;
using Main.Event;
using UnityEngine;

namespace Main.Entity
{
    public class BaseBehavior : AbstractCreatureBehavior
    {
        [NotNull] private readonly Func<bool> getGrounded;
        [NotNull] private readonly AbstractAudioPlayer audioPlayer;

        public BaseBehavior(AbstractCreature creature, [NotNull] Func<bool> getGrounded,
            [NotNull] AbstractAudioPlayer audioPlayer) : base(creature)
        {
            this.getGrounded = getGrounded;
            this.audioPlayer = audioPlayer;
            dashSkill = new DashSkill(creature, 0.15f,
                () => creature.SetMindState(MindState.Dash),
                () => creature.SetMindState(MindState.Idle));
            knockbackSkill = new KnockbackSkill(creature);

            AppendAttr(knockbackSkill.SkillAttr); // 所有角色的扣血動畫都一樣
        }

        public override void Jump()
        {
            // 不在地面、設定為不可空中移動時、不能移動、正在負面狀態中
            if (!CreatureAttr.MovableDyn)
                return;
            if (!getGrounded() && !CreatureAttr.EnableAirControl)
                return;
            CreatureAttr.Grounded = false;
            Rigidbody2D.AddForce_OnActive(new Vector2(0, CreatureAttr.JumpForce));
        }

        public void WallJump(int dir)
        {
            if (!CreatureAttr.MovableDyn)
                return;
            // 在地面上
            if (getGrounded())
                return;

            AnimManager.WallJump();
            Rigidbody2D.AddForce_OnActive(new Vector2(dir * 0.6f, 0.8f) * CreatureAttr.JumpForce);
        }

        public void Move(float dir)
        {
            if (AnimManager.IsTag("DeBuff") ||
                AnimManager.IsTag("Die"))
            {
                AnimManager.Move(false);
                Rigidbody2D.SetActiveX(0); // 當受到攻擊時，速度歸零
                return;
            }
            if (dir != 0)
            {
                AnimManager.Move(true);
                var moveX = Math.Sign(dir) * CreatureAttr.MoveSpeed;
                Rigidbody2D.SetActiveX(moveX);
                Creature.SetMindState(MindState.Move);
            }
            else
            {
                AnimManager.Move(false);
                Rigidbody2D.SetActiveX(0); // 當受到攻擊時，速度歸零

                // 當切換部分動畫時，不錯誤更改狀態。目前沒什麼用...
                if (AnimManager.IsTag("Attack") || AnimManager.IsTag("DeBuff") ||
                    AnimManager.IsTag("Die"))
                    return;
                Creature.SetMindState(MindState.Idle);
            }
        }

        private readonly DashSkill dashSkill;

        public void Dash(float dir)
        {
            // Grounded && MovableDyn && !CanNotMoving
            if (!CreatureAttr.MovableDyn) return;

            var moveX = dir * CreatureAttr.DashForce;
            dashSkill.Invoke(new Vector2(moveX, 0));
            // Debug.Log(CreatureAttr.DashForce);
        }

        public override void MoveTo(bool @switch, Vector2 targetPos)
        {
            if (AnimManager.IsTag("DeBuff") ||
                AnimManager.IsTag("Die"))
            {
                AnimManager.Move(false);
                Rigidbody2D.SetGuideX(0);
                return;
            }
            if (@switch)
            {
                AnimManager.Move(true);
                Rigidbody2D.MoveTo(targetPos, CreatureAttr.MoveSpeed, CreatureAttr.JumpForce);
                Creature.SetMindState(MindState.Move);
            }
            else
            {
                Rigidbody2D.SetGuideX(0);
                AnimManager.Move(false);
                // 當切換部分動畫時，不錯誤更改狀態
                if (AnimManager.IsTag("Attack") || AnimManager.IsTag("DeBuff") ||
                    AnimManager.IsTag("Die"))
                    return;
                Creature.SetMindState(MindState.Idle);
            }
        }

        private readonly KnockbackSkill knockbackSkill;

        public override void Hit(Vector2 direction, float force, Transform vfx = null, Vector2 offsetPos = default)
        {
            knockbackSkill.Invoke(direction, force, vfx, offsetPos);
        }

        public override void Hit(SkillAttr skillAttr)
        {
            if (skillAttr == null)
                return;
            // knockbackSkill.Invoke(skillAttr.Direction(), skillAttr.Knockback, skillAttr.VFX, skillAttr.OffsetPos);
            Creature.Invoke(knockbackSkill, skillAttr);
        }

        /*public void NormalAttack(Symbol symbol)
        {
            AnimManager.Attack(symbol);
        }*/
    }
}