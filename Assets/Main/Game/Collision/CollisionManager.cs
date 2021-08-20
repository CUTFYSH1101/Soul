using System;
using System.Linq;
using Main.Util;
using UnityEngine;
using UnityPhysics2D = UnityEngine.Physics2D;

namespace Main.Game.Collision
{
    public static class CollisionManager
    {
        /*private void Update()
        {
            // Debug.Log(this.GetDistance(Vector2.down));
            // Debug.Log(this.CircleCastAll(this.transform.position, 3).Any());
            // GetGrounded().LogLine();
            // AnyInView(3,hit=>hit.transform).LogLine();
            // AnyInView(3,"Ground").LogLine();
            // AnyDistance(this).LogLine();
            // Debug.Log(array.ArrayToString());
        }*/
        /// 使用 Where
        public static Collider2D[] AnyInView(this Component origin, float w, float h,
            Func<Collider2D, bool> filter) =>
            origin.BoxCastAll(new Vector2(w, h)).Filter(filter).ToArray();

        /// 使用 Select
        public static T[] AnyInView<T>(this Component origin, float w, float h,
            Func<Collider2D, T> selector) =>
            origin.BoxCastAll(new Vector2(w, h)).Get(selector).ToArray();

        /// 射線
        public static RaycastHit2D[] AnyInView(this Component origin, float distance, Vector2 facingDirection,
            Func<RaycastHit2D, bool> filter) =>
            origin.RayCastAll(facingDirection, hit2D => filter(hit2D) && hit2D.distance <= distance).ToArray();


        /// ground checker
        public static bool IsGrounded(this Component origin) =>
            origin.CircleCastAll(0.1f, "Ground").Any();

        public static float GroundClearance(this Component origin)
        {
            var _ = origin.GetDistance(Vector2.down, "Ground");
            return _ < .01f ? 0 : _;
        }

        public static float AnyDistance(this Component origin, Vector2 direction) => origin.GetDistance(direction);

        public static Vector2 GetLeanOnWallPos(this Component origin, float width) =>
            origin.CircleCastAll(width * 0.5f, collider2D => collider2D.CompareTag("Wall"))
                .Get(c => c.transform.position).FirstOrDefault();
        public static bool TouchTheWall(this Component origin) => origin.BoxCastAll(origin.GetColliderSize())
            .Any(collider2D1 => collider2D1.CompareTag("Wall"));
        /// 較耗資源不推薦每幀使用
        public static Vector2 GetColliderSize(this Component origin)
        {
            var collider = origin.GetOrLogComponent<Collider2D>();
            return collider.bounds.size;
        }

        public class TriggerEvent
        {
            protected (Component obj, Vector2 size) Collider; // 注意保持唯一性

            public TriggerEvent(Component component)
            {
                Collider.obj = component;
                Collider.size = component.GetColliderSize();
            }

            public virtual bool IsTriggerStay =>
                !Other.IsEmpty();

            public virtual Collider2D[] Others =>
                Collider.obj.BoxCastAll(Collider.size).ToArray();

            public virtual Collider2D Other =>
                Collider.obj.BoxCastAll(Collider.size).FirstOrNull();
        }

        public class TouchTheWallEvent : TriggerEvent
        {
            public TouchTheWallEvent(Component component) : base(component)
            {
            }
            public override Collider2D[] Others =>
                base.Others.Filter(collider2D =>
                    collider2D.CompareTag("Wall")).ToArray();

            public override Collider2D Other =>
                Collider.obj.BoxCastAll(Collider.size).FirstOrNull(collider2D =>
                    collider2D.CompareTag("Wall"));

            public Vector2 GetLeanOnWallPos() =>
                Other != null ? (Vector2) Other.transform.position : default;
        }

        public class TouchTheGroundEvent : TriggerEvent
        {
            public TouchTheGroundEvent(Component component) : base(component) => 
                Collider.size = new Vector2(Collider.size.x, 0.1f);

            public override Collider2D[] Others =>
                Collider.obj.BoxCastAll(Collider.size).Filter(collider2D =>
                    collider2D.CompareLayer("Ground"));

            public override Collider2D Other =>
                Collider.obj.BoxCastAll(Collider.size).FirstOrNull(collider2D =>
                    collider2D.CompareLayer("Ground"));
        }

        public static void IgnoreCollision()
        {
            var foot = UnityTool.GetComponents<Transform>("GroundChecker");
            var c1 = foot.Filter(transform1 => transform1.CompareTag("Ground1"))
                .Select(t => t.root.GetComponent<Collider2D>());
            var c2 = foot.Filter(transform1 => transform1.CompareTag("Ground2"))
                .Select(t => t.root.GetComponent<Collider2D>());
            var c3 = foot.Filter(transform1 => transform1.CompareTag("Ground3"))
                .Select(t => t.root.GetComponent<Collider2D>());

            // Debug.Log(c2.ToArray().ArrayToString());
            var grounds = UnityTool.GetComponents<Collider2D>(collider1 => collider1.CompareLayer("Ground"));
            var g1 = grounds.Filter(collider2D1 => collider2D1.CompareTag("Ground1"));
            var g2 = grounds.Filter(collider2D1 => collider2D1.CompareTag("Ground2"));
            var g3 = grounds.Filter(collider2D1 => collider2D1.CompareTag("Ground3"));
            foreach (var c in c1)
            {
                foreach (var g in g2) UnityPhysics2D.IgnoreCollision(c, g);
                foreach (var g in g3) UnityPhysics2D.IgnoreCollision(c, g);
            }

            foreach (var c in c2)
            {
                foreach (var g in g1) UnityPhysics2D.IgnoreCollision(c, g);
                foreach (var g in g3) UnityPhysics2D.IgnoreCollision(c, g);
            }

            foreach (var c in c3)
            {
                foreach (var g in g1) UnityPhysics2D.IgnoreCollision(c, g);
                foreach (var g in g2) UnityPhysics2D.IgnoreCollision(c, g);
            }
        }
    }
}