using System;
using System.Collections;
using Main.Common;
using JetBrains.Annotations;
using Main.Event;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Main.Util
{
    public partial class UnityCoroutine
    {
        private Actions actions;
        private IEnumerator coroutine;
        private MonoBehaviour mono;
        private IEnumerator Coroutine(Func<bool> stopCause)
        {
            actions.ONEnter?.Invoke();
            yield return new Update();

            while (!stopCause())
            {
                actions.ONRepeat?.Invoke();
                yield return new Update();
            }

            actions.ONExit?.Invoke();
        }

        /// （強制）結束協程。當協程無法透過stopCause自動停止時可使用
        public void StopCoroutine() => mono.StopCoroutine(coroutine);

        /// 創建一個協程，用以執行單一重複執行的事件C
        /// <code>
        /// 依序填入：
        /// 遊戲物件,
        /// 第一幀事件A,
        /// 最後一幀事件B, 停下來的原因b,
        /// 重複執行事件C,
        /// </code>
        /// <param skillName="container">Unity物件，協程執行的地方</param>
        /// <param skillName="onEnter">第一幀事件</param>
        /// <param skillName="onExit">最後一幀事件</param>
        /// <param skillName="exitCause">停下來的原因</param>
        /// <param skillName="onRepeat">重複執行的事件</param>
        public UnityCoroutine Create(Component container,
            [CanBeNull] Action onEnter,
            [CanBeNull] Action onExit, Func<bool> exitCause,
            [NotNull] Action onRepeat)
        {
            mono = container.GetOrAddComponent<MonoClass>();
            actions = new Actions(onEnter, onRepeat, onExit);
            // 開始協程
            coroutine = Coroutine(exitCause);
            mono.StartCoroutine(coroutine);
            return this;
        }

        /// 創建一個協程，用以執行單一重複執行的事件
        /// <param skillName="container">Unity物件，協程執行的地方</param>
        /// <param skillName="coroutine">被委託處理的協程</param>
        public UnityCoroutine Create(Component container, IEnumerator coroutine)
        {
            mono = container.GetOrAddComponent<MonoClass>();
            this.coroutine = coroutine;
            // 開始協程
            mono.StartCoroutine(this.coroutine);
            return this;
        }
    }

    public partial class UnityCoroutine
    {
        private IEnumerator Coroutine(
            Func<bool> enterCause,
            Func<bool> interruptCause)
        {
            if (!enterCause())
                yield return null;
            else
            {
                coroutine = Coroutine(interruptCause);
                mono.StartCoroutine(coroutine);
            }
        }

        public UnityCoroutine Create(Component container,
            Action onEnter, Func<bool> enterCause,
            Action onExit, Func<bool> exitCause,
            Action onRepeat)
        {
            mono = container.GetOrAddComponent<MonoClass>();
            actions = new Actions(onEnter, onRepeat, onExit);
            mono.StartCoroutine(Coroutine(enterCause, exitCause));
            return this;
        }
    }

    public partial class UnityCoroutine
    {
        private IEnumerator Coroutine(
            Func<bool> enterCause, Action firstAction,
            Func<bool> toAction2, Action action2,
            Func<bool> toAction3, Action action3,
            Func<bool> toAction4, Action action4)
        {
            if (!enterCause())
                yield return null;
            else
            {
                firstAction?.Invoke();
                yield return Wait(toAction2);

                action2?.Invoke();
                yield return Wait(toAction3);

                action3?.Invoke();
                yield return Wait(toAction4);

                action4?.Invoke();
            }
        }

        public UnityCoroutine Create(Component container,
            Func<bool> enterCause, Action firstAction,
            Func<bool> toAction2, Action action2,
            Func<bool> toAction3, Action action3,
            Func<bool> toAction4, Action action4)
        {
            mono = container.GetOrAddComponent<MonoClass>();
            // 開始協程
            coroutine = Coroutine(
                enterCause, firstAction,
                toAction2, action2,
                toAction3, action3,
                toAction4, action4);
            mono.StartCoroutine(coroutine);
            return this;
        }

        private IEnumerator Wait(Func<bool> cause)
        {
            while (!cause())
            {
                yield return new Update();
            }
        }
    }

    public partial class UnityCoroutine
    {
        private IEnumerator Coroutine(Action action, float delayTime)
        {
            var delayer = new CdCause(delayTime);
            delayer.Reset();
            yield return Wait(delayer.Cause);
            action?.Invoke();
        }

        public UnityCoroutine Create(Component container, Action action, float delayTime)
        {
            mono = container.GetOrAddComponent<MonoClass>();
            // 開始協程
            coroutine = Coroutine(action, delayTime);
            mono.StartCoroutine(coroutine);
            return this;
        }
    }

    public class UnityCoroutineObserver
    {
        private IEnumerator coroutine;

        private MonoBehaviour mono;
        // public void StopCoroutine() => mono.StopCoroutine(coroutine);

        private IEnumerator Coroutine([NotNull] UnityCoroutine subject,
            Func<bool> interruptCause, Action interruptAction)
        {
            yield return new Update();// 避免一開始就觸發
            while (!interruptCause())
                yield return new Update();

            subject.StopCoroutine();
            interruptAction?.Invoke();
        }

        public UnityCoroutineObserver Create([NotNull] Component container,
            [NotNull] UnityCoroutine subject,
            Func<bool> interruptCause, Action interruptAction)
        {
            mono = container.GetOrAddComponent<MonoClass>();
            coroutine = Coroutine(subject, interruptCause, interruptAction);
            mono.StartCoroutine(coroutine);
            return this;
        }
    }
}