using System;
using Main.Event;
using Main.Util;
using Main.Util.Timers;
using UnityEngine;

namespace Main.Entity.Skill_210528
{
    /// 包含Skill的功能，支持更多cause, if-else支持
    public abstract class AbstractSkill
    {
        protected enum State
        {
            Waiting, // 等待呼叫
            Progressing, // 執行中...
            Finished // 完成
        }

        protected State state;
        protected Action onEnter, onUpdate, onExit;
        protected Func<bool> causeEnter, causeExit;
        protected ICause causeEnter1, causeExit1;
        protected ICause causeCD;
        private MonoBehaviour mono;

        private bool CauseEnter() =>
            causeCD.Cause() &&
            (causeEnter == null || causeEnter()) &&
            (causeEnter1 == null || causeEnter1.Cause());

        private bool CauseExit() =>
            (causeExit == null || causeExit()) &&
            (causeExit1 == null || causeExit1.Cause());

        protected AbstractSkill(MonoBehaviour mono, float cdTime = 0.1f, Stopwatch.Mode mode = Stopwatch.Mode.LocalGame)
        {
            this.mono = mono;
            this.causeCD = new CDCause(cdTime, mode);
        }

        protected void Invoke()
        {
            if (state == State.Waiting)
            {
                causeExit1?.Reset();
                new UnityCoroutine().Create(mono,
                    () =>
                    {
                        state = State.Progressing;
                        onEnter?.Invoke();
                        Enter();
                    }, CauseEnter,
                    () =>
                    {
                        state = State.Waiting;
                        onExit?.Invoke();
                        Exit();
                    }, CauseExit,
                    () =>
                    {
                        onUpdate?.Invoke();
                        Update();
                    });
                causeEnter1?.Reset();
                causeCD?.Reset();
            }
        }

        protected abstract void Enter();

        protected abstract void Update();

        protected abstract void Exit();

        public override string ToString()
        {
            var msg =
                $"{this.GetType()}\n" +
                $"進程：{state}\n" +
                $"mono是否正常？{!mono.IsEmpty()}\n" +
                $"enter條件狀態？{CauseEnter()} {causeCD == null || causeCD.Cause()} {causeEnter == null || causeEnter.Invoke()} {causeEnter1 == null || causeEnter1.Cause()}\n" +
                $"exit條件狀態？{CauseExit()} {causeExit == null || causeExit.Invoke()} {causeExit1 == null || causeExit1.Cause()}\n";
            return msg;
        }
    }
}