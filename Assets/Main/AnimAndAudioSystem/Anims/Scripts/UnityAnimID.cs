using UnityEngine;

namespace Main.AnimAndAudioSystem.Anims.Scripts
{
    internal struct UnityAnimID
    {
        // 施放技能
        public static readonly int
            ToAttack = Animator.StringToHash("ToAttack"),
            ToJumpAttackSquare = Animator.StringToHash("ToJumpAttack.Square"),
            ToJumpAttackCross = Animator.StringToHash("ToJumpAttack.Cross"),
            ToJumpAttackCircle = Animator.StringToHash("ToJumpAttack.Circle"),
            ToNormalAttackSquare = Animator.StringToHash("ToNormalAttack.Square"),
            ToNormalAttackCross = Animator.StringToHash("ToNormalAttack.Cross"),
            ToNormalAttackCircle = Animator.StringToHash("ToNormalAttack.Circle"),
            DiveAttacking = Animator.StringToHash("DiveAttacking.U"),
            DiveAttackingSquare = Animator.StringToHash("DiveAttacking.Square"),
            DiveAttackingCross = Animator.StringToHash("DiveAttacking.Cross"),
            DiveAttackingCircle = Animator.StringToHash("DiveAttacking.Circle"),
            ToSpurAttackSquare = Animator.StringToHash("ToSpurAttack.Square"),
            ToSpurAttackCross = Animator.StringToHash("ToSpurAttack.Cross"),
            ToSpurAttackCircle = Animator.StringToHash("ToSpurAttack.Circle");

        // 移動
        public static readonly int
            Moving = Animator.StringToHash("Moving"),
            Speed = Animator.StringToHash("Speed"),
            Dashing = Animator.StringToHash("Dashing"),
            ToWallJump = Animator.StringToHash("ToWallJump"),
            Up = Animator.StringToHash("Up"),
            Down = Animator.StringToHash("Down");

        // 精神狀況
        public static readonly int
            Alive = Animator.StringToHash("Alive"),
            Knockbacked = Animator.StringToHash("Knockbacked");

        public static readonly int
            AttackSpeed = Animator.StringToHash("AttackSpeed");
    }
}