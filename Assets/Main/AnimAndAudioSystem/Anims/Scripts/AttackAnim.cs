using Main.AnimAndAudioSystem.Main.Common;
using UnityEngine;

namespace Main.AnimAndAudioSystem.Anims.Scripts
{
    /// 根據攻擊符號，出現對應動畫
    internal class AttackAnim
    {
        private readonly Animator animator;
        private readonly bool hasDirect, hasChoice;
        private readonly bool hasDirectDive, hasChoiceDive;
        private readonly bool hasChoiceSpur;
        private readonly bool hasChoiceJump;
        private readonly bool hasAttackSpeed;

        public AttackAnim(Animator animator)
        {
            this.animator = animator;
            hasDirect = animator.HasParameter(UnityAnimID.ToAttack);
            hasChoice = animator.HasParameter(UnityAnimID.ToNormalAttackSquare);
            hasDirectDive = animator.HasParameter(UnityAnimID.DiveAttacking);
            hasChoiceDive = animator.HasParameter(UnityAnimID.DiveAttackingSquare);
            hasChoiceSpur = animator.HasParameter(UnityAnimID.ToSpurAttackSquare);
            hasChoiceJump = animator.HasParameter(UnityAnimID.ToJumpAttackSquare);
            hasAttackSpeed = animator.HasParameter(UnityAnimID.AttackSpeed);
        }

        private void TriggerTemplate(bool hasSymbol, EnumSymbol symbol, int id)
        {
            if (!hasSymbol)
            {
                Debug.LogError("動畫機不含有" + symbol);
                return;
            }

            animator.SetTrigger(id);
        }

        public void NormalAttack(EnumSymbol symbol)
        {
            switch (symbol)
            {
                case EnumSymbol.Direct:
                    TriggerTemplate(hasDirect, symbol, UnityAnimID.ToAttack);
                    break;
                case EnumSymbol.Square:
                    TriggerTemplate(hasChoice, symbol, UnityAnimID.ToNormalAttackSquare);
                    break;
                case EnumSymbol.Circle:
                    TriggerTemplate(hasChoice, symbol, UnityAnimID.ToNormalAttackCircle);
                    break;
                case EnumSymbol.Cross:
                    TriggerTemplate(hasChoice, symbol, UnityAnimID.ToNormalAttackCross);
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

            animator.SetBool(id, @switch);
        }

        private void FloatTemplate(bool hasVar, int id, float value)
        {
            if (!hasVar)
            {
                Debug.LogError("動畫機不含有該參數");
                return;
            }

            animator.SetFloat(id, value);
        }

        public void DiveAttack(EnumSymbol symbol, bool @switch)
        {
            switch (symbol)
            {
                case EnumSymbol.Direct:
                    BoolTemplate(hasDirectDive, symbol, UnityAnimID.DiveAttacking, @switch);
                    break;
                case EnumSymbol.Square:
                    BoolTemplate(hasChoiceDive, symbol, UnityAnimID.DiveAttackingSquare, @switch);
                    break;
                case EnumSymbol.Circle:
                    BoolTemplate(hasChoiceDive, symbol, UnityAnimID.DiveAttackingCircle, @switch);
                    break;
                case EnumSymbol.Cross:
                    BoolTemplate(hasChoiceDive, symbol, UnityAnimID.DiveAttackingCross, @switch);
                    break;
                default:
                    Debug.LogError("超出範圍");
                    return;
            }
        }

        public void SpurAttack(EnumSymbol symbol)
        {
            switch (symbol)
            {
                case EnumSymbol.Square:
                    TriggerTemplate(hasChoiceSpur, symbol, UnityAnimID.ToSpurAttackSquare);
                    break;
                case EnumSymbol.Circle:
                    TriggerTemplate(hasChoiceSpur, symbol, UnityAnimID.ToSpurAttackCircle);
                    break;
                case EnumSymbol.Cross:
                    TriggerTemplate(hasChoiceSpur, symbol, UnityAnimID.ToSpurAttackCross);
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
                    TriggerTemplate(hasChoice, symbol, UnityAnimID.ToJumpAttackSquare);
                    break;
                case EnumSymbol.Circle:
                    TriggerTemplate(hasChoice, symbol, UnityAnimID.ToJumpAttackCross);
                    break;
                case EnumSymbol.Cross:
                    TriggerTemplate(hasChoice, symbol, UnityAnimID.ToJumpAttackCircle);
                    break;
                default:
                    Debug.LogError("超出範圍");
                    return;
            }
        }

        public void SetAttackSpeed(float value)
        {
            FloatTemplate(hasAttackSpeed, UnityAnimID.AttackSpeed, value);
        }
    }
}