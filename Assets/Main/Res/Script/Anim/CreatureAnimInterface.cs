using System;
using Main.Blood;
using Main.Res.Script;
using UnityEngine;

namespace Main.Res.CharactersRes.Animations.Scripts
{
    /// 部分與animator相關的，包含attack、move、jump、landing，不包含knockback
    public class CreatureAnimInterface
    {
        public bool IsTag(string tag) => _anim.IsTag(tag);
        private readonly Animator _anim;
        private readonly MoveAnim _moveAnim;
        private readonly MindStateAnim _mindStateAnim;
        private readonly AttackAnim _attackAnim;
        public float GetCurrentAnimationLength
        {
            get
            {
                try
                {
                    return _anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        public CreatureAnimInterface(Animator animator)
        {
            _anim = animator;
            _moveAnim = new MoveAnim(animator);
            _mindStateAnim = new MindStateAnim(animator);
            _attackAnim = new AttackAnim(animator);
        }

        public void SetAtkSpeed(float value) => _attackAnim.SetAtkSpeed(value);
        public void Knockback(bool @switch) => _mindStateAnim.Knockback(@switch);
        public void AtkDiveCrash(bool @switch) => _attackAnim.DiveCrash(@switch);
        public void Landed(bool @switch) => _attackAnim.Landed(@switch);
        public void AtkSpur(BloodType type) => _attackAnim.AtkSpur(type);
        public void AtkNormal(BloodType type) => _attackAnim.AtkNormal(type);
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

        public void AtkJump(BloodType shape) => _attackAnim.AtkJump(shape);
        public void Killed() => _mindStateAnim.Killed();
        public void Revival() => _mindStateAnim.Revival();
        public void Interrupt() => _anim.SetTrigger(UnityAnimID.ToInterruptAnimation);
    }
}