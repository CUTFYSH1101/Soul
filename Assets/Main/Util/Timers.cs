using System;

namespace Main.Util
{
    public class Stopwatch
    {
        public enum Mode
        {
            RealWorld,
            LocalGame
        }

        private readonly Mode mode;

        public Stopwatch(Mode mode = Mode.LocalGame)
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
                        return Time.LocalTimeSinceStartup;
                    case Mode.RealWorld:
                        return Time.RealtimeSinceStartup;
                    default:
                        return 0;
                }
            }
        }
    }

    public class CDTimer
    {
        private readonly Stopwatch timer;
        private float time;
        public float Time => time - timer.time;
        public float Lag { get; } // .1f

        /// 重製。直到時間到，才呼叫下一次計時開始
        public void Reset()
        {
            if (timer == null) return;
            time = Lag + timer.time;
        }

        public bool IsTimeUp =>
            timer.time > time;

        /// 初始化計時器。填入要計時的時間（秒）
        public CDTimer(float lag, Stopwatch.Mode mode)
        {
            Lag = lag;
            time = 0;
            timer = new Stopwatch(mode);
        }
    }

    public struct Time
    {
        // 每幀幾秒
        public static float DeltaTime => UnityEngine.Time.deltaTime;

        // 遊戲開始後
        public static float RealtimeSinceStartup => UnityEngine.Time.realtimeSinceStartup;

        // 遊戲載入後
        public static float LocalTimeSinceStartup => UnityEngine.Time.time;

        // 自從2021.1.1 00:00:00開始已經經過多少秒
        public static double CurrentTime => (float) DateTime.UtcNow
            .Subtract(new DateTime(2021, 1, 1, 0, 0, 0))
            .TotalSeconds * Scale / Scale;

        private const int Scale = 10000000;
    }

    /*/// <summary>
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
    }*/
}