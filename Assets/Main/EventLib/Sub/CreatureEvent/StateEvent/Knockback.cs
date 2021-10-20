using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Main.EventLib.Main.EventSystem.Main;
using Main.Entity.Creature;
using Main.EventLib.Condition;
using Main.EventLib.Main.EventSystem;
using Main.EventLib.Main.EventSystem.EventOrderbySystem;
using Main.EventLib.Main.EventSystem.Main.Interface;
using Main.EventLib.Main.EventSystem.Main.Sub.Interface;
using Main.EventLib.Sub.CreatureEvent.Skill;
using Main.EventLib.Sub.CreatureEvent.Skill.Attribute;
using UnityEngine;
using static Main.Util.Stopwatch.Mode;
using Object = UnityEngine.Object;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

namespace Main.EventLib.Sub.CreatureEvent.StateEvent
{
    /// <summary>
    /// 專注於擊退位移。
    /// 不處理角色狀態。
    /// 會生成導入的特效物件。
    /// </summary>
    /// <remarks>
    /// 為避免被干擾，需要仰賴以下才能正確執行效果。
    /// 1.角色事件排序系統、
    /// 2.EnumDeBuff、
    /// 3.CreatureAttr.IsDuringDeBuff。
    /// 無須設定SkillAttr，而是外部決定。
    /// 因為優先級高，沒有filter condition
    /// </remarks>
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class Knockback : AbsEventObject, IEvent4, IWorkOnCreature, ISkill
    {
        private CdCondition _forceDuration;
        private CdCondition _lagDuration;
        private const float ForceDuration = 0.02f; // 推力持續時間
        private const float LagDuration = 0.1f; // 滯留時間。or 0.12f
        private float _originDrag;

        [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
        public Knockback(Creature creature)
        {
            this.Build(creature, EnumOrder.Knockback, EnumCreatureEventTag.Knockback);
            EventAttr = new EventAttr(timerMode: RealWorld);

            _forceDuration = new CdCondition(ForceDuration, RealWorld);
            _lagDuration = new CdCondition(LagDuration, RealWorld);

            // 不設定開始條件，優先級高
            ToAct2 = _lagDuration.OrCause;
            ToAct3 = _forceDuration.OrCause;

            FinalAct = () =>
            {
                CreatureInterface.GetRb2D().PassiveX = 0;
                CreatureInterface.GetRb2D().Drag = _originDrag;
            };
        }

        /// 裝填攻擊方技能的參數，之後再根據參數產生vfx和擊退效果，不包含角色本身的vfx，不包含角色本身的掉血動畫。
        /// 使用擊退
        public void Execute(Vector2 direction, float force)
        {
            if (State != EnumState.Free) return;

            SetKnockBack(force * 30, direction); // 15, 5000
            Director.CreateEvent();
        }

        /// 裝填攻擊方技能的參數，之後再根據參數產生vfx和擊退效果，不包含角色本身的vfx，不包含角色本身的掉血動畫。
        /// 使用擊退和特效
        public void Execute(Vector2 direction, float force,
            Transform vfxObj, Vector2 offsetPos)
        {
            if (State != EnumState.Free) return;

            SetKnockBack(force, direction);
            SetVFX(vfxObj, direction, offsetPos);
            Director.CreateEvent();
        }

        // 滯留0.5秒
        public void First() => _lagDuration.Reset();

        // knockback
        public void Act2()
        {
            // 技能特效
            if (SkillAttr.VFX.@switch)
                CreateVFX(SkillAttr);

            // 擊退位移
            _forceDuration.Reset();
            if (Knockbackable)
                GetHit();
        }

        /// 可以移動之物體才可給予擊退
        private bool Knockbackable => SkillAttr.Knockback.Switch && CreatureInterface.GetAttr().MovableCoeff;

        private void GetHit()
        {
            _originDrag = CreatureInterface.GetRb2D().Drag;
            CreatureInterface.GetRb2D().Drag = 5000;
            CreatureInterface.GetRb2D().ResetAll();
            CreatureInterface.GetRb2D()
                .AddForce_OnPassive(SkillAttr.Knockback.FinForce, ForceMode2D.Impulse);
        }

        public void Act3()
        {
        }
        // end
        // public void Act3() => FinalAct?.Invoke();

        public void Act4()
        {
        }

        private void CreateVFX(SkillAttr skillAttr)
        {
            var _ = skillAttr.VFX;

            if (_.@switch == false) return;
            if (_.obj == null) return;

            Vector2 absPosition = default;
            // 預設為該生物的座標
            if (_.offsetPos == default)
                absPosition = CreatureInterface.GetCreature().AbsolutePosition;

            Object.Instantiate(_.obj, absPosition + _.offsetPos, Quaternion.LookRotation(_.dynDirection().normalized));
        }

        private void SetVFX([CanBeNull] Transform obj, Vector2 direction = default, Vector2 offsetPos = default,
            bool @switch = false) => SkillAttr.SetVFX(obj, () => direction, offsetPos, @switch);

        private void SetKnockBack(float force, Vector2 direction = default, bool @switch = true)
            => SkillAttr.SetKnockBack(force, () => direction, @switch);

        public Func<bool> FilterIn => null;
        public Func<bool> ToInterrupt => null;
        public Func<bool> ToAct2 { get; }
        public Func<bool> ToAct3 { get; }
        public Func<bool> ToAct4 => null;
        public SkillAttr SkillAttr { get; set; }
        public CreatureInterface CreatureInterface { get; set; }
    }
}