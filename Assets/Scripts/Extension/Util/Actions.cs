using System;
using JetBrains.Annotations;

namespace Main.Util
{
    public class Actions
    {
        [CanBeNull] private Action onEnter, onRepeat, onExit;

        [CanBeNull]
        public Action OnEnter
        {
            get => onEnter;
            set => onEnter = value;
        }

        [CanBeNull]
        public Action OnRepeat
        {
            get => onRepeat;
            set => onRepeat = value;
        }

        [CanBeNull]
        public Action OnExit
        {
            get => onExit;
            set => onExit = value;
        }

        public Actions(
            [CanBeNull] Action onEnter, 
            [CanBeNull] Action onRepeat, 
            [CanBeNull] Action onExit)
        {
            this.onEnter = onEnter;
            this.onRepeat = onRepeat;
            this.onExit = onExit;
        }
    }
}