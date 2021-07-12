using Main.Event;
using Main.Util;
using Main.Util.Timers;
using UnityEngine;

namespace Main.Entity.Skill_210613
{
    public class MonoTest : MonoBehaviour
    {
        private ConcreteSkillWrapper concreteSkill1;

        private void Awake()
        {
            concreteSkill1 = new ConcreteSkillWrapper(this);
        }

        private void Update()
        {
            if (UnityEngine.Input.anyKeyDown)
            {
                concreteSkill1.Invoke();
            }
        }
    }

    public class ConcreteSkillWrapper : AbstractSkillWrapper
    {
        public ConcreteSkillWrapper(Component container, float cdTime = 0.1f) :
            base(container, cdTime)
        {
            CauseEnter.cause1 = new CdCause(5); // cd 5秒
            CauseToAction2.cause1 = new CdCause(1);
            CauseToAction3.cause1 = new CdCause(1);
            CauseToAction4.cause1 = new CdCause(1);
            CauseInterrupt.cause1 = new CdCause(2);
        }

        public new void Invoke()
        {
            this.LogMethodName();
            base.Invoke();
        }

        protected override void EnterAction1()
        {
            this.LogMethodName();
        }

        protected override void Action2()
        {
            this.LogMethodName();
        }

        protected override void Action3()
        {
            this.LogMethodName();
        }

        protected override void ExitAction4()
        {
            this.LogMethodName();
        }
    }
    
}