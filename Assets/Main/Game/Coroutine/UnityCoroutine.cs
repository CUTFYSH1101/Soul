using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Main.EventLib.Main.EventSystem;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Main.Game.Coroutine
{
    public partial class UnityCoroutine
    {
        private ActionData[] _elements;
        private (MonoBehaviour mono, IEnumerator coroutine) _instance;
        private static Update _waitUnityUpdate;

        public void InterruptCoroutine()
        {
            if (_instance.coroutine != null)
                _instance.mono.StopCoroutine(_instance.coroutine);
        }

        public static IEnumerator Wait([CanBeNull] Func<bool> condition)
        {
            yield return null; // 避免一開始就觸發
            while (!condition.OrCause())
                yield return null;
        }

        // 如果不滿足條件則無限次觸發，滿足則跳往下一行
        public static IEnumerator RepeatMethodUntilTrue(Action method, Func<bool> condition)
        {
            yield return null; // 避免一開始就觸發
            while (!condition.OrCause())
            {
                yield return null;
                method?.Invoke();
            }

            #region UpdateOld

            /*yield return new Update();
            while (!causeInterruptAndExit())
            {
                yield return new Update();
                _elements[1].Action?.Invoke();
            }
            _elements[2].Action?.Invoke();*/

            #endregion
        }

        // 等待直到滿足條件才觸發，並跳往下一行
        public static IEnumerator ExecuteMethodOnceWhenTrue(Action method, Func<bool> condition)
        {
            yield return null; // 避免一開始就觸發
            while (!condition.OrCause())
                yield return null;

            method?.Invoke();
        }
    }

    public partial class UnityCoroutine
    {
        public UnityCoroutine Create(
            params (Func<bool> Condition, Action Action, bool IsUpdateMethod)[] actions)
        {
            if (IsActionLessThenTwo(actions))
                throw new Exception();

            _elements = new ActionData[actions.Length];
            _elements = actions.Select(tuple => new ActionData(tuple.Condition, tuple.Action, tuple.IsUpdateMethod))
                .ToArray();
            _instance.mono = MonoClass.Instance;
            _instance.coroutine = Coroutine();

            if (Filter) Execute();
            return this;
        }

        public UnityCoroutine Create(
            params ActionData[] actions)
        {
            if (IsActionLessThenTwo(actions))
                throw new Exception();

            _instance.mono = MonoClass.Instance;
            _instance.coroutine = Coroutine();
            _elements = actions;

            if (_elements[^1].IsUpdateMethod) throw new Exception(); // 不允許最後一輪為update
            if (Filter) Execute();
            return this;
        }

        #region 規則

        // update本身不含有condition，必須依賴下一輪，除非兩輪update相連，或是第一輪為update
        // 最後一輪不得為update，因為update必須仰賴下一輪，否則回傳錯誤
        // update->once，執行後一輪的condition（並省略掉一次once的condition）
        // update->update，執行後一輪的condition（update不得為最後一輪）
        // once->update
        // 第一輪如果為update，執行condition

        #endregion

        private IEnumerator Coroutine()
        {
            bool previousIsUpdate = false;
            ActionData current = default;
            ActionData next = default;
            for (var i = 1; i < _elements.Length; i++)
            {
                current = _elements[i];
                next = default; // 重設next
                if (i + 1 < _elements.Length) next = _elements[i + 1];

                yield return current.IsUpdateMethod switch
                {
                    true => RepeatMethodUntilTrue(current.Action,
                        next.Condition),// update本身不具有condition，仰賴下一輪的condition
                    false => ExecuteMethodOnceWhenTrue(current.Action,
                        previousIsUpdate ? null : current.Condition) // 上一輪如果是update，跳過這一輪的
                };
                previousIsUpdate = current.IsUpdateMethod; // 紀錄上一輪是否為update
            }

            #region Old

            /*bool previousIsUpdate = false;
            ActionElement current = default;
            ActionElement next = default;
            for (var i = 1; i < _elements.Length; i++)
            {
                current = _elements[i];
                next = default; // 重設next
                if (i + 1 < _elements.Length) next = _elements[i + 1];

                yield return current.IsUpdateMethod switch
                {
                    true => RepeatMethodUntilTrue(current.Action,
                        (next.IsEmpty() ? current : next).Condition.Instance),
                    false => ExecuteMethodOnceWhenTrue(current.Action,
                        previousIsUpdate ? null : current.Condition.Instance) // 上一輪如果是update
                };
                previousIsUpdate = current.IsUpdateMethod; // 紀錄上一輪是否為update
            }*/
            /*for (int i = 1; i < _elements.Length; i++)
                yield return ExecuteMethodOnceWhenTrue(_elements[i]);
            yield return RepeatMethodUntilTrue(_elements[1].Action, _elements[2].Condition.Instance);*/

            #endregion

            yield return null;
        }

        private static bool IsActionLessThenTwo<T>(IReadOnlyCollection<T> actions) =>
            actions == null || actions.Count < 2;

        private bool Filter => _elements[0].Condition.OrCause();

        private void Execute()
        {
            _instance.mono.StartCoroutine(_instance.coroutine);
            _elements[0].Action?.Invoke();
        }
    }

    public class UnityCoroutineObserver
    {
        private (UnityCoroutine coroutine, Func<bool> interruptCause, Action finalAct) _observed;
        private (MonoBehaviour mono, IEnumerator coroutine) _instance;

        public void CoroutineCheckDoneAndDoFinAct([NotNull] UnityCoroutine observed,
            Func<bool> interrupt, Action finalAct)
        {
            _observed = (observed, interrupt, finalAct);
            _instance = (MonoClass.Instance, Coroutine());
            _instance.mono.StartCoroutine(_instance.coroutine);
        }

        private IEnumerator Coroutine()
        {
            yield return UnityCoroutine.Wait(_observed.interruptCause);

            _observed.coroutine.InterruptCoroutine();
            _observed.finalAct?.Invoke();
            // _instance.mono.StopCoroutine(_instance.coroutine);
        }
    }
}