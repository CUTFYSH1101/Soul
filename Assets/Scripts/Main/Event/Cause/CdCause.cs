using Main.Util.Timers;

namespace Main.Event
{
    /// <summary>
    /// IsTimeUp
    /// </summary>
    public class CdCause : ICause
    {
        private readonly CDTimer cdTimer;

        public CdCause(float timer, Stopwatch.Mode mode = Stopwatch.Mode.LocalGame)
        {
            cdTimer = new CDTimer(timer, mode);
        }

        public bool Cause()
        {
            var temp = cdTimer.IsTimeUp;
            return temp;
        }

        public void Reset()
        {
            if (cdTimer.IsTimeUp) cdTimer.Reset();
        }
    }
}