using UnityEngine;
using static Main.AnimAndAudioSystem.Anims.Scripts.UnityAnimID;

namespace Main.AnimAndAudioSystem.Anims.Scripts
{
    internal class MindStateAnim
    {
        public bool Alive => _animator.IsName("Die");
        private readonly Animator _animator;
        private readonly bool _hasAnim;


        public MindStateAnim(Animator animator)
        {
            _animator = animator;
            _hasAnim = animator.HasParameter(Knockbacked); // 確認有沒有anim
        }

        public void Killed() => _animator.SetBool(UnityAnimID.Alive, false);
        public void Revival() => _animator.SetBool(UnityAnimID.Alive, true);
        public void Knockback(bool @switch) => _animator.SetBool(Knockbacked, @switch);
    }
}