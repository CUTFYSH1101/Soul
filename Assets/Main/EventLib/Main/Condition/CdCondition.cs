using Main.Util;

namespace Main.EventLib.Condition
{
    /// <summary>
    /// IsTimeUp
    /// </summary>
    public struct CdCondition : ICondition
    {
        private readonly CdTimer _timer;

        public CdCondition(float timer, Stopwatch.Mode mode = Stopwatch.Mode.LocalGame) =>
            _timer = new CdTimer(timer, mode);

        public float Lag =>
            _timer.Lag;

        public float Time =>
            _timer.Time;

        public bool AndCause() =>
            !IsEmpty() &&
            _timer.IsTimeUp;

        public bool OrCause() =>
            IsEmpty() ||
            _timer.IsTimeUp;

        public readonly void Reset() =>
            _timer?.Reset();
        public bool IsEmpty() =>
            Equals(default(CdCondition)) || _timer.Lag == 0;
    }
}