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

        public AttackAnim(Animator animator)
        {
            this.animator = animator;
            hasDirect = animator.HasParameter(UnityAnimID.ToAttack);
            hasChoice = animator.HasParameter(UnityAnimID.ToNormalAttackSquare);
            hasDirectDive = animator.HasParameter(UnityAnimID.DiveAttacking);
            hasChoiceDive = animator.HasParameter(UnityAnimID.DiveAttackingSquare);
            hasChoiceSpur = animator.HasParameter(UnityAnimID.ToSpurAttackSquare);
        }

        private void TriggerTemplate(bool hasType, Symbol type, int id)
        {
            if (!hasType)
            {
                Debug.LogError("動畫機不含有" + type);
                return;
            }

            animator.SetTrigger(id);
        }

        public void NormalAttack(Symbol type)
        {
            switch (type)
            {
                case Symbol.Direct:
                    TriggerTemplate(hasDirect, type, UnityAnimID.ToAttack);
                    break;
                case Symbol.Square:
                    TriggerTemplate(hasChoice, type, UnityAnimID.ToNormalAttackSquare);
                    break;
                case Symbol.Circle:
                    TriggerTemplate(hasChoice, type, UnityAnimID.ToNormalAttackCircle);
                    break;
                case Symbol.Cross:
                    TriggerTemplate(hasChoice, type, UnityAnimID.ToNormalAttackCross);
                    break;
                default:
                    Debug.LogError("超出範圍");
                    return;
            }
        }

        private void BoolTemplate(bool hasType, Symbol type, int id, bool @switch)
        {
            if (!hasType)
            {
                Debug.LogError("動畫機不含有" + type);
                return;
            }

            animator.SetBool(id, @switch);
        }

        public void DiveAttack(Symbol type, bool @switch)
        {
            switch (type)
            {
                case Symbol.Direct:
                    BoolTemplate(hasDirectDive, type, UnityAnimID.DiveAttacking, @switch);
                    break;
                case Symbol.Square:
                    BoolTemplate(hasChoiceDive, type, UnityAnimID.DiveAttackingSquare, @switch);
                    break;
                case Symbol.Circle:
                    BoolTemplate(hasChoiceDive, type, UnityAnimID.DiveAttackingCircle, @switch);
                    break;
                case Symbol.Cross:
                    BoolTemplate(hasChoiceDive, type, UnityAnimID.DiveAttackingCross, @switch);
                    break;
                default:
                    Debug.LogError("超出範圍");
                    return;
            }
        }

        public void SpurAttack(Symbol type)
        {
            switch (type)
            {
                case Symbol.Square:
                    TriggerTemplate(hasChoiceSpur, type, UnityAnimID.ToSpurAttackSquare);
                    break;
                case Symbol.Circle:
                    TriggerTemplate(hasChoiceSpur, type, UnityAnimID.ToSpurAttackCircle);
                    break;
                case Symbol.Cross:
                    TriggerTemplate(hasChoiceSpur, type, UnityAnimID.ToSpurAttackCross);
                    break;
                default:
                    Debug.LogError("超出範圍");
                    return;
            }
        }
    }
}