using Main.Game.Input;
using Main.Util.Timers;

namespace Main.Event
{
    public class DBClick : ICause
    {
        /*private Action ONSingle
        {
            get
            {
                SubReset();
                return onSingle;
            }
        }

        private Action ONDouble
        {
            get
            {
                SubReset();
                return onDouble;
            }
        }*/

        // private readonly Action onSingle, onDouble;
        private readonly CDTimer singleTimer;
        private readonly string key;

        public enum State
        {
            None,
            Single,
            Double
        }

        private State state = State.None;

        public DBClick(string key, float duration)
        {
            this.key = key;
            this.singleTimer = new CDTimer(duration, Stopwatch.Mode.RealWorld);
            /*this.onSingle = onSingle ?? "single event".LogLine;
            this.onDouble = onDouble ?? "double event".LogLine;*/
            /*this.onSingle = () => "single event".LogLine();
            this.onDouble = () => "double event".LogLine();*/
        }

        /// 初始化狀態。還沒有任何點擊
        private bool HasNoClick()
            => state == State.None && !Input.GetButtonDown(key);

        /// Start。當第一次點擊。優先度高
        private bool IsFirstClick()
            => state == State.None && Input.GetButtonDown(key);

        /// 當第一次點擊後～第二次點擊前 || 第一次點擊後~執行OnSingleEvent
        private bool IsInSingle()
            => state == State.Single && !singleTimer.IsTimeUp;

        /// 當第二次點擊。注意優先度中
        private bool IsDouble()
            => IsInSingle() && Input.GetButtonDown(key);

        /// 當玩家在時間結束之前沒有再次點擊，瞬間觸發。注意仰賴外部呼叫。優先度低
        private bool IsSingle()
            => state == State.Single && singleTimer.IsTimeUp;

        private void SubReset() => state = State.None;


        public bool Cause()
        {
            // 1. 還沒點擊還沒計時return false
            // 2. 按下第一次，
            // 3. 第一次時間到期，執行single ev
            // 4. 第一次時間未到期，按下第二次，執行double eve
            if (HasNoClick())
            {
                return false;
            }

            // 如果dbClick條件滿足，會優先執行
            if (IsDouble())
            {
                state = State.Double;
                // ONDouble?.Invoke();
                Reset();
                return true;
            }


            if (IsSingle())
            {
                // ONSingle?.Invoke();
                Reset();
                return false;
            }

            // 第一次按下，設定下一次事件
            if (IsFirstClick())
            {
                state = State.Single;
                singleTimer.Reset();
            }

            return false;
        }

        // 在玩家觸發IsSingle以後，依然會持續一段時間，直到Reset
        public void Reset()
        {
            SubReset();
        }

        public override string ToString()
        {
            string msg = $"{this.GetType()}" +
                         $"目前狀態；\n" +
                         $"是否已經第一次點擊；{!HasNoClick()}\n" +
                         $"是否已經第一次點擊；{!IsFirstClick()}\n" +
                         $"是否滿足dbClick：{IsDouble()}";
            return msg;
        }
    }
}