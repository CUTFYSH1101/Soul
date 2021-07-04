using UnityEngine;

namespace Main.Game.Anim
{
    public struct UnityAnimID
    {
        // 施放技能
        public static readonly int
            ToAttack = Animator.StringToHash("ToAttack"),
            ToNormalAttackSquare = Animator.StringToHash("ToNormalAttack.Square"),
            ToNormalAttackCircle = Animator.StringToHash("ToNormalAttack.Circle"),
            ToNormalAttackCross = Animator.StringToHash("ToNormalAttack.Cross"),
            DiveAttacking = Animator.StringToHash("DiveAttacking.U"),
            DiveAttackingSquare = Animator.StringToHash("DiveAttacking.Square"),
            DiveAttackingCircle = Animator.StringToHash("DiveAttacking.Circle"),
            DiveAttackingCross = Animator.StringToHash("DiveAttacking.Cross"),
            ToSpurAttackSquare = Animator.StringToHash("ToSpurAttack.Square"),
            ToSpurAttackCircle = Animator.StringToHash("ToSpurAttack.Circle"),
            ToSpurAttackCross = Animator.StringToHash("ToSpurAttack.Cross");

        // 移動
        public static readonly int
            Moving = Animator.StringToHash("Moving"),
            Speed = Animator.StringToHash("Speed"),
            Dashing = Animator.StringToHash("Dashing"),
            Up = Animator.StringToHash("Up"),
            Down = Animator.StringToHash("Down");

        // 精神狀況
        public static readonly int
            Alive = Animator.StringToHash("Alive"),
            Knockbacked = Animator.StringToHash("Knockbacked");
    }
}