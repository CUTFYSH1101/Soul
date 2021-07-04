using System;
using JetBrains.Annotations;

namespace Main.Common
{
    public class Actions
    {
        [CanBeNull] public Action ONEnter { get; }

        [CanBeNull] public Action ONRepeat { get; }

        [CanBeNull] public Action ONExit { get; }

        public Actions(
            [CanBeNull] Action onEnter,
            [CanBeNull] Action onRepeat,
            [CanBeNull] Action onExit)
        {
            this.ONEnter = onEnter;
            this.ONRepeat = onRepeat;
            this.ONExit = onExit;
        }
    }
}