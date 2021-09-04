using JetBrains.Annotations;
using Main.Entity.Creature;
using Main.EventSystem.Cause;
using Main.EventSystem.Event.Attribute;
using Main.EventSystem.Event.CreatureEventSystem.Common;
using Main.EventSystem.Event.CreatureEventSystem.Decorator;
using Main.EventSystem.Event.CreatureEventSystem.Skill;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Attribute;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Common;
using Main.Util;
using UnityEngine;
using Object = UnityEngine.Object;
using Quaternion = UnityEngine.Quaternion;
using Time = UnityEngine.Time;
using Vector2 = UnityEngine.Vector2;

namespace Main.EventSystem.Event.CreatureEventSystem.StateEvent
{
    public class Knockback : AbstractCreatureEventB, ISkill
    {
        // 專注於擊退事件，擊退位移一段時間，UI和Anim特效
        // 不包含客製化負面效果，動畫切換(因為每個負面狀態的動畫不同)
        // 不包含角色本身的掉血動畫、技能特效，必須在Invoke前設定相關係數，請見SetKnockBack和SetVFX
        // 注意，如果未設定進入負面狀態，角色可以任意打斷該狀態，設定負面狀態請用別類寫
        // 被攻擊到，時間暫停0.1秒->擊退位移+特效生成，等候0.1秒->停止移動，並再次等候0.1秒->結束
        // 為何最後要停止0.1秒，是為了避免負面一解除就能移動，產生一種回彈的怪異效果
        [Tooltip("推力持續時間")] private const float ForceDuration = 0.01f;
        private const float LagDuration = 0.1f;
        private readonly CdCause _forceDuration;
        private readonly CdCause _lagDuration;

        /// [vfx,knockback]。
        /// 裝填攻擊方技能的參數，之後再根據參數產生vfx和擊退效果，不包含角色本身的vfx，不包含角色本身的掉血動畫
        public SkillAttr SkillAttr { get; }

        public Knockback(Creature creature) : base(creature, new EventAttr(
            timerMode: Stopwatch.Mode.RealWorld))
        {
            // 本身不帶有擊退方向，執行擊退行為時，參數由外部設定
            // todo 新增掉血特效
            SkillAttr = new SkillAttr(EnumSkillTag.None);

            _forceDuration = new CdCause(ForceDuration, Stopwatch.Mode.RealWorld);
            _lagDuration = new CdCause(LagDuration, Stopwatch.Mode.RealWorld);
            // 不設定開始條件
            CauseToAction2 = new FuncCause(() => _lagDuration.OrCause());
            CauseToAction3 = new FuncCause(() => _forceDuration.OrCause());
            CauseToAction4 = new FuncCause(() => _lagDuration.OrCause());
            // 玩家或怪物移動時，不會進入擊退狀態
            // CauseInterrupt = new FuncCause(() => Math.Abs(SkillTemplate.GetRigidbody2D().ActiveX) > 0.1f);
            InitCreatureEventOrder(EnumCreatureEventTag.Knockback, EnumOrder.Knockback);
            FinalEvent = () => CreatureInterface.GetRigidbody2D().PassiveX = 0;
        }

        protected override bool FinCauseEnter() => true; // 沒有人可以阻擋我進入knockback

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

        /// 裝填攻擊方技能的參數，之後再根據參數產生vfx和擊退效果，不包含角色本身的vfx，不包含角色本身的掉血動畫。
        /// 使用擊退
        public void Invoke(Vector2 direction, float force)
        {
            if (State != EnumState.Free) return;

            SetKnockBack(force, direction);
            base.Invoke();
        }

        /// 裝填攻擊方技能的參數，之後再根據參數產生vfx和擊退效果，不包含角色本身的vfx，不包含角色本身的掉血動畫。
        /// 使用擊退和特效
        public void Invoke(Vector2 direction, float force,
            Transform vfxObj, Vector2 offsetPos)
        {
            if (State != EnumState.Free) return;

            SetKnockBack(force, direction);
            SetVFX(vfxObj, direction, offsetPos);
            base.Invoke();
        }

        protected override void EnterAction1()
        {
            _lagDuration.Reset();
            Time.timeScale = 0;
        }

        protected override void Action2()
        {
            // Debug.Log(SkillAttr.Knockback.@switch + " " + SkillAttr.VFX.@switch);
            // todo error 詳見excel

            Time.timeScale = 1;
            // 技能特效
            if (SkillAttr.VFX.@switch)
                CreateVFX(SkillAttr);
            // 擊退位移
            _forceDuration.Reset();
            if (SkillAttr.Knockback.Switch && CreatureInterface.GetCreatureAttr().MovableCoeff) // 可以移動之物體才可給予擊退
            {
                CreatureInterface.GetRigidbody2D().ResetAll();
                CreatureInterface.GetRigidbody2D()
                    .AddForce_OnPassive(SkillAttr.Knockback.FinForce, ForceMode2D.Impulse);
            }
        }

        // 暫停移動中，避免回彈
        protected override void Action3()
        {
            _lagDuration.Reset();
            // 不銷毀特效物件
            CreatureInterface.GetRigidbody2D().PassiveX = 0;
        }

        // 回到Idle
        protected override void ExitAction4()
        {
            // 如果有interruptCause，就要放passiveX=0
        }
    }
}