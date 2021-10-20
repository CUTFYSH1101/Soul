using JetBrains.Annotations;
using UnityEngine;

namespace Main.Game.Collision.Event
{
    public interface ITriggerStayEvent : IPhysicEvent
    {
        // public (Component obj, Vector2 size) Collider { get; set; }

        // new bool IsTrigger { get; }

        // Func<Collider2D, bool> Cause { get; }

        [CanBeNull] Collider2D[] Others { get; }

        [CanBeNull] Collider2D First { get; }
    }
}