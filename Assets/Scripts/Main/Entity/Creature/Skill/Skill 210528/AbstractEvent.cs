using System;
using Main.Event;
using Main.Util;
using Main.Util.Timers;
using UnityEngine;

namespace Main.Entity.Creature.Skill.Skill_210528
{
    public abstract class AbstractEvent
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
        private readonly CdCause causeCD, causeMaxDuration;
        private readonly MonoBehaviour mono;

        private bool CauseEnter() =>
            causeCD.Cause() &&
            (causeEnter == null || causeEnter()) &&
            (causeEnter1 == null || causeEnter1.Cause());

        // 一旦時長到達，無論如何都要結束
        private bool CauseExit() =>
            causeMaxDuration.Cause() ||
            (causeExit == null || causeExit()) &&
            (causeExit1 == null || causeExit1.Cause());

        protected AbstractEvent(MonoBehaviour mono, float cdTime = 0.1f, float maxDuration = 10f)
        {
            this.mono = mono;
            causeCD = new CdCause(cdTime, Stopwatch.Mode.LocalGame); // 預設為localGame
            causeMaxDuration = new CdCause(maxDuration, Stopwatch.Mode.LocalGame); // 預設為localGame
        }
        /*protected AbstractSkill(AbstractCreature creature, float cdTime = 0.1f)
        {
            this.mono = creature.MonoClass();
            this.causeCD = new CdCause(cdTime, Stopwatch.Mode.LocalGame);// 預設為localGame
        }*/

        protected void Invoke()
        {
            if (state == State.Waiting)
            {
                causeExit1?.Reset();
                causeMaxDuration?.Reset();
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