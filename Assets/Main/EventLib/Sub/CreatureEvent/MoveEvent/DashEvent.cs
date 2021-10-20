using System;
using Main.EventLib.Main.EventSystem.Main;
using Main.Entity.Creature;
using Main.EventLib.Common;
using Main.EventLib.Main.EventSystem;
using Main.EventLib.Main.EventSystem.EventOrderbySystem;
using Main.EventLib.Main.EventSystem.Main.Interface;
using Main.EventLib.Main.EventSystem.Main.Sub.Interface;
using Main.EventLib.Sub.CreatureEvent.Skill;
using UnityEngine;
using static Main.Res.Script.Audio.DictAudioPlayer.Key;
// ReSharper disable PossibleNullReferenceException

namespace Main.EventLib.Sub.CreatureEvent.MoveEvent
{
    public sealed class DashEvent : AbsEventObject, IEvent3, IWorkOnCreature
    {
        // 有冷卻時間和持續時間條件
        private bool _inited;

        private bool IsStopMoving()
        {
            if (!_inited) return false; // 避免一開始就使條件成立
            return !CreatureInterface.GetRb2D().IsMoving;
        }

        private Vector2 _force;
        private float _forceValue;

        public void SetDash(float value, float forceDuration)
        {
            _forceValue = value;
            EventAttr.MaxDuration = forceDuration;
            _inited = true;
        }

        public void SetForce(Vector2 value)
        {
            _force = value;
            _inited = true;
        }

        // dbClick -> onEnter
        // CauseDuration.IsTimeUp || math.abs rigidbody.velocity.x <= 0.1f -> onExit
        public DashEvent(Creature creature,
            float cdTime = 0.8f, float duration = 0.19f)
        {
            Director = this.Build(creature, EnumOrder.Dash, EnumCreatureEventTag.Dash);
            EventAttr = new EventAttr(cdTime, duration);
            // no SkillAttr

            FilterIn = () =>
                CreatureInterface.MovableDyn && _inited; // 防呆。事件執行順序：SetForce->Enter
            ToInterrupt = IsStopMoving;

            PreWork += () => creature.SetMindState(EnumMindState.Dashing);
            PostWork += () => creature.SetMindState(EnumMindState.Idle);

            FinalAct += () =>
            {
                // 1.在空中會繼續向前移動一段距離，在地面則歸零 2.在DiveAttack時會歸零
                CreatureInterface.GetRb2D().ActiveX *= CreatureInterface.Grounded ? 0 : 0.3f;
                CreatureInterface.GetRb2D().UseGravity(true);
                CreatureInterface.GetAnim().Dash(false);
                d_f = CreatureInterface.GetAbsolutePosition().x;
                Debug.Log(d_f-d_0);
            };
        }

        public void Invoke(Vector2 dir)
        {
            if (State != EnumState.Free) return;
            SetForce(dir.normalized * CreatureInterface.GetAttr().DashForce);
            Director.CreateEvent();
        }
        private float d_0, d_f;
        public void Enter()
        {
            _inited = false;
            // 開啟動畫
            CreatureInterface.Play(Dash);
            CreatureInterface.GetAnim().Dash(true);
            CreatureInterface.GetRb2D().UseGravity(false);
            CreatureInterface.GetRb2D().ResetAll(); // 使Y軸不變
            CreatureInterface.AddForce_OnActive(_force, ForceMode2D.Impulse);
            d_0 = CreatureInterface.GetAbsolutePosition().x;
        }

        public void Update()
        {
            CreatureInterface.GetRb2D().instance.velocity =
                new Vector2(CreatureInterface.GetRb2D().Velocity.x, 0); // 強制霸體
        }

        public void Exit()
        {
        }

        public Func<bool> FilterIn { get; }
        public Func<bool> ToInterrupt { get; }
        public CreatureInterface CreatureInterface { get; set; }
    }
}