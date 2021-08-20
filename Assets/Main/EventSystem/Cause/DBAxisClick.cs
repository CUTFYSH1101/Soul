using System;

namespace Main.EventSystem.Cause
{
    public class DBAxisClick : ICause
    {
        private readonly DBClick _positiveListener, _negativeListener;
        public Func<int> AxisRaw { get; }
        public DBAxisClick(string key, float duration)
        {
            _positiveListener = new DBClick(() => Input.Input.GetAxisRawDown(key) > 0, duration);
            _negativeListener = new DBClick(() => Input.Input.GetAxisRawDown(key) < 0, duration);
            AxisRaw = () => Input.Input.GetAxisRaw(key);
        }

        public bool AndCause() => _positiveListener.AndCause() || _negativeListener.AndCause();

        public void Reset()
        {
            _positiveListener.Reset();
            _negativeListener.Reset();
        }
    }
}