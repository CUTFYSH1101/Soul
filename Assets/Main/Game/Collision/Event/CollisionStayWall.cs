using System;
using Main.Util;
using UnityEngine;

namespace Main.Game.Collision.Event
{
    public class CollisionStayWall : TriggerStayEvent, IPhysicEvent, ITriggerStayEvent
    {
        private const string Tag = "Wall";
        public CollisionStayWall(Component component) : base(component)
        {
        }
        private static Func<Collider2D, bool> Cause => collider => collider.CompareTag(Tag);
        public override bool IsTrigger => First != null;

        public override Collider2D[] Others =>
            Collider.obj.BoxCastAll(Collider.size, Cause);

        public override Collider2D First =>
            Collider.obj.BoxCastAll(Collider.size).FirstOrNull(Cause);

        public Vector2 GetLeanOnWallPos()
        {
            var first = First;
            return first != null ? (Vector2) first.transform.position : default;
        }
    }
}