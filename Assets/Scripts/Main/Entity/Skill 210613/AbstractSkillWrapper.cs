using System;
using Main.Event;
using Main.Util;
using Main.Util.Timers;
using UnityEngine;

namespace Main.Entity.Skill_210613
{
    public struct BundleCause
    {
        public Func<bool> cause;
        public ICause cause1;
        private bool used;

        public bool Cause
        {
            get
            {
                /*if (!used)
                {
                    used = true;
                    cause1?.Reset();
                }*/

                return (cause1 == null || cause1.Cause()) &&
                       (cause == null || cause.Invoke());
            }
        }

        public void Reset()
        {
            used = false;
            cause1?.Reset();
        }
    }

    public abstract class AbstractSkillWrapper
    {
        protected enum State
        {
            Waiting, // 等待呼叫
            Progressing, // 執行中...
            Finished // 完成
        }

        protected State state = State.Waiting;
        private Component container;

        private ICause causeCD;

        protected BundleCause
            CauseEnter,
            CauseInterrupt,
            CauseToAction2,
            CauseToAction3,
            CauseToAction4;

        protected abstract void EnterAction1(); // 蓄力動畫
        protected abstract void Action2(); // 攻擊動畫
        protected abstract void Action3(); // 暈眩動畫
        protected abstract void ExitAction4(); // 回到一開始

        protected AbstractSkillWrapper(Component container,
            float cdTime = 0.1f, Stopwatch.Mode mode = Stopwatch.Mode.LocalGame)
        {
            this.container = container;
            this.causeCD = new CDCause(cdTime, mode);
        }
        protected void Invoke()
        {
            if (state == State.Waiting)
            {
                // 避免一開始就滿足條件，而提早結束
                CauseInterrupt.Reset();
                UnityCoroutine unityCoroutine = new UnityCoroutine().Create(container,
                    () => CauseEnter.Cause, () =>
                    {
                        state = State.Progressing;
                        CauseToAction2.Reset();
                        EnterAction1(); // 蓄力動畫
                    },
                    () => CauseToAction2.Cause, () =>
                    {
                        CauseToAction3.Reset();
                        Action2(); // 攻擊動畫
                    },
                    () => CauseToAction3.Cause, () =>
                    {
                        CauseToAction4.Reset();
                        Action3(); // 暈眩動畫
                    },
                    () => CauseToAction4.Cause, () =>
                    {
                        state = State.Waiting;
                        CauseEnter.Reset();
                        ExitAction4(); // 回到一開始
                    });
                
                new UnityCoroutineObserver().Create(container, unityCoroutine, () =>
                    (CauseInterrupt.cause != null || CauseInterrupt.cause1 != null) &&
                    CauseInterrupt.Cause, () => // 必須要有條件才能打斷
                {
                    state = State.Waiting;
                    CauseEnter.Reset();
                    ExitAction4(); // 回到一開始
                });
                
                // 避免無論如何都可進入
                causeCD?.Reset(); // 開始計算CD時間
            }
        }
    }
}