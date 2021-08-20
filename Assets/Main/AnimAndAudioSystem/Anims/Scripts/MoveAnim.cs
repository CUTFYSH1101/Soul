using UnityEngine;
using static Main.AnimAndAudioSystem.Anims.Scripts.UnityAnimID;

namespace Main.AnimAndAudioSystem.Anims.Scripts
{
    /// 避免float和bool的起衝突...協調器
    internal class MoveAnim
    {
        private readonly Animator _animator;

        private enum Type
        {
            Float,
            Bool
        }

        private readonly Type _type;

        private enum Dir
        {
            Up,
            Down,
            Stay
        }

        private Dir _dir;
        private readonly bool _hasAnim;
        public bool UseGroundedChecker => _hasAnim; // 作為判斷要不要使用GroundedChecker的一個參數

        public MoveAnim(Animator animator)
        {
            _animator = animator;
            _type = animator.HasParameter(Moving) ? Type.Bool : Type.Float;
            _hasAnim = animator.HasParameter(Up); // 確認有沒有anim
        }

        public void Dash(bool @switch)
            => _animator.SetBool(Dashing, @switch);

        public void Move(bool @switch)
        {
            switch (_type)
            {
                case Type.Bool:
                    _animator.SetBool(Moving, @switch);
                    break;
                case Type.Float:
                    _animator.SetFloat(Speed, @switch ? 1 : 0);
                    break;
            }
        }

        public void JumpUpdate(int speedY)
        {
            if (!_hasAnim)
                return;

            if (speedY > .1)
                _dir = Dir.Up;
            else if (speedY < -.1)
                _dir = Dir.Down;
            else
                _dir = Dir.Stay;

            switch (_dir)
            {
                case Dir.Up:
                    _animator.SetBool(Up, true);
                    _animator.SetBool(Down, false);
                    break;
                case Dir.Down:
                    _animator.SetBool(Up, false);
                    _animator.SetBool(Down, true);
                    break;
                case Dir.Stay:
                    _animator.SetBool(Up, false);
                    _animator.SetBool(Down, false);
                    break;
            }
        }

        public void WallJump()
        {
            _animator.SetTrigger(ToWallJump);
        }
    }
}