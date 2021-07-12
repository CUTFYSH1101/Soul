using Main.Game.Input;
using Main.Util.Timers;

namespace Main.Event
{
    public class SingleClick : ICause
    {
        private readonly CDTimer singleTimer;
        private DBClick.State state = DBClick.State.None;
        private readonly string key;

        /// 優先級0
        private bool IsFirstClick()
            => state == DBClick.State.None && Input.GetButtonDown(key);

        private bool IsInSingle()
            => state == DBClick.State.Single && !singleTimer.IsTimeUp;

        /// 優先級1
        private bool IsDouble()
            => IsInSingle() && Input.GetButtonDown(key);

        /// 優先級2
        private bool IsSingle()
            => state == DBClick.State.Single && singleTimer.IsTimeUp;

        public SingleClick(string key, float limitTime)
        {
            this.key = key;
            singleTimer = new CDTimer(limitTime, Stopwatch.Mode.RealWorld);
        }

        public bool Cause()
        {
            if (IsFirstClick())
            {
                state = DBClick.State.Single;
                singleTimer.Reset();
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

        private void SubReset() => state = DBClick.State.None;
    }
}