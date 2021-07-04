using Main.Common;
using Main.Entity.Controller;
using Main.Util.Timers;
using UnityEngine;
using Main.Util;
using Main.Attribute;
using Main.Entity.Attr;
using Test2.Causes;
using AttackType = Main.Entity.Controller.ICreature.NormalAttackAnimator.Type;

namespace Test2
{
    public class NormalAttack : AbstractSkill
    {
        private readonly ICreature creature;
        private readonly ICreatureAttr attr;
        private readonly Animator animator;
        private readonly ICause minDuration = new CDCause(.1f);

        public NormalAttack(ICreature creature, float cdTime = 0.2f, Stopwatch.Mode mode = Stopwatch.Mode.LocalGame) :
            base(creature.GetRigidbody2D().GetOrAddComponent<MonoClass>(), cdTime, mode)
        {
            this.creature = creature;
            attr = creature.GetCreatureAttr();
            animator = creature.GetAnimator();
            // causeEnter = () => animator.GetStateInfo().IsTag("Attack");
            causeExit = () => !animator.GetStateInfo().IsTag("Attack") && minDuration.Cause();
        }

        public void Invoke(AttackType type)
        {
            if (state == State.Waiting)
            {
                if (minDuration.Cause())
                    minDuration.Reset();
                creature.Attack(type);
                base.Invoke();
            }
        }

        protected override void Enter()
        {
            attr.MindState = MindState.Attack;
        }

        protected override void Update()
        {
        }

        protected override void Exit()
        {
            attr.MindState = MindState.Idle;
        }
    }
}