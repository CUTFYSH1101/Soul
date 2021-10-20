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

        private readonly Mode _mode;

        public Stopwatch(Mode mode = Mode.LocalGame)
        {
            _mode = mode;
        }

        public float Time
        {
            get
            {
                switch (_mode)
                {
                    case Mode.LocalGame:
                        return Util.Time.LocalTimeSinceStartup;
                    case Mode.RealWorld:
                        return Util.Time.RealtimeSinceStartup;
                    default:
                        return 0;
                }
            }
        }
    }

    public class CdTimer
    {
        private readonly Stopwatch _timer;
        private float _time;
        public float Time => _time - _timer.Time;
        public float Lag { get; } // .1f

        /// 重製。直到時間到，才呼叫下一次計時開始
        public void Reset()
        {
            if (_timer == null) return;
            _time = Lag + _timer.Time;
        }

        public bool IsTimeUp =>
            _timer.Time > _time;

        /// 初始化計時器。填入要計時的時間（秒）
        public CdTimer(float lag, Stopwatch.Mode mode)
        {
            Lag = lag;
            _time = 0;
            _timer = new Stopwatch(mode);
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
        private readonly CdCondition cdCause;
        private readonly MonoBehaviour mono;

        public CDMethod(MonoBehaviour mono, float cdTime, Stopwatch.Mode mode = Stopwatch.Mode.LocalGame)
        {
            this.mono = mono;
            cdCause = new CdCondition(cdTime, mode);
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