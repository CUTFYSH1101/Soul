using Main.Common;
using UnityEngine;

namespace Main.Game.Anim
{
    /// 根據攻擊符號，出現對應動畫
    public class AttackAnim
    {
        private readonly Animator animator;
        private readonly bool hasDirect, hasChoice;
        private readonly bool hasDirectDive, hasChoiceDive;
        private readonly bool hasChoiceSpur;
        private readonly bool hasChoiceJump;

        public AttackAnim(Animator animator)
        {
            this.animator = animator;
            hasDirect = animator.HasParameter(UnityAnimID.ToAttack);
            hasChoice = animator.HasParameter(UnityAnimID.ToNormalAttackSquare);
            hasDirectDive = animator.HasParameter(UnityAnimID.DiveAttacking);
            hasChoiceDive = animator.HasParameter(UnityAnimID.DiveAttackingSquare);
            hasChoiceSpur = animator.HasParameter(UnityAnimID.ToSpurAttackSquare);
            hasChoiceJump = animator.HasParameter(UnityAnimID.ToJumpAttackSquare);
        }

        private void TriggerTemplate(bool hasSymbol, Symbol symbol, int id)
        {
            if (!hasSymbol)
            {
                Debug.LogError("動畫機不含有" + symbol);
                return;
            }

            animator.SetTrigger(id);
        }

        public void NormalAttack(Symbol symbol)
        {
            switch (symbol)
            {
                case Symbol.Direct:
                    TriggerTemplate(hasDirect, symbol, UnityAnimID.ToAttack);
                    break;
                case Symbol.Square:
                    TriggerTemplate(hasChoice, symbol, UnityAnimID.ToNormalAttackSquare);
                    break;
                case Symbol.Circle:
                    TriggerTemplate(hasChoice, symbol, UnityAnimID.ToNormalAttackCircle);
                    break;
                case Symbol.Cross:
                    TriggerTemplate(hasChoice, symbol, UnityAnimID.ToNormalAttackCross);
                    break;
                default:
                    Debug.LogError("超出範圍");
                    return;
            }
        }

        private void BoolTemplate(bool hasSymbol, Symbol symbol, int id, bool @switch)
        {
            if (!hasSymbol)
            {
                Debug.LogError("動畫機不含有" + symbol);
                return;
            }

            animator.SetBool(id, @switch);
        }

        public void DiveAttack(Symbol symbol, bool @switch)
        {
            switch (symbol)
            {
                case Symbol.Direct:
                    BoolTemplate(hasDirectDive, symbol, UnityAnimID.DiveAttacking, @switch);
                    break;
                case Symbol.Square:
                    BoolTemplate(hasChoiceDive, symbol, UnityAnimID.DiveAttackingSquare, @switch);
                    break;
                case Symbol.Circle:
                    BoolTemplate(hasChoiceDive, symbol, UnityAnimID.DiveAttackingCircle, @switch);
                    break;
                case Symbol.Cross:
                    BoolTemplate(hasChoiceDive, symbol, UnityAnimID.DiveAttackingCross, @switch);
                    break;
                default:
                    Debug.LogError("超出範圍");
                    return;
            }
        }

        public void SpurAttack(Symbol symbol)
        {
            switch (symbol)
            {
                case Symbol.Square:
                    TriggerTemplate(hasChoiceSpur, symbol, UnityAnimID.ToSpurAttackSquare);
                    break;
                case Symbol.Circle:
                    TriggerTemplate(hasChoiceSpur, symbol, UnityAnimID.ToSpurAttackCircle);
                    break;
                case Symbol.Cross:
                    TriggerTemplate(hasChoiceSpur, symbol, UnityAnimID.ToSpurAttackCross);
                    break;
                default:
                    Debug.LogError("超出範圍");
                    return;
            }
        }

        public void JumpAttack(Symbol symbol)
        {
            switch (symbol)
            {
                case Symbol.Square:
                    TriggerTemplate(hasChoice, symbol, UnityAnimID.ToJumpAttackSquare);
                    break;
                case Symbol.Circle:
                    TriggerTemplate(hasChoice, symbol, UnityAnimID.ToJumpAttackCross);
                    break;
                case Symbol.Cross:
                    TriggerTemplate(hasChoice, symbol, UnityAnimID.ToJumpAttackCircle);
                    break;
                default:
                    Debug.LogError("超出範圍");
                    return;
            }
        }
    }
}