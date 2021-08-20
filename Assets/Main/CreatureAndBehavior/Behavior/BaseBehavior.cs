/*using System;
using Main.EventSystem.Common;
using Main.EventSystem.Event.CreatureEventSystem.MoveEvent;
using Main.EventSystem.Event.CreatureEventSystem.Skill;
using UnityEngine;

namespace Main.CreatureAndBehavior.Behavior
{
    public class BaseBehavior : AbstractCreatureBehavior
    {
        public BaseBehavior(Creature.Creature creature) : base(creature)
        {
            /*
            dashEvent = new DashEvent(creature, 0.15f,
                () => creature.SetMindState(EnumMindState.Dash),
                () => creature.SetMindState(EnumMindState.Idle));
            knockbackSkill = new KnockbackSkill(creature);

            AppendAttr(knockbackSkill.SkillAttr); // 所有角色的扣血動畫都一樣
        #1#
        }

        public override void Jump()
        {
            // 不在地面、設定為不可空中移動時、不能移動、正在負面狀態中
            if (!CreatureAttr.MovableDyn)
                return;
            if (!CreatureAttr.Grounded && !CreatureAttr.EnableAirControl)
                return;
            CreatureAttr.Grounded = false;
            Rigidbody2D.ResetY(); // 避免小跳
            Rigidbody2D.AddForce_OnActive(new Vector2(0, CreatureAttr.JumpForce), ForceMode2D.Impulse);
        }

        public void WallJump(int dir)
        {
            if (!CreatureAttr.MovableDyn)
                return;
            // 在地面上
            if (CreatureAttr.Grounded)
                return;

            AnimManager.WallJump();
            // 避免bug
            Rigidbody2D.ResetAll();
            Rigidbody2D.AddForce_OnActive(new Vector2(dir * 0.6f, 0.8f) * CreatureAttr.JumpForce, ForceMode2D.Impulse);
        }

        // 本身不包含MovableDyn等判斷式
        public void Move(int dir, bool setSpeed = true, bool setMind = true, bool setAnim = true)
        {
            // 如果為負面狀態，SetActiveX為0，由外部更改state
            // 如果成功執行，狀態更改為move
            // 如果自主停下，而非負面，狀態更改為idle
            // 如果在空中，停止動畫，保留ActiveX的速度，不設定為0

            // 如果在空中，保留舊數值
            if (setAnim) AnimManager.Move(dir != 0);
            // 當受到攻擊時，速度歸零
            if (setSpeed) Rigidbody2D.ActiveX = Math.Sign(dir) * CreatureAttr.MoveSpeed;
            // 當受到攻擊，不更改狀態，以免干擾
            if (setMind) Creature.SetMindState(dir != 0 ? EnumMindState.Move : EnumMindState.Idle);
        }

        private readonly DashEvent _dashEvent;

        public void Dash(float dirX)
        {
            if (!CreatureAttr.MovableDyn) return;

            _dashEvent.Invoke(new Vector2(dirX * CreatureAttr.DashForce, 0));
        }

        public void InterruptDash() => _dashEvent.Interrupt();

        public void MoveTo(bool @switch, Vector2 targetPos, bool setSpeed = true, bool setMind = true,
            bool setAnim = true)
        {
            // 如果在空中，保留舊數值
            if (setAnim) AnimManager.Move(@switch);
            // 當受到攻擊時，速度歸零
            if (setSpeed)
            {
                if (@switch) Rigidbody2D.MoveTo(targetPos, CreatureAttr.MoveSpeed, CreatureAttr.JumpForce);
                else Rigidbody2D.GuideX = 0;
            }

            // 當受到攻擊，不更改狀態，以免干擾
            if (setMind) Creature.SetMindState(@switch ? EnumMindState.Move : EnumMindState.Idle);
        }
    }
}*/