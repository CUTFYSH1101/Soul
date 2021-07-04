using System;
using System.Linq;
using Main.Util;
using UnityEngine;

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
        /// ground checker
        public static bool IsGrounded(this Component origin) =>
            origin.CircleCastAll(0.1f, "Ground").Any();

        public static Vector2 GetLeanOnWallPos(this Component origin, float width) =>
            origin.CircleCastAll(width * 0.5f, collider2D => collider2D.CompareTag("Wall"))
                .Select(c=>c.transform.position).FirstOrDefault();

        public static Collider2D[] AnyInView(this Component origin, float w, float h,
            Func<Collider2D, bool> predicate) =>
            origin.BoxCastAll(w,h, predicate).ToArray(); 

        /// is facing left
        public static RaycastHit2D[] AnyInView(this Component origin, float distance, Vector2 facingDirection,
            Func<RaycastHit2D, bool> predicate) =>
            origin.RayCastAll(facingDirection, hit2D => predicate(hit2D) && hit2D.distance <= distance).ToArray();

        public static float AnyDistance(this Component origin, Vector2 direction) => origin.GetDistance(direction);
        public static float GroundClearance(this Component origin) => origin.GetDistance(Vector2.down,"Ground");
    }
}