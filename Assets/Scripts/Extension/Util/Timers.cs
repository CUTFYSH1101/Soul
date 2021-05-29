using System.Collections;
using UnityEngine;

namespace Main.Util
{
    public class Timers
    {
        /// 計時器方法
        public abstract class RepeatMethod
        {
            private readonly MonoBehaviour mono;
            private IEnumerator updateMethod;

            protected RepeatMethod(Component mono)
            {
                this.mono = mono.GetOrAddComponent<MonoClass>();
            }

            /// 開始計時
            protected void CallUpdate(bool @switch)
            {
                if (@switch)
                {
                    updateMethod = Update();
                    mono.StartCoroutine(updateMethod);
                }
                else
                {
                    mono.StopCoroutine(updateMethod);
                }
            }

            /// 開始計時
            protected virtual void Invoke()
            {
                Enter(); // 呼叫第一幀
                CallUpdate(true); // 呼叫使重複執行
            }

            /// 第一幀要執行什麼事。如為複寫類，需接地
            protected abstract void Enter();

            // 注意不要使用mono.StopCoroutine(Update());因可能會停掉其他類別的同名方法
            /// 最後一幀要執行什麼事。如為複寫類，需接地
            protected abstract void Exit();

            /// Invoke或CallUpdate後會反覆執行
            protected abstract IEnumerator Update();

            public override string ToString()
            {
                var message =
                    $"------------------------\n" +
                    $"{GetType().Name}\n" +
                    $"呼叫迭代器： {mono.GetIsNotNullToString()}\n" +
                    $"更新方法： {updateMethod.GetIsNotNullToString()}\n" +
                    $"------------------------\n";
                return message;
            }
        }

        /// 計時器方法。
        /// <code>
        /// 一開始處於TimeUp狀態
        /// Reset後需搭配Update倒數計時
        /// </code>>
        public class Timer
        {
            public float Lag { get; set; } = .1f;
            private float time;
            public void Reset() => time = Lag;

            /// 必須使用實作，否則無法用
            public void Update() => time -= Time.deltaTime;

            public bool IsTimeUp => time <= 0;

            /// 初始化計時器
            // public CDTimer() => Reset();
            public Timer()
            {
            }

            /// 初始化計時器。填入要計時的時間（秒）
            public Timer(float lag)
            {
                Lag = lag;
                // Reset();
            }
        }
    }
}