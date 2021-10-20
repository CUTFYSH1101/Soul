using System;
using Main.EventLib.Main.EventSystem.Main;
using Main.Entity.Creature;
using Main.EventLib.Common;
using Main.EventLib.Main.EventSystem;
using Main.EventLib.Main.EventSystem.EventOrderbySystem;
using Main.EventLib.Main.EventSystem.Main.Interface;
using Main.EventLib.Main.EventSystem.Main.Sub.Interface;
using Main.EventLib.Sub.CreatureEvent.Skill;
using Main.Game;
using Main.Res.CharactersRes.Animations.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main.EventLib.Sub.CreatureEvent.MoveEvent
{
    public class FlyToEvent : AbsEventObject, IEvent3, IWorkOnCreature
    {
        private readonly (CreatureAnimInterface anim, ProxyUnityRb2D rb2D, CreatureAttr attr) _obj;
        private readonly CreatureInterface _interface;
        private Vector2 _target, _self;
        private Vector2 _distance;
        private const float AxisXDistanceToStopMove = 0.1f;
        private const float TryJumpIfAboveDifferenceHeight = 1.5f;
        private const float Offset = 0.5f;
        private readonly float _flySpeed;

        public FlyToEvent(Creature creature)
        {
            this.Build(creature, EnumOrder.Move, EnumCreatureEventTag.Move);

            _interface = CreatureInterface;
            _obj.anim = _interface.GetAnim();
            _obj.rb2D = _interface.GetRb2D();
            _obj.attr = _interface.GetAttr();

            PreWork += () => _obj.attr.MindState = EnumMindState.Moving;
            PostWork += () => _obj.attr.MindState = EnumMindState.Idle;
            // 注意必須在其他事件觸發以前，避免重複設定
            FinalAct += () =>
            {
                _obj.anim.Move(false);
                _obj.rb2D.ResetAll();
            };
            // 角色速度增加些微隨機性，減少重疊狀況
            _flySpeed = _obj.attr.MoveSpeed + Random.Range(0.0f, Offset);
            FilterIn = () =>
                Math.Abs(_distance.x) > AxisXDistanceToStopMove + Random.Range(0.0f, Offset);
            ToInterrupt = () => 
                Math.Abs(_distance.x) < AxisXDistanceToStopMove + Random.Range(0.0f, Offset);
        }

        public void Invoke(Vector2 target)
        {
            if (State != EnumState.Free) return;

            _distance = (target - _self);
            _target = target;
            Director.CreateEvent();
        }

        public void Enter()
        {
        }

        public void Update()
        {
            _self = _interface.GetAbsolutePosition();
            _distance = _target - _self;
            if (_obj.attr.Grounded)
            {
                // Debug.Log("追阿");
                _obj.anim.Move(true);
                _obj.rb2D.Velocity = _distance.normalized * _flySpeed;
            }
        }

        public void Exit()
        {
        }

        public Func<bool> FilterIn { get; }
        public Func<bool> ToInterrupt { get; }
        public CreatureInterface CreatureInterface { get; set; }
    }
}