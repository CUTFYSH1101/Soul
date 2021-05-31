using Main.Common;
using Main.Entity.Attr;
using Main.Entity.Controller;
using Main.Util;
using Test2.Causes;
using Test2.Timers;
using UnityEngine;
using Rigidbody2D = Main.Entity.Controller.Rigidbody2D;

namespace Test2
{
    public class DiveAttack : AbstractSkill
    {
        private float diveForce => attr.DiveForce;
        private float recoilForce => attr.RecoilForce;
        private readonly Rigidbody2D rigidbody2D;
        private readonly ICreature creature;
        private readonly ICreatureAttr attr;

        private ICause duration;
        private static readonly int DiveAttacking = Animator.StringToHash("DiveAttacking");
        private Vector2 dir;

        public DiveAttack(ICreature creature, float cdTime = 0.2f) :
            base(creature.GetRigidbody2D().GetOrAddComponent<MonoClass>(), cdTime, Stopwatch.Mode.LocalGame)
        {
            this.creature = creature;
            rigidbody2D = creature.GetRigidbody2D();
            attr = creature.GetCreatureAttr();

            duration = new CDCause(0.2f);
            causeEnter = () => !attr.Grounded; //mindState = 空中時，允許俯衝
            causeExit = () => attr.Grounded || duration.Cause(); // 或是duration停止時
        }

        public void Invoke()
        {
            if (state == State.Waiting)
            {
                if (duration.Cause())
                    duration.Reset();
                base.Invoke();
            }
        }

        protected override void Enter()
        {
            dir = new Vector2(creature.IsFacingRight ? 0.7f : -0.7f, -0.7f);
            rigidbody2D.AddForce(dir * diveForce, ForceMode2D.Impulse);
            creature.GetAnimator().SetBool(DiveAttacking, true);
        }

        protected override void Update()
        {
        }

        protected override void Exit()
        {
            // 創造傷害範圍
            dir = -new Vector2(0, -0.7f);
            rigidbody2D.AddForce(dir * recoilForce,ForceMode2D.Impulse);
            Debug.Log(dir);
            // 回到idle
            creature.GetAnimator().SetBool(DiveAttacking, false);
            // stop move / jump
            // open
        }
    }
}