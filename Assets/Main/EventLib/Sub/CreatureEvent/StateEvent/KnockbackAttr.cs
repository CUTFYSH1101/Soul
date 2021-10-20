using System;
using UnityEngine;

namespace Main.EventLib.Sub.CreatureEvent.StateEvent
{
    public struct KnockbackAttr
    {
        public float Force;
        public Func<Vector2> DynDirection;
        public Vector2 FinForce => Force * DynDirection().normalized;
        private readonly bool _switch;

        public KnockbackAttr(float force, Func<Vector2> dynDirection, bool @switch)
        {
            this.Force = force;
            this.DynDirection = dynDirection;
            this._switch = @switch;
        }

        public bool Switch => !(DynDirection == null || Force == 0 || _switch == false);
    }
}