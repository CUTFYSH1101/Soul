using System.Collections;
using Main.Common;
using Main.Entity.Attr;
using Main.Entity.Controller;
using Main.Util;
using Test2.Causes;
using Main.Util.Timers;
using UnityEngine;
using Rigidbody2D = Main.Entity.Controller.Rigidbody2D;

namespace Test2
{
    public class DiveAttack : AbstractSkill
    {
        private readonly float gravityScale;
        private float diveForce => attr.DiveForce;
        private float recoilForce => attr.RecoilForce;
        private readonly Rigidbody2D rigidbody2D;
        private readonly ICreature creature;

        private readonly ICreatureAttr attr;

        // 總技能時長
        private readonly ICause totalDuration;
        private readonly ICause chargedDuration;
        private static readonly int DiveAttacking = Animator.StringToHash("DiveAttacking");
        private Vector2 dir;

        public DiveAttack(ICreature creature, float cdTime = 0.2f) :
            base(creature.GetRigidbody2D().GetOrAddComponent<MonoClass>(), cdTime, Stopwatch.Mode.LocalGame)
        {
            this.creature = creature;
            rigidbody2D = creature.GetRigidbody2D();
            gravityScale = rigidbody2D.instance.gravityScale;
            attr = creature.GetCreatureAttr();

            totalDuration = new CDCause(0.5f);
            chargedDuration = new CDCause(0.3f);
            causeEnter = () => !attr.Grounded; //mindState = 空中時，允許俯衝
            causeExit = () => attr.Grounded || totalDuration.Cause(); // 或是duration停止時
        }

        public void Invoke()
        {
            if (state == State.Waiting)
            {
                if (totalDuration.Cause())
                    totalDuration.Reset();
                base.Invoke();
            }
        }

        protected override void Enter()
        {
            if (chargedDuration.Cause())
                chargedDuration.Reset();
            attr.MindState = MindState.Attack;
            rigidbody2D.StartCoroutine(ChargedAnimation());
        }

        protected override void Update()
        {
        }

        protected override void Exit()
        {
            // 創造傷害範圍
            // dir = -new Vector2(0, -0.7f);
            // rigidbody2D.AddForce_OnActive(dir * recoilForce,ForceMode2D.Impulse);
            // Debug.Log(dir);
            // 回到idle
            creature.GetAnimator().SetBool(DiveAttacking, false);
            // stop move / jump
            // open
            attr.MindState = MindState.Idle;
            // rigidbody2D.StartCoroutine(Stiff());
        }

        private IEnumerator ChargedAnimation()
        {
            rigidbody2D.instance.gravityScale = 0;
            // 播放蓄力動畫
            while (!chargedDuration.Cause())
            {
                yield return new WaitForSeconds(.1f);
            }
            rigidbody2D.instance.gravityScale = gravityScale;
            // 關閉蓄力動畫
            
            // 播放攻擊中動畫
            Crash();
            yield return null;
        }

        // 播放攻擊中動畫
        private void Crash()
        {
            dir = new Vector2(creature.IsFacingRight ? 0.7f : -0.7f, -0.7f);
            rigidbody2D.AddForce_OnActive(dir * diveForce, ForceMode2D.Impulse);
            creature.GetAnimator().SetBool(DiveAttacking, true);
        }

        private IEnumerator Stiff()
        {
            attr.MindState = MindState.Knockback;// TODO 會有專屬的stiff動畫嗎
            /*while (stiffDuration.Cause())
            {
                
            }*/
            yield return null;
        }
    }
}