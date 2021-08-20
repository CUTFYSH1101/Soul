using System;
using Main.AnimAndAudioSystem.Audios.Scripts;
using Main.Entity.Creature;
using Main.EventSystem.Event.CreatureEventSystem.Skill;
using Main.Game.Collision;

namespace Main.EventSystem.Event.CreatureEventSystem.MoveEvent
{
    public class JumpOrWallJump : IInterruptible
    {
        private readonly CreatureInterface _creatureInterface;
        private readonly CollisionManager.TouchTheWallEvent _triggerEvent;
        private (Action jump, Action<int?> wallJump) _actions;
        private bool _switch = true;

        public JumpOrWallJump(AbstractCreature creature)
        {
            _creatureInterface = new CreatureInterface(creature);
            _triggerEvent = new CollisionManager.TouchTheWallEvent(_creatureInterface.GetRigidbody2D());
        }

        public JumpOrWallJump InitAction(Action jump, Action<int?> wallJump)
        {
            _actions.wallJump = wallJump;
            _actions.jump = jump;
            return this;
        }

        private bool TouchTheWall() => _triggerEvent.IsTriggerStay;

        /// wallJump + jump
        public void Invoke()
        {
            if (!Switch) return;

            // 排除牆角邊，只有空中碰牆才能蹬牆跳
            if (TouchTheWall() && !_creatureInterface.Grounded)
            {
                var dir = Math.Sign(_creatureInterface.GetAbsolutePosition().x - _triggerEvent.GetLeanOnWallPos().x);
                _actions.wallJump?.Invoke(dir);
                _creatureInterface.Play(DictionaryAudioPlayer.Key.WallJump);
            }
            else
            {
                _actions.jump?.Invoke();
            }
        }

        public bool Switch
        {
            get => _switch;
            set
            {
                if (!_switch)
                {
                    _creatureInterface.GetAnimManager().Interrupt();
                    // 是否要停止rb2d.x？
                }

                _switch = value;
            }
        }
    }
}