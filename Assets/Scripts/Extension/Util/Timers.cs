using System.Collections;
using Main.Common;
using UnityEngine;

namespace Main.Util.Timers
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

    public class Stopwatch
    {
        public enum Mode
        {
            RealWorld,
            LocalGame
        }

        private readonly Mode mode;

        public Stopwatch(Mode mode)
        {
            this.mode = mode;
        }

        public float time
        {
            get
            {
                switch (mode)
                {
                    case Mode.LocalGame:
                        return UnityEngine.Time.time;
                    case Mode.RealWorld:
                        return UnityEngine.Time.realtimeSinceStartup;
                    default:
                        return 0;
                }
            }
        }
    }

    public class CDTimer
    {
        private readonly Stopwatch timer;
        private readonly float lag = .1f;
        private float time;

        /// 重製。直到時間到，才呼叫下一次計時開始
        public void Reset()
        {
            if (IsTimeUp)
                time = lag + timer.time;
        }

        public bool IsTimeUp => timer.time > time;

        /// 初始化計時器。填入要計時的時間（秒）
        public CDTimer(float lag, Stopwatch.Mode mode)
        {
            this.lag = lag;
            timer = new Stopwatch(mode);
        }
    }
}