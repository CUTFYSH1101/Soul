using Main.EventSystem.Cause;
using Main.EventSystem.Event.Attribute;
using Main.Util;

namespace Main.EventSystem.Event.Demo.EventB
{
    public class ConcreteEventB : AbstractEventB
    {
        private CdCause wait = new CdCause(2);

        public ConcreteEventB() : base(new EventAttr(0, 10))
        {
            CauseEnter = new FuncCause(() => true);
            CauseInterrupt = new FuncCause(() => false);
            CauseToAction2 = new FuncCause(() => true);
            CauseToAction3 = new FuncCause(() => wait.OrCause());
            CauseToAction4 = new FuncCause(() => true);
            // wait.OrCause() && State == EnumState.ProgressingFirst
        }

        public new void Invoke() => base.Invoke();

        protected override void EnterAction1()
        {
            "EnterAction1".LogLine();
        }

        protected override void Action2()
        {
            "Action2".LogLine();
            wait.Reset();
            State.LogLine();
        }

        protected override void Action3()
        {
            "Action3".LogLine();
        }

        protected override void ExitAction4()
        {
            "ExitAction4".LogLine();
        }
    }
}