using System;

namespace Main.EventLib.Sub
{
    public static class DebugMode
    {
        public static bool IsOpen
        {
            get => _isOpen;
            set
            {
                _isOpen = value;
                if (_isOpen)
                {
                    DoWhenOpen?.Invoke();
                }
                else
                {
                    DoWhenClose?.Invoke();
                }
            }
        }
        private static bool _isOpen;
        public static Action DoWhenClose;
        public static Action DoWhenOpen;
    }
}