using System;

namespace Main.EventSystem.Event
{
    public readonly struct FuncCause
    {
        public bool AndCause => Instance != null && Instance(); // null則始終不成立
        public bool OrCause => Instance == null || Instance(); // null表示默認為true
        public readonly Func<bool> Instance;
        public FuncCause(Func<bool> instance) => Instance = instance;

        /*
        public void Reset()
        {
        }
    */
    }
}