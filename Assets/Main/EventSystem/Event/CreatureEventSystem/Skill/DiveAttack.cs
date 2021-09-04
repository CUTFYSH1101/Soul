/*using System;
using Main.AnimAndAudioSystem.Main.Common;
using Main.Entity.Creature;
using Main.EventSystem.Cause;
using Main.EventSystem.Common;
using Main.EventSystem.Event.Attribute;
using Main.EventSystem.Event.CreatureEventSystem.Common;
using Main.EventSystem.Event.CreatureEventSystem.Decorator;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Attribute;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Common;
using Main.Game.Collision;
using UnityEngine;

namespace Main.EventSystem.Event.CreatureEventSystem.Skill
{
    public class DiveAttack : AbstractCreatureEventB, ISkill
    {
        // 有冷卻時間條件
        // 1.限定高度1.5米才可觸發，設定狀態參數(沒有持續時間，冷卻7秒)，俯衝
        // 2.接觸地面，停止移動和動畫，開始逗留0.5秒（固定值）
        // 3.恢復正常移動
        private const float Height = 1.5f;

        private bool MoreThenGroundClearance =>
            CreatureInterface.GetCreature().Transform.GroundClearance() > Height;

        private readonly CdCause _stayDuration;
        private readonly CdCause _minDuration;
        public SkillAttr SkillAttr { get; }

        public DiveAttack(AbstractCreature creature, float cdTime = 7) :
            base(creature, new EventAttr(cdTime))
        {
            SkillAttr = new SkillAttr(EnumSkillTag.DiveAttack, cdTime: cdTime)
                .SetKnockBack(CreatureInterface.GetCreatureAttr().DiveForce * 0.05f, () => // todo 修改力大小
                    new Vector2(CreatureInterface.GetDirX, 0)); // todo 根據對方相對位置
            SkillAttr.DeBuff = DeBuff.Dizzy; // 帶有暈眩效果

            _stayDuration = new CdCause(0.2f); // 使用後一秒不能操控
            _minDuration = new CdCause(0.5f); // 最低時長

            var touchTheWallEvent = new CollisionManager.TouchTheWallEvent(CreatureInterface.GetRigidbody2D());
            var attr = CreatureInterface.GetCreatureAttr();
            // Debug.Log(creature.GetAnimator().HasParameter(UnityAnimID.DiveAttacking));
            // 離地1.5米可觸發
            CauseEnter = new FuncCause(() =>
                !CreatureInterface.IsTag("Attack") &&
                !attr.Grounded && MoreThenGroundClearance && // 空中時，允許俯衝
                !touchTheWallEvent.IsTriggerStay && // 牆角邊禁止俯衝
                attr.MovableDyn && attr.AttackableDyn); // 限定可攻擊可移動者
            // 碰地
            CauseToAction2 = new FuncCause(() => attr.Grounded);
            // 碰地後經過1秒
            CauseToAction3 = new FuncCause(() => _stayDuration.OrCause());
            // maxDuration或撞到牆壁時，中斷方法
            CauseInterrupt = new FuncCause(() =>
                !CreatureInterface.GetRigidbody2D().IsMoving && touchTheWallEvent.IsTriggerStay);

            PreWork += () =>
            {
                CreatureInterface.MindState = EnumMindState.Attack;
                CreatureInterface.CurrentSkill = SkillAttr.SkillTag;
            };
            PostWork += () =>
            {
                // 注意exit是否from knockback
                CreatureInterface.MindState = default;
                CreatureInterface.CurrentSkill = default;
            };
            FinalEvent += () =>
            {
                // 中斷時執行
                CreatureInterface.GetCreatureAttr().MovableDyn = true; // 允許移動
                CreatureInterface.GetRigidbody2D().SwitchPhysicReduceSimulateX = true;
                CreatureInterface.GetRigidbody2D().FasterByTime = false;
                if (SkillAttr.Symbol == EnumSymbol.None) CreatureInterface.GetAnimManager().Interrupt();
                else CreatureInterface.GetAnimManager().DiveAttack(SkillAttr.Symbol, false); // 切換動畫
            };
            InitCreatureEventOrder(EnumCreatureEventTag.DiveAttack, EnumOrder.Attack);
        }

        public void Invoke(EnumSymbol symbol)
        {
            if (State != EnumState.Free) return;

            _minDuration.Reset();
            SkillAttr.Symbol = symbol;
            base.Invoke();
        }

        // Crash
        protected override void EnterAction1()
        {
            // Debug.Log("EnterAction1");
            var dir = new Vector2(CreatureInterface.GetDirX * 0.6f, -0.7f).normalized;
            // 切換動畫
            CreatureInterface.GetAnimManager().DiveAttack(SkillAttr.Symbol, true); // 注意需要先更改skillAttr的數值，才有辦法呼叫
            // 避免衝刺錯誤
            CreatureInterface.GetRigidbody2D().SwitchPhysicReduceSimulateX = false;
            CreatureInterface.GetRigidbody2D().FasterByTime = true;
            CreatureInterface.GetRigidbody2D().ResetX();
            // 添加力
            CreatureInterface.AddForce_OnActive(dir * CreatureInterface.GetCreatureAttr().DiveForce,
                ForceMode2D.Impulse);
        }

        // Landing+Stay
        protected override void Action2()
        {
            // Debug.Log(CreatureInterface.GetRigidbody2D().Velocity.x);
            PreAction2?.Invoke();
            _stayDuration.Reset();
            CreatureInterface.GetRigidbody2D().ResetAll(); // 停止移動
            CreatureInterface.GetCreatureAttr().MovableDyn = false; // 禁止移動
            CreatureInterface.GetAnimManager().DiveAttack(SkillAttr.Symbol, false); // 切換動畫
            CreatureInterface.GetAnimManager().ExitDiveAttack(true); // 切換動畫
        }

        public Action PreAction2 { get; set; }
        // Idle
        protected override void Action3()
        {
            CreatureInterface.GetAnimManager().ExitDiveAttack(false); // 切換動畫
        }

        protected override void ExitAction4()
        {
            // 中斷時執行
            CreatureInterface.GetCreatureAttr().MovableDyn = true; // 允許移動
            CreatureInterface.GetRigidbody2D().SwitchPhysicReduceSimulateX = true;
            CreatureInterface.GetRigidbody2D().FasterByTime = false;
            CreatureInterface.GetAnimManager().DiveAttack(SkillAttr.Symbol, false); // 切換動畫
        }
    }
}*/

using System;
using Main.AnimAndAudioSystem.Main.Common;
using Main.Entity.Creature;
using Main.EventSystem.Cause;
using Main.EventSystem.Common;
using Main.EventSystem.Event.Attribute;
using Main.EventSystem.Event.CreatureEventSystem.Common;
using Main.EventSystem.Event.CreatureEventSystem.Decorator;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Attribute;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Common;
using Main.Game.Collision;
using Main.Util;
using UnityEngine;

namespace Main.EventSystem.Event.CreatureEventSystem.Skill
{
    public class DiveAttack : AbstractCreatureEventB, ISkill
    {
        // 有冷卻時間條件
        // 1.限定高度1.5米才可觸發，設定狀態參數(沒有持續時間，冷卻7秒)，俯衝
        // 2.接觸地面，停止移動和動畫，開始逗留0.5秒（固定值）
        // 3.恢復正常移動
        private const float Height = 1.5f;

        private bool MoreThenGroundClearance =>
            CreatureInterface.GetCreature().Transform.GroundClearance() > Height;

        private readonly CdCause _stayDuration;
        private readonly CdCause _minDuration;
        public SkillAttr SkillAttr { get; }

        public DiveAttack(Creature creature, float cdTime = 7) :
            base(creature, new EventAttr(cdTime))
        {
            SkillAttr = new SkillAttr(EnumSkillTag.DiveAttack, cdTime: cdTime)
                .SetKnockBack(CreatureInterface.GetCreatureAttr().DiveForce * 0.05f, () => // todo 修改力大小
                    CreatureInterface.LookAt); // todo 根據對方相對位置
            SkillAttr.DeBuff = DeBuff.Dizzy; // 帶有暈眩效果

            _stayDuration = new CdCause(0.2f, Stopwatch.Mode.RealWorld); // 使用後一秒不能操控
            _minDuration = new CdCause(0.5f); // 最低時長

            var touchTheWallEvent = new CollisionManager.TouchTheWallEvent(CreatureInterface.GetRigidbody2D());
            var attr = CreatureInterface.GetCreatureAttr();
            // Debug.Log(creature.GetAnimator().HasParameter(UnityAnimID.DiveAttacking));
            // 離地1.5米可觸發
            CauseEnter = new FuncCause(() =>
                !CreatureInterface.IsTag("Attack") &&
                !attr.Grounded && MoreThenGroundClearance && // 空中時，允許俯衝
                !touchTheWallEvent.IsTriggerStay && // 牆角邊禁止俯衝
                attr.MovableDyn && attr.AttackableDyn); // 限定可攻擊可移動者

            /*// 碰地
            CauseToAction2 = new FuncCause(() => attr.Grounded);
            // 碰地後經過1秒
            CauseToAction3 = new FuncCause(() => _stayDuration.OrCause());
            // maxDuration或撞到牆壁時，中斷方法
            CauseInterrupt = new FuncCause(() =>
                !CreatureInterface.GetRigidbody2D().IsMoving && touchTheWallEvent.IsTriggerStay); 8/13衝刺攻擊改動前*/

            // 碰地
            CauseToAction2 = new FuncCause(() => _stayDuration.OrCause());
            // 碰地後經過1秒
            CauseToAction3 = new FuncCause(() => attr.Grounded);
            CauseToAction4 = new FuncCause(() => _stayDuration.OrCause());
            // maxDuration或撞到牆壁時，中斷方法
            CauseInterrupt = new FuncCause(() =>
                !CreatureInterface.GetRigidbody2D().IsMoving && touchTheWallEvent.IsTriggerStay);


            PreWork += () =>
            {
                CreatureInterface.MindState = EnumMindState.Attack;
                CreatureInterface.CurrentSkill = SkillAttr.SkillTag;
            };
            PostWork += () =>
            {
                // 注意exit是否from knockback
                CreatureInterface.MindState = default;
                CreatureInterface.CurrentSkill = default;
            };
            FinalEvent += () =>
            {
                // 中斷時執行
                CreatureInterface.GetCreatureAttr().MovableDyn = true; // 允許移動
                CreatureInterface.GetRigidbody2D().SwitchPhysicReduceSimulateX = true;
                CreatureInterface.GetRigidbody2D().FasterByTime = false;
                if (SkillAttr.Symbol == EnumSymbol.None) CreatureInterface.GetAnimManager().Interrupt();
                else CreatureInterface.GetAnimManager().DiveAttack(SkillAttr.Symbol, false); // 切換動畫
            };
            InitCreatureEventOrder(EnumCreatureEventTag.DiveAttack, EnumOrder.Attack);
        }

        public void Invoke(EnumSymbol symbol)
        {
            if (State != EnumState.Free) return;

            _minDuration.Reset();
            _stayDuration.Reset();

            SkillAttr.Symbol = symbol;
            base.Invoke();
        }

        // Delay 0.2s
        protected override void EnterAction1()
        {
            CreatureInterface.GetRigidbody2D().Velocity = new Vector2(
                CreatureInterface.GetRigidbody2D().Velocity.x * 0.3f, CreatureInterface.GetRigidbody2D().Velocity.y);
        }

        // Crash
        protected override void Action2()
        {
            // Time.timeScale = 1f;

            // Debug.Log("EnterAction1");
            var dir = new Vector2(CreatureInterface.LookAtAxisX * 0.6f, -0.7f).normalized;
            // 切換動畫
            CreatureInterface.GetAnimManager().DiveAttack(SkillAttr.Symbol, true); // 注意需要先更改skillAttr的數值，才有辦法呼叫
            // 避免衝刺錯誤
            CreatureInterface.GetRigidbody2D().SwitchPhysicReduceSimulateX = false;
            CreatureInterface.GetRigidbody2D().FasterByTime = true;
            CreatureInterface.GetRigidbody2D().ResetX();
            // 添加力
            CreatureInterface.AddForce_OnActive(dir * CreatureInterface.GetCreatureAttr().DiveForce,
                ForceMode2D.Impulse);
        }

        public Action AfterTouchGround { get; set; }

        // Landing+Stay，接觸地面後，有一段時間不能移動
        protected override void Action3()
        {
            // Debug.Log(CreatureInterface.GetRigidbody2D().Velocity.x);
            AfterTouchGround?.Invoke();
            _stayDuration.Reset();
            CreatureInterface.GetRigidbody2D().ResetAll(); // 停止移動
            CreatureInterface.GetCreatureAttr().MovableDyn = false; // 禁止移動
            CreatureInterface.GetAnimManager().DiveAttack(SkillAttr.Symbol, false); // 切換動畫
            CreatureInterface.GetAnimManager().ExitDiveAttack(true); // 切換動畫
        }

        // Idle，可自由活動
        protected override void ExitAction4()
        {
            CreatureInterface.GetAnimManager().ExitDiveAttack(false); // 切換動畫

            // 中斷時執行
            CreatureInterface.GetCreatureAttr().MovableDyn = true; // 允許移動
            CreatureInterface.GetRigidbody2D().SwitchPhysicReduceSimulateX = true;
            CreatureInterface.GetRigidbody2D().FasterByTime = false;
            CreatureInterface.GetAnimManager().DiveAttack(SkillAttr.Symbol, false); // 切換動畫
        }
    }
}