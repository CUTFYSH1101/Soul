using System;
using System.Collections;
using Main.Common;
using JetBrains.Annotations;
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
            actions.OnEnter?.Invoke();
            while (!stopCause())
            {
                actions.OnRepeat?.Invoke();
                yield return new Update();
            }
            actions.OnExit?.Invoke();
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
        /// <param name="container">Unity物件，協程執行的地方</param>
        /// <param name="onEnter">第一幀事件</param>
        /// <param name="onExit">最後一幀事件</param>
        /// <param name="exitCause">停下來的原因</param>
        /// <param name="onRepeat">重複執行的事件</param>
        public void Create(Component container,
            [CanBeNull] Action onEnter,
            [CanBeNull] Action onExit, Func<bool> exitCause,
            [NotNull] Action onRepeat)
        {
            mono = container.GetOrAddComponent<MonoClass>();
            actions = new Actions(onEnter, onRepeat, onExit);
            // 開始協程
            coroutine = Coroutine(exitCause);
            mono.StartCoroutine(coroutine);
        }

        /// 創建一個協程，用以執行單一重複執行的事件
        /// <param name="container">Unity物件，協程執行的地方</param>
        /// <param name="coroutine">被委託處理的協程</param>
        public void Create(Component container, IEnumerator coroutine)
        {
            mono = container.GetOrAddComponent<MonoClass>();
            this.coroutine = coroutine;
            // 開始協程
            mono.StartCoroutine(this.coroutine);
        }
    }

    public partial class UnityCoroutine
    {
        private IEnumerator Coroutine(
            Func<bool> enterCause,
            Func<bool> exitCause)
        {
            if (!enterCause())
                yield return null;
            else
            {
                coroutine = Coroutine(exitCause);
                mono.StartCoroutine(coroutine);
            }
        }

        public void Create(Component container,
            Action onEnter, Func<bool> enterCause,
            Action onExit, Func<bool> exitCause,
            Action onRepeat)
        {
            mono = container.GetOrAddComponent<MonoClass>();
            actions = new Actions(onEnter, onRepeat, onExit);
            mono.StartCoroutine(Coroutine(enterCause, exitCause));
        }
    }
}