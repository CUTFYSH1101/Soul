using UnityEngine;

namespace Main.AnimCinemachine
{
   public class Anim
   {
      private readonly Animator _unityAnimator;
      private static readonly int Speed = Animator.StringToHash("Speed");

      public Anim(Animator unityAnimator) => 
         _unityAnimator = unityAnimator;

      public void Move(int dir) => 
         _unityAnimator.SetInteger(Speed,dir);
   }
}
