using Main.EventSystem.Event.Attribute;
using Main.Util;

namespace Main.EventSystem.Event.Demo.EventA
{
    public class ConcreteEventA : AbstractEventA
    {
        public ConcreteEventA(float duration = 10) :
            base(new EventAttr(0, duration))
        {
        }

        public new void Invoke() => base.Invoke();
        protected override bool FinCauseEnter() => true;

        protected override void Enter()
        {
            var _ = "Enter " +
                    base.FinCauseEnter() + " " +
                    base.FinCauseExit();
            _.LogLine();
        }

        protected override void Update()
        {
            var _ = "Update " +
                    base.FinCauseEnter() + " " +
                    base.FinCauseExit();
            _.LogLine();
        }

        protected override void Exit()
        {
            var _ = "Exit " +
                    base.FinCauseEnter() + " " +
                    base.FinCauseExit();
            _.LogLine();
        }
    }
}