using System;
using Main.Entity.Creature.Skill.Skill_210528;
using Main.Event;
using Main.Extension.Util;

namespace Main.Entity.Skill_210528
{
    public class StateEvent : AbstractEvent
    {
        private readonly CdCause fixDuration;

        public StateEvent(AbstractCreature creature, Action onEnter, Action onExit, float duration) :
            base(creature.MonoClass(), 0.1f, duration) // 沒有cd
        {
            fixDuration = new CdCause(duration);
            causeEnter = () => creature.GetCreatureAttr().Alive;
            causeExit1 = fixDuration;
            this.onEnter = onEnter;
            this.onExit = onExit;
        }

        public void Invoke()
        {
            base.Invoke();
        }

        protected override void Enter()
        {
        }

        protected override void Update()
        {
        }

        protected override void Exit()
        {
        }
    }
}