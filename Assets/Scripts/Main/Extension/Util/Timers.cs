using Main.Event;
using UnityEngine;

namespace Main.Util.Timers
{
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


    /// <summary>
    /// 單純只有CD的方法。內容為Skill縮減版
    /// </summary>
    public class CDMethod
    {
        private readonly CdCause cdCause;
        private readonly MonoBehaviour mono;

        public CDMethod(MonoBehaviour mono, float cdTime, Stopwatch.Mode mode = Stopwatch.Mode.LocalGame)
        {
            this.mono = mono;
            cdCause = new CdCause(cdTime, mode);
        }

        public void Invoke()
        {
            "試圖發動技能".LogLine();
            // 如果原本就沒有目的 || 目的達成
            if (cdCause == null || cdCause.Cause())
            {
                cdCause.Reset();
                Enter();
                new UnityCoroutine().Create(mono,
                    Enter,
                    Exit, cdCause.Cause,
                    Update);
            }
        }

        /// 第一幀事件
        protected void Enter()
        {
            "開始".LogLine();
        }

        /// 最後一幀事件
        protected void Exit()
        {
            "結束".LogLine();
        }

        /// 過程中事件
        protected void Update()
        {
            "更新中...".LogLine();
            // if (causeEnter.Cause())
            // {
            //     // 停止此Update事件
            //     Exit(); // 最後一幀事件
            // }
        }
    }
}