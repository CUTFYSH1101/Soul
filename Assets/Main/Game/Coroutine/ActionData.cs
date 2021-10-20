using System;

namespace Main.Game.Coroutine
{
    public readonly struct ActionData
    {
        public Func<bool> Condition { get; }
        public Action Action { get; }
        public bool IsUpdateMethod { get; }
        public ActionData(Func<bool> condition, Action action, bool isUpdateMethod = false)
        {
            Condition = condition;
            Action = action;
            IsUpdateMethod = isUpdateMethod;
        }

        public static ActionData[] CreateArray(params (Func<bool> condition, Action action)[] contents)
        {
            var arr = new ActionData[contents.Length];
            for (var i = 0; i < arr.Length; i++) arr[i] = new ActionData(contents[i].condition, contents[i].action);
            return arr;
        }

        public bool IsEmpty() => Equals(default(ActionData));
    }
}