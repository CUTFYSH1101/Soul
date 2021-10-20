using Main.EventLib.Condition;
using Main.Util;

namespace Main.EventLib.Main.EventSystem
{
    public class EventAttr
    {
        public CdCondition CdCondition { get; private set; }
        public CdCondition DurationCondition{ get; private set; }

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
            get => CdCondition.Lag;
            set
            {
                if (CdCondition.IsEmpty() || CdTime != value)
                    CdCondition = new CdCondition(value, _timerMode);
            }
            // 預設為localGame
        }

        public float MaxDuration
        {
            get => DurationCondition.Lag;
            set
            {
                if (DurationCondition.IsEmpty() || MaxDuration != value)
                    DurationCondition = new CdCondition(value, _timerMode);
            }
            // 預設為localGame
        }
    }
}