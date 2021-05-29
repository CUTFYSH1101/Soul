using Main.Util;
using Test2.Causes;
using UnityEngine;
using Stopwatch = Test2.Timers.Stopwatch;

namespace Test2
{
    class Sample : AbstractSkill
    {
        private CDCause temp1;

        public void Reset() =>
            temp1.Reset();

        public Sample(MonoBehaviour mono, float cdTime = 0.3f, Stopwatch.Mode mode = Stopwatch.Mode.LocalGame) :
            base(mono, cdTime, mode)
        {
            // causeExit1 = new CDCause(2);
            temp1 = new CDCause(2);
            causeExit = () => temp1.Cause();
        }

        public void Invoke()
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