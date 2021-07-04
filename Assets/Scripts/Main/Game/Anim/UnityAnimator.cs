using System.Linq;
using UnityEngine;

namespace Main.Game.Anim
{
    public static class UnityAnimator
    {
        /*private readonly Animator animator;
        private AnimatorStateInfo GETStateInfo => animator.GetStateInfo();

        public UnityAnimator(Animator animator)
        {
            this.animator = animator;
        }*/
        public static AnimatorStateInfo GetStateInfo(this Animator animator) => animator.GetCurrentAnimatorStateInfo(0);

        public static bool IsTag(this Animator animator, string tag)
            => animator.GetStateInfo().IsTag(tag);

        public static bool IsName(this Animator animator, string name)
            => animator.GetStateInfo().IsName(name);

        public static void SetBool(this Animator animator, int id, bool value)
            => animator.SetBool(id, value);

        public static void SetTrigger(this Animator animator, int id)
            => animator.SetTrigger(id);

        public static void SetFloat(this Animator animator, int id, float value)
            => animator.SetFloat(id, value);

        public static bool HasParameter(this Animator animator, int id)
            => animator.parameters.Any(parameter => parameter.nameHash == id);

        public static bool NotHasParameter(this Animator animator, int id)
            => animator.parameters.All(parameter => parameter.nameHash != id);
    }
}