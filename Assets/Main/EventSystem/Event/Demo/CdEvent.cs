using Main.Util;

namespace Main.EventSystem.Event.Demo
{
    public class CdEvent : AbstractEventC
    {
        public CdEvent(float cdTime = 0, float duration = 10) :
            base(cdTime, duration)
        {
        }

        protected override bool FinCauseEnter() => true;

        protected override void Enter()
        {
            "Enter".LogLine();
        }

        protected override void Exit()
        {
            "Exit".LogLine();
        }
    }
}