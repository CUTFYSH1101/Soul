using System;
using System.Linq;
using Main.Entity;
using Main.Game.Collision;
using Main.Util;
using UnityEngine;

namespace Test2
{
    public class PP
    {
        private static bool GetC(RaycastHit2D hit2D) => 
            hit2D.GetComponent<Profile>().Get(p => !p.IsKilled())!=null;

        public static bool FindC(AbstractCreatureAI abstractCreatureAI) => 
            abstractCreatureAI.GetTransform().AnyInView(3, Vector2.left, GetC).Any();

        public static bool FindC(Transform transform) => 
            transform.AnyInView(3, Vector2.left, GetC).Any();

        public static bool FindC(Transform transform, Func<Profile, bool> filter) =>
            transform.AnyInView(3, Vector2.left, GetC)
                .Select(hit2D=>hit2D.GetComponent<Profile>()).Any(filter);
    }
}