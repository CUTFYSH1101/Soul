using UnityEngine;
using static Main.Game.Anim.UnityAnimID;

namespace Main.Game.Anim
{
    public class MindStateAnim
    {
        public bool Alive => animator.IsName("Die");
        private readonly Animator animator;
        private readonly bool hasAnim;


        public MindStateAnim(Animator animator)
        {
            this.animator = animator;
            hasAnim = animator.HasParameter(Knockbacked); // 確認有沒有anim
        }

        public void Killed() => animator.SetBool(UnityAnimID.Alive, false);
        public void Revival() => animator.SetBool(UnityAnimID.Alive, true);
        public void Knockback(bool @switch) => animator.SetBool(Knockbacked, @switch);
    }
}