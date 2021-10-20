using JetBrains.Annotations;
using Main.Util;
using UnityEngine;

namespace Main.Game.Collision.Event
{
    public class TriggerStayEvent : IPhysicEvent, ITriggerStayEvent
    {
        public (Component obj, Vector2 size) Collider; // 注意保持唯一性

        public TriggerStayEvent(Component component)
        {
            Collider.obj = component;
            Collider.size = component.GetColliderSize();
        }

        public virtual bool IsTrigger =>
            !First.IsEmpty();

        [CanBeNull]
        public virtual Collider2D[] Others =>
            Collider.obj.BoxCastAll(Collider.size);

        [CanBeNull]
        public virtual Collider2D First =>
            Others.FirstOrNull();
    }
}