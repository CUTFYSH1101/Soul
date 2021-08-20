using Main.AnimAndAudioSystem.Main.Common;
using UnityEngine;

namespace Main.AnimAndAudioSystem.Anims.Scripts
{
    /// 部分與animator相關的，包含attack、move、jump、landing，不包含knockback
    public class CreatureAnimManager
    {
        public bool IsTag(string tag) => anim.IsTag(tag);
        private readonly Animator anim;
        private readonly MoveAnim moveAnim;
        private readonly MindStateAnim mindStateAnim;
        private readonly AttackAnim attackAnim;

        public CreatureAnimManager(Animator animator)
        {
            anim = animator;
            moveAnim = new MoveAnim(animator);
            mindStateAnim = new MindStateAnim(animator);
            attackAnim = new AttackAnim(animator);
        }

        public void SetAttackSpeed(float value) => attackAnim.SetAttackSpeed(value);
        public void Knockback(bool @switch) => mindStateAnim.Knockback(@switch);
        public void DiveAttack(EnumSymbol type, bool @switch) => attackAnim.DiveAttack(type, @switch);
        public void SpurAttack(EnumSymbol type) => attackAnim.SpurAttack(type);
        public void Attack(EnumSymbol type) => attackAnim.NormalAttack(type);
        public void Move(bool @switch) => moveAnim.Move(@switch);
        public void Dash(bool @switch) => moveAnim.Dash(@switch);

        public void JumpUpdate(int speedY)
        {
            if (moveAnim.UseGroundedChecker)
                moveAnim.JumpUpdate(speedY);
        }
        public void WallJump() => moveAnim.WallJump();
        public void JumpAttack(EnumSymbol symbol) => attackAnim.JumpAttack(symbol);
        public void Killed() => mindStateAnim.Killed();
        public void Revival() => mindStateAnim.Revival();
    }
}