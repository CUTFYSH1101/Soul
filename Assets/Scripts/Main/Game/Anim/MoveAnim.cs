using UnityEngine;
using static Main.Game.Anim.UnityAnimID;

namespace Main.Game.Anim
{
    /// 避免float和bool的起衝突...協調器
    public class MoveAnim
    {
        private readonly Animator animator;

        private enum Type
        {
            Float,
            Bool
        }

        private readonly Type type;

        private enum Dir
        {
            Up,
            Down,
            Stay
        }

        private Dir dir;
        private readonly bool hasAnim;
        public bool UseGroundedChecker => hasAnim; // 作為判斷要不要使用Grounded的一個工具

        public MoveAnim(Animator animator)
        {
            this.animator = animator;
            type = animator.HasParameter(Moving) ? Type.Bool : Type.Float;
            hasAnim = animator.HasParameter(Up); // 確認有沒有anim
        }

        public void Dash(bool @switch)
            => animator.SetBool(Dashing, @switch);

        public void Move(bool @switch)
        {
            switch (type)
            {
                case Type.Bool:
                    animator.SetBool(Moving, @switch);
                    break;
                case Type.Float:
                    animator.SetFloat(Speed, @switch ? 1 : 0);
                    break;
            }
        }

        public void JumpUpdate(int speedY)
        {
            if (!hasAnim)
                return;

            if (speedY > .1)
                dir = Dir.Up;
            else if (speedY < -.1)
                dir = Dir.Down;
            else
                dir = Dir.Stay;

            switch (dir)
            {
                case Dir.Up:
                    animator.SetBool(Up, true);
                    animator.SetBool(Down, false);
                    break;
                case Dir.Down:
                    animator.SetBool(Up, false);
                    animator.SetBool(Down, true);
                    break;
                case Dir.Stay:
                    animator.SetBool(Up, false);
                    animator.SetBool(Down, false);
                    break;
            }
        }
    }
}