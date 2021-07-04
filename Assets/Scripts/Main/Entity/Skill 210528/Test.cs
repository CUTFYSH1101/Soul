using Main.Event;
using Main.Util;
using UnityEngine;
using Stopwatch = Main.Util.Timers.Stopwatch;

namespace Main.Entity.Skill_210528
{
    class Test : AbstractSkill
    {
        private CDCause temp1;

        public void Reset() =>
            temp1.Reset();

        public Test(MonoBehaviour mono, float cdTime = 0.3f, Stopwatch.Mode mode = Stopwatch.Mode.LocalGame) :
            base(mono, cdTime, mode)
        {
            // causeExit1 = new CDCause(2);
            temp1 = new CDCause(2);
            causeExit = () => temp1.Cause();
        }

        public new void Invoke()
        {
            if (temp1.Cause())
            {
                temp1.Reset();
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