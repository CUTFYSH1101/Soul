using Main.Event;
using Main.Util;
using UnityEngine;

namespace Main.Entity.Skill_210528
{
    class Test : AbstractSkill
    {
        private readonly CdCause duration;

        public void Reset() =>
            duration
                .Reset();

        public Test(MonoBehaviour mono, float cdTime = 0.3f) :
            base(mono, cdTime)
        {
            // causeExit1 = new CdCause(2);
            duration = new CdCause(2);
            causeExit = () => duration.Cause();
        }

        public new void Invoke()
        {
            if (duration.Cause())
            {
                duration.Reset();
            }
            base.Invoke();
        }

        protected override void Enter()
        {
            "開始".LogLine();
        }

        protected override void Update()
        {
            "更新".LogLine();
        }

        protected override void Exit()
        {
            "結束".LogLine();
        }
    }

}