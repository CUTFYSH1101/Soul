using System;
using JetBrains.Annotations;
using Main.EventLib.Main.EventSystem.Main;
using Main.Entity.Creature;
using Main.EventLib.Common;
using Main.EventLib.Main.EventSystem.EventOrderbySystem;
using Main.EventLib.Main.EventSystem.Main.Interface;
using Main.EventLib.Main.EventSystem.Main.Sub.Interface;
using Main.EventLib.Sub.CreatureEvent.Skill;
using Main.Game;
using Main.Res.CharactersRes.Animations.Scripts;

// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable PossibleNullReferenceException
namespace Main.EventLib.Sub.CreatureEvent.MoveEvent
{
    public class MovingEvent : AbsEventObject, IEvent3, IWorkOnCreature
    {
        private readonly Func<int> _axisInput;

        private readonly (CreatureAnimInterface anim, ProxyUnityRb2D rb2D, CreatureAttr attr, Func<bool> IsFacingRight)
            _obj;

        public MovingEvent(Creature creature, [NotNull] string axisInputKey)
        {
            this.Build(creature, EnumOrder.Move, EnumCreatureEventTag.Move);

            var @interface = CreatureInterface;
            _obj.anim = @interface.GetAnim();
            _obj.rb2D = @interface.GetRb2D();
            _obj.attr = @interface.GetAttr();
            _obj.IsFacingRight = () => @interface.IsFacingRight;

            _axisInput = () => Input.Input.GetAxisRaw(axisInputKey);
            FilterIn = () =>
                CreatureInterface.MovableDyn;
            // 鬆開按鍵時，還站在地面上，則停止
            ToInterrupt = () =>
                _axisInput() == 0 && @interface.Grounded ||
                !CreatureInterface.MovableDyn;
            PreWork += () => _obj.attr.MindState = EnumMindState.Moving;
            PostWork += () => _obj.attr.MindState = EnumMindState.Idle;
            // 注意必須在其他事件觸發以前，避免重複設定
            FinalAct += StopMove;
            /*FinalAct += () =>
            {
                _obj.anim.Move(false);
                if (@interface.Grounded) _obj.rb2D.ActiveX = 0;
            };*/
        }

        public void MoveUpdate()
        {
            if (_axisInput() == 0)
                return;

            if (!_obj.attr.EnableMoveDyn)
                return;

            Director.CreateEvent();
        }

        public void Enter()
        {
        }

        public void Update()
        {
            if (!_obj.attr.Grounded)
                StopMove();
            else
                Move();
        }

        private void StopMove()
        {
            _obj.anim.Move(false);
            if (CreatureInterface.Grounded) _obj.rb2D.ActiveX = 0;
            // _obj.rb2D.ActiveX = 0;
            _obj.attr.MindState = EnumMindState.Idle;
        }

        private void Move()
        {
            _obj.anim.Move(_axisInput() != 0);
            _obj.rb2D.ActiveX = Math.Sign(_axisInput()) * _obj.attr.MoveSpeed;
            _obj.attr.MindState = EnumMindState.Moving;
        }

        public void Exit()
        {
        }

        public Func<bool> FilterIn { get; }
        public Func<bool> ToInterrupt { get; }
        public CreatureInterface CreatureInterface { get; set; }
    }
    /*public class MovingEvent : AbstractCreatureEventA
    {
        private readonly Func<int> _axisInput;

        private readonly (CreatureAnimManager anim, ProxyUnityRb2D rb2D, CreatureAttr attr, Func<bool> IsFacingRight)
            _obj;

        public MovingEvent(Creature creature, [NotNull] string axisInputKey) : base(creature)
        {
            var @interface = CreatureInterfaceForSkill;
            _obj.anim = @interface.GetAnimManager();
            _obj.rb2D = @interface.GetRigidbody2D();
            _obj.attr = @interface.GetCreatureAttr();
            _obj.IsFacingRight = () => @interface.IsFacingRight;

            _axisInput = () => Input.Input.GetAxisRaw(axisInputKey);
            /*
            CauseEnter = () =>
                _axisInput() != 0);
            #1#
            // 鬆開按鍵時，還站在地面上，則停止
            CauseExit = () =>
                _axisInput() == 0 && @interface.Grounded);
            PreWork += () => _obj.attr.MindState = EnumMindState.Moving;
            PostWork += () => _obj.attr.MindState = EnumMindState.Idle;
            // 注意必須在其他事件觸發以前，避免重複設定
            FinalEvent += () =>
            {
                _obj.anim.Move(false);
                if (@interface.Grounded) _obj.rb2D.ActiveX = 0;
            };
            InitCreatureEventOrder(EnumCreatureEventTag.Move, EnumOrder.Move);
        }

        public void MoveUpdate()
        {
            if (_axisInput() == 0)
                return;

            if (!_obj.attr.EnableMoveDyn)
                return;

            base.Invoke();
        }

        protected override void Enter()
        {
        }

        protected override void Update()
        {
            if (!_obj.attr.Grounded)
            {
                _obj.anim.Move(false);
                // _obj.rb2D.ActiveX = 0;
                _obj.attr.MindState = EnumMindState.Idle;
            }
            else
            {
                _obj.anim.Move(_axisInput() != 0);
                _obj.rb2D.ActiveX = Math.Sign(_axisInput()) * _obj.attr.MoveSpeed;
                _obj.attr.MindState = EnumMindState.Moving;
            }

            /*if (!_obj.attr.Grounded && !_obj.IsFacingRight() && _axisInput() > 0)
                _obj.rb2D.ActiveX *= -1; // flip
            if (!_obj.attr.Grounded && _obj.IsFacingRight() && _axisInput() < 0)
                _obj.rb2D.ActiveX *= -1;#1#
        }

        protected override void Exit()
        {
            FinalEvent?.Invoke();
        }
    }*/
}