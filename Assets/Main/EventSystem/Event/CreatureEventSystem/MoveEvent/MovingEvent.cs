using System;
using JetBrains.Annotations;
using Main.AnimAndAudioSystem.Anims.Scripts;
using Main.Entity.Creature;
using Main.EventSystem.Common;
using Main.EventSystem.Event.CreatureEventSystem.Common;
using Main.EventSystem.Event.CreatureEventSystem.Decorator;
using Main.Game;

namespace Main.EventSystem.Event.CreatureEventSystem.MoveEvent
{
    public class MovingEvent : AbstractCreatureEventA
    {
        private readonly Func<int> _axisInput;
        private readonly (CreatureAnimManager anim, UnityRb2D rb2D, CreatureAttr attr) _obj;

        public MovingEvent(Creature creature, [NotNull] string axisInputKey) : base(creature)
        {
            var @interface = CreatureInterface;
            _obj.anim = @interface.GetAnimManager();
            _obj.rb2D = @interface.GetRigidbody2D();
            _obj.attr = @interface.GetCreatureAttr();

            _axisInput = () => Input.Input.GetAxisRaw(axisInputKey);
            /*
            CauseEnter = new FuncCause(() =>
                _axisInput() != 0);
            */
            // 鬆開按鍵時，還站在地面上，則停止
            CauseExit = new FuncCause(() =>
                _axisInput() == 0 && @interface.Grounded);
            PreWork += () => _obj.attr.MindState = EnumMindState.Move;
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

            if (!_obj.attr.MovableDyn)
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
                _obj.attr.MindState = EnumMindState.Move;
            }
        }

        protected override void Exit()
        {
            FinalEvent?.Invoke();
        }
    }
}