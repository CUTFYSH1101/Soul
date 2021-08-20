using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Main.EventSystem.Util
{
    public partial class UnityCoroutine
    {
        private (MonoBehaviour mono, IEnumerator coroutine) _instance;
        public void InterruptCoroutine() => _instance.mono.StopCoroutine(_instance.coroutine);
    }

    // CoroutineA
    public partial class UnityCoroutine
    {
        private (Action onEnter, Action onUpdate, Action onExit) _actionA;

        public UnityCoroutine CreateActionA(
            Func<bool> causeEnter, Action onEnter,
            Func<bool> causeInterrupt, Action onExit,
            Action onRepeat)
        {
            _actionA.onEnter = onEnter;
            _actionA.onUpdate = onRepeat;
            _actionA.onExit = onExit;
            _instance.mono = MonoClass.Instance;
            _instance.coroutine = Coroutine(causeEnter, causeInterrupt);
            _instance.mono.StartCoroutine(_instance.coroutine);
            return this;
        }

        private IEnumerator Coroutine(
            Func<bool> causeEnter,
            Func<bool> causeInterrupt)
        {
            if (!causeEnter())
                yield return null;
            else
            {
                _instance.coroutine = Coroutine(causeInterrupt);
                _instance.mono.StartCoroutine(_instance.coroutine);
            }
        }

        private IEnumerator Coroutine(Func<bool> causeInterrupt)
        {
            _actionA.onEnter?.Invoke();

            yield return new Update();
            while (true)
            {
                yield return new Update();
                if (causeInterrupt()) break;
                _actionA.onUpdate?.Invoke();
            }

            _actionA.onExit?.Invoke();
        }
    }

    // CoroutineB
    public partial class UnityCoroutine
    {
        private (Action firstAction, Action action2, Action action3, Action exitOrInterruptAction) _actionB;

        private (Func<bool> firstAction, Func<bool> action2, Func<bool> action3, Func<bool> exitOrInterruptAction)
            _causeB;

        private static IEnumerator Wait(Func<bool> cause)
        {
            while (!cause())
                yield return new Update();
        }

        private IEnumerator CoroutineB()
        {
            if (!_causeB.firstAction())
                yield return null;
            else
            {
                _actionB.firstAction?.Invoke();

                yield return Wait(_causeB.action2);
                _actionB.action2?.Invoke();

                yield return Wait(_causeB.action3);
                _actionB.action3?.Invoke();

                yield return Wait(_causeB.exitOrInterruptAction);
                _actionB.exitOrInterruptAction?.Invoke();
            }
        }

        public UnityCoroutine CreateActionB(
            Func<bool> causeEnter, Action firstAction,
            Func<bool> toAction2, Action action2,
            Func<bool> toAction3, Action action3,
            Func<bool> causeExitOrInterrupt, Action exitOrInterruptAction)
        {
            _actionB.firstAction = firstAction;
            _actionB.action2 = action2;
            _actionB.action3 = action3;
            _actionB.exitOrInterruptAction = exitOrInterruptAction;
            _causeB.firstAction = causeEnter;
            _causeB.action2 = toAction2;
            _causeB.action3 = toAction3;
            _causeB.exitOrInterruptAction = causeExitOrInterrupt;

            _instance.mono = MonoClass.Instance;
            _instance.coroutine = CoroutineB();
            _instance.mono.StartCoroutine(_instance.coroutine);
            return this;
        }
    }

    // CoroutineC
    public partial class UnityCoroutine
    {
        private (Action onEnter, Action onExit) _actionC;
        private Func<bool> _causeCExitAction;

        private IEnumerator CoroutineC()
        {
            _actionC.onEnter?.Invoke();
            yield return new Update();

            yield return Wait(_causeCExitAction);
            _actionC.onExit?.Invoke();
        }

        public UnityCoroutine CreateActionC(
            Action onEnter,
            Func<bool> causeExit, Action onExit)
        {
            _actionC.onEnter = onEnter;
            _actionC.onExit = onExit;
            _causeCExitAction = causeExit;

            _instance.mono = MonoClass.Instance;
            _instance.coroutine = CoroutineC();
            _instance.mono.StartCoroutine(_instance.coroutine);
            return this;
        }
    }

    // CoroutineB
    public class UnityCoroutineObserver
    {
        private (MonoBehaviour mono, IEnumerator coroutine) _instance;

        private IEnumerator Coroutine([NotNull] UnityCoroutine subject,
            Func<bool> causeInterrupt, Action interruptAction)
        {
            yield return new Update(); // 避免一開始就觸發
            while (!causeInterrupt())
                yield return new Update();

            subject.InterruptCoroutine();
            interruptAction?.Invoke();
            // instance.mono.StopCoroutine(instance.coroutine);
        }

        public UnityCoroutineObserver Create(
            [NotNull] UnityCoroutine subject,
            Func<bool> causeInterrupt, Action interruptAction)
        {
            _instance.mono = MonoClass.Instance;
            _instance.coroutine = Coroutine(subject, causeInterrupt, interruptAction);
            _instance.mono.StartCoroutine(_instance.coroutine);
            return this;
        }
    }
}