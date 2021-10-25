using System;
using Main.Blood;
using Main.EventLib.Main.EventSystem.Main;
using Main.Game.Collision;
using Main.Entity.Creature;
using Main.EventLib.Common;
using Main.EventLib.Condition;
using Main.EventLib.Main.EventSystem;
using Main.EventLib.Main.EventSystem.EventOrderbySystem;
using Main.EventLib.Main.EventSystem.Main.Interface;
using Main.EventLib.Main.EventSystem.Main.Sub.Interface;
using Main.EventLib.Sub.CreatureEvent.Skill.Attribute;
using Main.EventLib.Sub.CreatureEvent.Skill.Common;
using Main.Game;
using Main.Game.Collision.Event;
using Main.Res.Script;
using Main.Util;
using UnityEngine;

// 有冷卻時間條件
// 1.限定高度1.5米才可觸發，設定狀態參數(沒有持續時間，冷卻7秒)，俯衝
// 2.接觸地面，停止移動和動畫，開始逗留0.5秒（固定值）
// 3.恢復正常移動

// cd:7
namespace Main.EventLib.Sub.CreatureEvent.Skill
{
    /// <summary>
    /// 冷卻時間7秒的特殊攻擊。
    /// 為了速度感，會在空中及地面滯留0.5秒。
    /// 在以下條件下可觸發：
    /// - 角色可移動
    /// - 角色可攻擊
    /// - 離地高度1.5m
    /// - 不在牆角邊（撞牆不動顯得很可笑）
    /// 命令行順序：
    /// 在空中滯留0.5秒->向地面約45度衝刺->落地後，同樣滯留0.5秒->回到idle。
    /// </summary>
    public sealed class AtkDive : AbsEventObject, IEvent4, IWorkOnCreature, ISkill
    {
        private const float Height = 1.5f;

        private bool MoreThenGroundClearance =>
            CreatureInterface.GetRb2D().GroundClearance() > Height;

        private readonly CdCondition _stayDuration;
        private readonly CdCondition _minDuration;
        private readonly ProxyUnityRb2D _rb2D;

        public AtkDive(Creature creature)
        {
            Director = this.Build(creature, EnumOrder.Attack, EnumCreatureEventTag.AtkDive);

            SkillAttr = new SkillAttr(EnumSkillTag.AtkDive)
                .SetKnockBack(CreatureInterface.GetAttr().DiveForce * 0.05f, () =>
                    CreatureInterface.LookAt); // todo 根據對方相對位置
            SkillAttr.Debuff = EnumDebuff.Dizzy; // 帶有暈眩效果

            _stayDuration = new CdCondition(0.3f, Stopwatch.Mode.RealWorld); // 使用後一秒不能操控
            _minDuration = new CdCondition(0.5f); // 最低時長

            var touchTheWallEvent = new CollisionStayWall(CreatureInterface.GetRb2D());
            var attr = CreatureInterface.GetAttr();
            _rb2D = CreatureInterface.GetRb2D();

            // 離地1.5米->慢動作
            FilterIn = () =>
                !CreatureInterface.IsTag("Attack") &&
                !attr.Grounded && MoreThenGroundClearance && // 空中時，允許俯衝
                !touchTheWallEvent.IsTrigger && // 牆角邊禁止俯衝
                attr.EnableMoveDyn && attr.EnableAttackDyn; // 限定可攻擊可移動者
            // 0.3秒->攻擊 (Crash)
            ToAct2 = () => _stayDuration.OrCause();
            // 碰地後->靜止 (Landing)
            ToAct3 = () => attr.Grounded;
            // 0.3秒->回歸一開始 (Idle)
            ToAct4 = () => _stayDuration.OrCause();
            // maxDuration || 撞牆時->回到一開始 (Idle)
            ToInterrupt = () => !_rb2D.IsMoving && touchTheWallEvent.IsTrigger;

            PreWork += () =>
            {
                CreatureInterface.MindState = EnumMindState.Attacking;
                CreatureInterface.CurrentSkill = SkillAttr.SkillTag;
            };
            PostWork += () =>
            {
                // 注意exit是否from knockback
                CreatureInterface.MindState = default;
                CreatureInterface.CurrentSkill = default;
            };
            FinalAct += () =>
            {
                SetAnimState(AnimState.Free);

                Booster(false);
                CreatureInterface.GetAttr().EnableMoveDyn = true; // 允許移動
            };
        }

        private float _originCd;

        public void Execute(BloodType shape)
        {
            if (State != EnumState.Free) return;

            if (_originCd == 0)
                _originCd = EventAttr.CdTime;
            EventAttr.CdTime = DebugMode.IsOpen ? 0 : _originCd;

            _minDuration.Reset();
            _stayDuration.Reset();

            SkillAttr.BloodType = shape;
            Director.CreateEvent();
        }

        // 慢動作
        public void First()
        {
            _rb2D.Velocity = new Vector2(
                _rb2D.Velocity.x * 0.3f,
                _rb2D.Velocity.y);
        }

        // Crash
        public void Act2()
        {
            SetAnimState(AnimState.Crash);
            // 添加推力
            _rb2D.ResetX();
            Booster(true);
            var dir = new Vector2(CreatureInterface.LookAtAxisX * 0.6f, -0.7f).normalized;
            _rb2D.AddForce_OnActive(dir * CreatureInterface.GetAttr().DiveForce,
                ForceMode2D.Impulse);
        }


        public Action AfterTouchGround { get; set; }

        // Landing
        public void Act3()
        {
            AfterTouchGround?.Invoke();
            _stayDuration.Reset();
            SetAnimState(AnimState.Landed);

            _rb2D.ResetAll(); // 停止移動
            CreatureInterface.GetAttr().EnableMoveDyn = false; // 禁止移動
        }

        public void Act4()
        {
        }
        /*// Idle，可自由活動
        public void Act4() => FinalAction?.Invoke();*/

        private void Booster(bool open)
        {
            _rb2D.SwitchPhysicReduceSimulateX = !open;
            _rb2D.FasterByTime = open;
        }

        private enum AnimState
        {
            Free,
            Crash,
            Landed
        }

        /// 切換動畫
        private void SetAnimState(AnimState state)
        {
            var anim = CreatureInterface.GetAnim();
            switch (state)
            {
                case AnimState.Free:
                    anim.Landed(false);
                    anim.AtkDiveCrash(false);
                    break;
                case AnimState.Crash:
                    anim.AtkDiveCrash(true);
                    break;
                case AnimState.Landed:
                    anim.AtkDiveCrash(false);
                    anim.Landed(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public Func<bool> FilterIn { get; }
        public Func<bool> ToAct2 { get; }
        public Func<bool> ToAct3 { get; }
        public Func<bool> ToAct4 { get; }
        public Func<bool> ToInterrupt { get; }
        public SkillAttr SkillAttr { get; set; }
        public CreatureInterface CreatureInterface { get; set; }
    }
}