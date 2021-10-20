using System;
using Main.Res.Script;
using UnityEngine;

namespace Main.Res.CharactersRes.Animations.Scripts
{
    /// 根據攻擊符號，出現對應動畫
    internal class AttackAnim
    {
        private readonly Animator _animator;
        private readonly bool _hasDirect, _hasChoice;
        private readonly bool _hasDiveCrash, _hasDiveLanded;
        private readonly bool _hasChoiceSpur;
        private readonly bool _hasChoiceJump;
        private readonly bool _hasAttackSpeed;

        public AttackAnim(Animator animator)
        {
            _animator = animator;
            _hasDirect = animator.HasParameter(UnityAnimID.ToAttack);
            _hasChoice = animator.HasParameter(UnityAnimID.ToNormalAttackSquare);
            _hasDiveCrash = animator.HasParameter(UnityAnimID.IsAtkDiveCrash);
            _hasDiveLanded = animator.HasParameter(UnityAnimID.IsAtkDiveLanded);
            _hasChoiceSpur = animator.HasParameter(UnityAnimID.ToSpurAttackSquare);
            _hasChoiceJump = animator.HasParameter(UnityAnimID.ToJumpAttackSquare);
            _hasAttackSpeed = animator.HasParameter(UnityAnimID.AttackSpeed);
        }

        private static void ThrowException() => throw new Exception("動畫機不含有該參數");

        // throw new ArgumentOutOfRangeException(nameof(hasParam), false, null);
        private void TriggerTemplate(bool hasParam, int id)
        {
            if (!hasParam) ThrowException();
            _animator.SetTrigger(id);
        }

        private void BoolTemplate(bool hasParam, int id, bool @switch)
        {
            if (!hasParam) ThrowException();
            _animator.SetBool(id, @switch);
        }

        private void FloatTemplate(bool hasParam, int id, float value)
        {
            if (!hasParam) ThrowException();
            _animator.SetFloat(id, value);
        }

        public void AtkNormal(EnumShape shape)
        {
            switch (shape)
            {
                case EnumShape.Direct:
                    TriggerTemplate(_hasDirect, UnityAnimID.ToAttack);
                    break;
                case EnumShape.Square:
                    TriggerTemplate(_hasChoice, UnityAnimID.ToNormalAttackSquare);
                    break;
                case EnumShape.Circle:
                    TriggerTemplate(_hasChoice, UnityAnimID.ToNormalAttackCircle);
                    break;
                case EnumShape.Cross:
                    TriggerTemplate(_hasChoice, UnityAnimID.ToNormalAttackCross);
                    break;
                default:
                    Debug.LogError("超出範圍");
                    return;
            }
        }


        public void DiveCrash(bool @switch) => BoolTemplate(_hasDiveCrash,UnityAnimID.IsAtkDiveCrash, @switch);

        public void Landed(bool @switch) => BoolTemplate(_hasDiveLanded,UnityAnimID.IsAtkDiveLanded, @switch);

        public void AtkSpur(EnumShape shape)
        {
            switch (shape)
            {
                case EnumShape.Square:
                    TriggerTemplate(_hasChoiceSpur, UnityAnimID.ToSpurAttackSquare);
                    break;
                case EnumShape.Circle:
                    TriggerTemplate(_hasChoiceSpur, UnityAnimID.ToSpurAttackCircle);
                    break;
                case EnumShape.Cross:
                    TriggerTemplate(_hasChoiceSpur, UnityAnimID.ToSpurAttackCross);
                    break;
                default:
                    Debug.LogError("超出範圍");
                    return;
            }
        }

        public void AtkJump(EnumShape shape)
        {
            switch (shape)
            {
                case EnumShape.Square:
                    TriggerTemplate(_hasChoice, UnityAnimID.ToJumpAttackSquare);
                    break;
                case EnumShape.Circle:
                    TriggerTemplate(_hasChoice, UnityAnimID.ToJumpAttackCircle);
                    break;
                case EnumShape.Cross:
                    TriggerTemplate(_hasChoice, UnityAnimID.ToJumpAttackCross);
                    break;
                default:
                    Debug.LogError("超出範圍");
                    return;
            }
        }
        public void SetAtkSpeed(float value) => FloatTemplate(_hasAttackSpeed, UnityAnimID.AttackSpeed, value);
    }
}