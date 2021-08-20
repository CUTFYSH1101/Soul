using Main.EventSystem.Cause;
using Main.Util;

namespace Main.EventSystem.Event.Attribute
{
    public class EventAttr
    {
        public CdCause CauseCd, CauseMaxDuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cdTime">0表示沒有冷卻時間</param>
        /// <param name="maxDuration">最大時長，超過該時間，無論如何事件都會終止</param>
        /// <param name="timerMode">使用現實世界的時間，還是遊戲中的</param>
        public EventAttr(float cdTime = 0, float maxDuration = 10, Stopwatch.Mode timerMode = Stopwatch.Mode.LocalGame)
        {
            _timerMode = timerMode;
            CdTime = cdTime;
            MaxDuration = maxDuration;
        }

        private readonly Stopwatch.Mode _timerMode;

        public float CdTime
        {
            get => CauseCd.Lag;
            set
            {
                if (CauseCd.IsEmpty() || MaxDuration != value)
                    CauseCd = new CdCause(value, _timerMode);
            }
            // 預設為localGame
        }

        public float MaxDuration
        {
            get => CauseMaxDuration.Lag;
            set
            {
                if (CauseMaxDuration.IsEmpty() || MaxDuration != value)
                    CauseMaxDuration = new CdCause(value, _timerMode);
            }
            // 預設為localGame
        }
    }
}