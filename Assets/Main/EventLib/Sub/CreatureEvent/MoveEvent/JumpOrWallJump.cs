using System;
using Main.Entity.Creature;
using Main.EventLib.Condition;
using Main.EventLib.Main.EventSystem;
using Main.EventLib.Sub.CreatureEvent.Skill;
using Main.Game.Collision.Event;
using Main.Res.Script.Audio;

namespace Main.EventLib.Sub.CreatureEvent.MoveEvent
{
    public class JumpOrWallJump : IInterruptible
    {
        private readonly CreatureInterface _creatureInterface;
        private readonly CollisionStayWall _triggerEvent;
        private (Action jump, Action<int?> wallJump) _actions;
        private bool _switch = true;
        private CdCondition _cdCondition = new(0);

        public float CdTime
        {
            get => _cdCondition.Lag;
            set => _cdCondition = new CdCondition(value);
        }

        public JumpOrWallJump(Creature creature)
        {
            _creatureInterface = new CreatureInterface(creature);
            _triggerEvent = new CollisionStayWall(_creatureInterface.GetRb2D());
        }

        public JumpOrWallJump InitAction(Action jump, Action<int?> wallJump)
        {
            _actions.wallJump = wallJump;
            _actions.jump = jump;
            return this;
        }

        private bool TouchTheWall() => _triggerEvent.IsTrigger;

        /// wallJump + jump
        public void Invoke()
        {
            if (!IsOpen) return;

            if (CdTime != 0 && !_cdCondition.AndCause()) return;
            _cdCondition.Reset();

            if (!_creatureInterface.MovableDyn) return;
            

            // 排除牆角邊，只有空中碰牆才能蹬牆跳
            if (TouchTheWall() && !_creatureInterface.Grounded)
            {
                var dir = Math.Sign(_creatureInterface.GetAbsolutePosition().x -
                                    _triggerEvent.GetLeanOnWallPos().x);
                _actions.wallJump?.Invoke(dir);
                _creatureInterface.Play(DictAudioPlayer.Key.WallJump);
            }
            else
            {
                _actions.jump?.Invoke();
            }
        }

        public bool IsOpen
        {
            get => _switch;
            set
            {
                if (!_switch)
                {
                    _creatureInterface.GetAnim().Interrupt();
                    // 是否要停止rb2d.x？
                }

                _switch = value;
            }
        }
    }
}