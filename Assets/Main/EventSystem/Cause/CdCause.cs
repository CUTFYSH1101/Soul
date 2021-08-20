using Main.Util;

namespace Main.EventSystem.Cause
{
    /// <summary>
    /// IsTimeUp
    /// </summary>
    public struct CdCause : ICause
    {
        private readonly CdTimer _cdTimer;

        public CdCause(float timer, Stopwatch.Mode mode = Stopwatch.Mode.LocalGame) =>
            _cdTimer = new CdTimer(timer, mode);

        public float Lag =>
            _cdTimer.Lag;

        public float Time =>
            _cdTimer.Time;

        public bool AndCause() =>
            !IsEmpty() &&
            _cdTimer.IsTimeUp;

        public bool OrCause() =>
            IsEmpty() ||
            _cdTimer.IsTimeUp;

        public readonly void Reset() =>
            _cdTimer?.Reset();

        public bool IsEmpty() =>
            Equals(default(CdCause)) || _cdTimer.Lag == 0;
    }
}