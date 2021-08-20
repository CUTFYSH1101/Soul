using Main.Util;

namespace Main.EventSystem.Cause
{
    public class SingleClick : ICause
    {
        private readonly CdTimer _singleTimer;
        private DBClick.State _state = DBClick.State.None;
        private readonly string _key;

        /// 優先級0
        private bool IsFirstClick()
            => _state == DBClick.State.None && Input.Input.GetButtonDown(_key);

        private bool IsInSingle()
            => _state == DBClick.State.Single && !_singleTimer.IsTimeUp;

        /// 優先級1
        private bool IsDouble()
            => IsInSingle() && Input.Input.GetButtonDown(_key);

        /// 優先級2
        private bool IsSingle()
            => _state == DBClick.State.Single && _singleTimer.IsTimeUp;

        public SingleClick(string key, float limitTime)
        {
            _key = key;
            _singleTimer = new CdTimer(limitTime, Stopwatch.Mode.RealWorld);
        }

        public bool AndCause()
        {
            if (IsFirstClick())
            {
                _state = DBClick.State.Single;
                _singleTimer.Reset();
            }

            if (IsDouble())
            {
                /*state = DBClick.State.Double;
                // ...*/
                SubReset();
            }

            var temp = IsSingle();
            if (IsSingle())
            {
                SubReset();
            }

            return temp;
        }

        public void Reset()
        {
        }

        private void SubReset() => _state = DBClick.State.None;
    }
}