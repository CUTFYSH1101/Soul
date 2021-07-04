using Main.Common;
using Main.Entity;
using Main.Util;
using UnityEngine;

namespace Main.Extension.Util
{
    public static class UnityMono
    {
        /*public static Rigidbody2D Rigidbody2D<T>(this T container) where T : AbstractCreature
        =>container.app*/
        public static MonoBehaviour MonoClass<T>(this T container) where T : AbstractCreature
            => container.GetTransform().GetOrAddComponent<MonoClass>();
    }
}