using System;
using JetBrains.Annotations;

namespace Main.Common
{
    public class Functions<T>
    {
        [CanBeNull] public Func<T> ONEnter { get; }

        [CanBeNull] public Func<T> ONRepeat { get; }

        [CanBeNull] public Func<T> ONExit { get; }

        public Functions(
            [CanBeNull] Func<T> onEnter,
            [CanBeNull] Func<T> onRepeat,
            [CanBeNull] Func<T> onExit)
        {
            this.ONEnter = onEnter;
            this.ONRepeat = onRepeat;
            this.ONExit = onExit;
        }
    }
}