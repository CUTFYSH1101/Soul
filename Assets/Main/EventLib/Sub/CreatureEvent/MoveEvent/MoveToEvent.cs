using System;
using System.Diagnostics.CodeAnalysis;
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

/*
1.確認與目標的距離，必須大於0.1才表示有要追
2.隨時更新與目標的距離，當小於0.1代表已經到達目標位置，因此停止移動
3.除非移動到目標點，否則不停止
3-1.update: 在空中，繼續追，不停止guildX，不關閉anim(因為沒有跳躍動畫)，mindState = Move
3-2.update: 在地面，繼續追，自由控制guildX
4.exit: anim關閉，mindState = Idle，guildX關閉
guildX如果是空中就不停止(但是一落地就要停)->
 */

/*
 * 1.cause == true, enter
 * 2.cause == false, prevent enter
 * 3.after event, set cause true, allow invoke event again
 */
namespace Main.EventLib.Sub.CreatureEvent.MoveEvent
{
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class MoveToEvent : AbsEventObject, IEvent3, IWorkOnCreature
    {
        private readonly (CreatureAnimInterface anim, ProxyUnityRb2D rb2D, CreatureAttr attr) _obj;
        private readonly CreatureInterface _interface;
        private Vector2 _target, _self;
        private Vector2 _distance;
        private const float AxisXDistanceToStopMove = 0.1f;
        private const float TryJumpIfAboveDifferenceHeight = 1.5f;
        private const float Offset = 0.5f;
        private readonly float _moveSpeed;
        private ProxyUnityRb2D _rb;

        public MoveToEvent(Creature creature)
        {
            this.Build(creature, EnumOrder.Move, EnumCreatureEventTag.Move);
            EventAttr = new EventAttr(0.1f);

            _interface = CreatureInterface;
            _obj.anim = _interface.GetAnim();
            _obj.rb2D = _interface.GetRb2D();
            _obj.attr = _interface.GetAttr();

            PreWork += () => _obj.attr.MindState = EnumMindState.Moving;
            PostWork += () => _obj.attr.MindState = EnumMindState.Idle;
            // 注意必須在其他事件觸發以前，避免重複設定
            FinalAct += StopMove;

            // 角色速度增加些微隨機性，減少重疊狀況
            _moveSpeed = _obj.attr.MoveSpeed + Random.Range(0.0f, Offset);
            FilterIn = () =>
                !HadArrivedTarget &&
                CreatureInterface.MovableDyn;
            ToInterrupt = () =>
                HadArrivedTarget ||
                !CreatureInterface.MovableDyn;
        }
        // 遊戲初始化時，距離為0，因此moveTo在一開始無效
        private bool HadArrivedTarget => Math.Abs(_distance.x) <= AxisXDistanceToStopMove + Random.Range(0.0f, Offset);

        private void StopMove()
        {
            _obj.anim.Move(false);
            _obj.rb2D.GuideX = 0;
            // if (_interface.Grounded) _obj.rb2D.GuideX = 0;
        }

        public void Invoke(Vector2 target)
        {
            if (State != EnumState.Free) return;

            _distance = target - _self;
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
                UpdateMove();
            /*
                Debug.Log($"目標:{_target}" +
                          $"距離:{_distance} " +
                          $"速度:{_obj.rb2D.ToString()} ");
            */
        }

        private void UpdateMove()
        {
            _obj.anim.Move(true);
            _obj.rb2D.MoveTo(_target, _moveSpeed,
                _obj.attr.JumpForce,
                TryJumpIfAboveDifferenceHeight);
        }

        public void Exit()
        {
        }

        public Func<bool> FilterIn { get; }
        public Func<bool> ToInterrupt { get; }
        public CreatureInterface CreatureInterface { get; set; }
    }
}