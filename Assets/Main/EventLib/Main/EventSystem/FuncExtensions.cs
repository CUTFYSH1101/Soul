using System;
using JetBrains.Annotations;

namespace Main.EventLib.Main.EventSystem
{
    public static class FuncExtensions
    {
        public static bool AndCause([CanBeNull] this Func<bool> condition) =>
            condition != null && condition();

        public static bool OrCause([CanBeNull] this Func<bool> condition) =>
            condition == null || condition();
    }
}