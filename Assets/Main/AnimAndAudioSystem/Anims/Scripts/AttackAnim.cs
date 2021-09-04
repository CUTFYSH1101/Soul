using Main.AnimAndAudioSystem.Main.Common;
using UnityEngine;

namespace Main.AnimAndAudioSystem.Anims.Scripts
{
    /// 根據攻擊符號，出現對應動畫
    internal class AttackAnim
    {
        private readonly Animator _animator;
        private readonly bool _hasDirect, _hasChoice;
        private readonly bool _hasDirectDive, _hasChoiceDive;
        private readonly bool _hasChoiceSpur;
        private readonly bool _hasChoiceJump;
        private readonly bool _hasAttackSpeed;

        public AttackAnim(Animator animator)
        {
            _animator = animator;
            _hasDirect = animator.HasParameter(UnityAnimID.ToAttack);
            _hasChoice = animator.HasParameter(UnityAnimID.ToNormalAttackSquare);
            _hasDirectDive = animator.HasParameter(UnityAnimID.DiveAttacking);
            _hasChoiceDive = animator.HasParameter(UnityAnimID.DiveAttackingSquare);
            _hasChoiceSpur = animator.HasParameter(UnityAnimID.ToSpurAttackSquare);
            _hasChoiceJump = animator.HasParameter(UnityAnimID.ToJumpAttackSquare);
            _hasAttackSpeed = animator.HasParameter(UnityAnimID.AttackSpeed);
        }

        private void TriggerTemplate(bool hasSymbol, EnumSymbol symbol, int id)
        {
            if (!hasSymbol)
            {
                Debug.LogError("動畫機不含有" + symbol);
                return;
            }

            _animator.SetTrigger(id);
        }

        public void NormalAttack(EnumSymbol symbol)
        {
            switch (symbol)
            {
                case EnumSymbol.Direct:
                    TriggerTemplate(_hasDirect, symbol, UnityAnimID.ToAttack);
                    break;
                case EnumSymbol.Square:
                    TriggerTemplate(_hasChoice, symbol, UnityAnimID.ToNormalAttackSquare);
                    break;
                case EnumSymbol.Circle:
                    TriggerTemplate(_hasChoice, symbol, UnityAnimID.ToNormalAttackCircle);
                    break;
                case EnumSymbol.Cross:
                    TriggerTemplate(_hasChoice, symbol, UnityAnimID.ToNormalAttackCross);
                    break;
                default:
                    Debug.LogError("超出範圍");
                    return;
            }
        }

        private void BoolTemplate(bool hasSymbol, EnumSymbol symbol, int id, bool @switch)
        {
            if (!hasSymbol)
            {
                Debug.LogError("動畫機不含有" + symbol);
                return;
            }

            _animator.SetBool(id, @switch);
        }

        private void FloatTemplate(bool hasVar, int id, float value)
        {
            if (!hasVar)
            {
                Debug.LogError("動畫機不含有該參數");
                return;
            }

            _animator.SetFloat(id, value);
        }

        public void DiveAttack(EnumSymbol symbol, bool @switch)
        {
            switch (symbol)
            {
                case EnumSymbol.Direct:
                    BoolTemplate(_hasDirectDive, symbol, UnityAnimID.DiveAttacking, @switch);
                    break;
                case EnumSymbol.Square:
                    BoolTemplate(_hasChoiceDive, symbol, UnityAnimID.DiveAttackingSquare, @switch);
                    break;
                case EnumSymbol.Circle:
                    BoolTemplate(_hasChoiceDive, symbol, UnityAnimID.DiveAttackingCircle, @switch);
                    break;
                case EnumSymbol.Cross:
                    BoolTemplate(_hasChoiceDive, symbol, UnityAnimID.DiveAttackingCross, @switch);
                    break;
                default:
                    Debug.Log(symbol);
                    Debug.LogError("超出範圍");
                    return;
            }
        }
        
        public void ExitDiveAttack(bool @switch) => _animator.SetBool(UnityAnimID.DiveAttackExit,@switch);

        public void SpurAttack(EnumSymbol symbol)
        {
            switch (symbol)
            {
                case EnumSymbol.Square:
                    TriggerTemplate(_hasChoiceSpur, symbol, UnityAnimID.ToSpurAttackSquare);
                    break;
                case EnumSymbol.Circle:
                    TriggerTemplate(_hasChoiceSpur, symbol, UnityAnimID.ToSpurAttackCircle);
                    break;
                case EnumSymbol.Cross:
                    TriggerTemplate(_hasChoiceSpur, symbol, UnityAnimID.ToSpurAttackCross);
                    break;
                default:
                    Debug.LogError("超出範圍");
                    return;
            }
        }

        public void JumpAttack(EnumSymbol symbol)
        {
            switch (symbol)
            {
                case EnumSymbol.Square:
                    TriggerTemplate(_hasChoice, symbol, UnityAnimID.ToJumpAttackSquare);
                    break;
                case EnumSymbol.Circle:
                    TriggerTemplate(_hasChoice, symbol, UnityAnimID.ToJumpAttackCircle);
                    break;
                case EnumSymbol.Cross:
                    TriggerTemplate(_hasChoice, symbol, UnityAnimID.ToJumpAttackCross);
                    break;
                default:
                    Debug.LogError("超出範圍");
                    return;
            }
        }

        public void SetAttackSpeed(float value)
        {
            FloatTemplate(_hasAttackSpeed, UnityAnimID.AttackSpeed, value);
        }
    }
}