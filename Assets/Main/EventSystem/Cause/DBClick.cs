using System;
using Main.Util;

namespace Main.EventSystem.Cause
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
        private readonly Func<bool> _clickEvent;
        private readonly CdTimer _singleTimer;

        public enum State
        {
            None,
            Single,
            Double
        }

        private State _state = State.None;

        public DBClick(string key, float duration)
        {
            _singleTimer = new CdTimer(duration, Stopwatch.Mode.RealWorld);
            _clickEvent = () => Input.Input.GetButtonDown(key);
            /*this.onSingle = onSingle ?? "single event".LogLine;
            this.onDouble = onDouble ?? "double event".LogLine;*/
            /*this.onSingle = () => "single event".LogLine();
            this.onDouble = () => "double event".LogLine();*/
        }

        public DBClick(Func<bool> clickEvent, float duration)
        {
            _singleTimer = new CdTimer(duration, Stopwatch.Mode.RealWorld);
            _clickEvent = clickEvent;
            /*this.onSingle = onSingle ?? "single event".LogLine;
            this.onDouble = onDouble ?? "double event".LogLine;*/
            /*this.onSingle = () => "single event".LogLine();
            this.onDouble = () => "double event".LogLine();*/
        }

        /// 初始化狀態。還沒有任何點擊
        private bool HasNoClick()
            => _state == State.None && !_clickEvent();

        /// Start。當第一次點擊。優先度高
        private bool IsFirstClick()
            => _state == State.None && _clickEvent();

        /// 當第一次點擊後～第二次點擊前 || 第一次點擊後~執行OnSingleEvent
        private bool IsInSingle()
            => _state == State.Single && !_singleTimer.IsTimeUp;

        /// 當第二次點擊。注意優先度中
        private bool IsDouble()
            => IsInSingle() && _clickEvent();

        /// 當玩家在時間結束之前沒有再次點擊，瞬間觸發。注意仰賴外部呼叫。優先度低
        private bool IsSingle()
            => _state == State.Single && _singleTimer.IsTimeUp;

        private void SubReset() => _state = State.None;


        public bool AndCause()
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
                _state = State.Double;
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
                _state = State.Single;
                _singleTimer.Reset();
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
            string msg = $"{GetType()}" +
                         $"目前狀態；\n" +
                         $"是否已經第一次點擊；{!HasNoClick()}\n" +
                         $"是否已經第一次點擊；{!IsFirstClick()}\n" +
                         $"是否滿足dbClick：{IsDouble()}";
            return msg;
        }
    }
}