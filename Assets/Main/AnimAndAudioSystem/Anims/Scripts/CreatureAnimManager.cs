using Main.AnimAndAudioSystem.Main.Common;
using Main.Entity;
using UnityEngine;

namespace Main.AnimAndAudioSystem.Anims.Scripts
{
    /// 部分與animator相關的，包含attack、move、jump、landing，不包含knockback
    public class CreatureAnimManager : IComponent
    {
        public bool IsTag(string tag) => _anim.IsTag(tag);
        private readonly Animator _anim;
        private readonly MoveAnim _moveAnim;
        private readonly MindStateAnim _mindStateAnim;
        private readonly AttackAnim _attackAnim;

        public CreatureAnimManager(Animator animator)
        {
            _anim = animator;
            _moveAnim = new MoveAnim(animator);
            _mindStateAnim = new MindStateAnim(animator);
            _attackAnim = new AttackAnim(animator);
        }

        public void SetAttackSpeed(float value) => _attackAnim.SetAttackSpeed(value);
        public void Knockback(bool @switch) => _mindStateAnim.Knockback(@switch);
        public void DiveAttack(EnumSymbol type, bool @switch) => _attackAnim.DiveAttack(type, @switch);
        public void ExitDiveAttack(bool @switch) => _attackAnim.ExitDiveAttack(@switch);
        public void SpurAttack(EnumSymbol type) => _attackAnim.SpurAttack(type);
        public void Attack(EnumSymbol type) => _attackAnim.NormalAttack(type);
        public void Move(bool @switch) => _moveAnim.Move(@switch);
        public void Dash(bool @switch) => _moveAnim.Dash(@switch);

        public void JumpUpdate(int speedY)
        {
            if (_moveAnim.UseGroundedChecker)
                _moveAnim.JumpUpdate(speedY);
        }

        public void WallJump() => _moveAnim.WallJump();

        public void WallJump(bool @switch)
        {
            if (@switch) _moveAnim.WallJump();
            else Interrupt();
        }

        public void JumpAttack(EnumSymbol symbol) => _attackAnim.JumpAttack(symbol);
        public void Killed() => _mindStateAnim.Killed();
        public void Revival() => _mindStateAnim.Revival();
        public void Interrupt() => _anim.SetTrigger(UnityAnimID.ToInterruptAnimation);
        public int Id { get; }

        public void Update()
        {
        }
    }
}