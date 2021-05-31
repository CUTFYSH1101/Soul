using System;
using System.Linq;
using JetBrains.Annotations;
using Main.Util;
using UnityEngine;
using static UnityEngine.Physics2D;

namespace Main.Entity.Controller
{
    public static class Collision
    {
        public static int ToLayerMask(this int layerIndex)
            => 1 << layerIndex;

        /// 回傳是否碰撞
        public static bool CircleCast(this Component subject,
            Vector2 point, float radius,
            int? layerMask = null)
        {
            return CircleCast(subject, out _, point, radius, layerMask);
        }

        /// 回傳是否碰撞以及碰撞物。
        public static bool CircleCast(this Component subject,
            [CanBeNull] out Collider2D[] result,
            Vector2 point, float radius,
            int? layerMask = null)
        {
            Collider2D[] collider2Ds;
            // Collider2D[] collider2Ds = new Collider2D[10];
            if (layerMask == null)
            {
                collider2Ds = OverlapCircleAll(point, radius);
                // OverlapCircleNonAlloc(point, radius, collider2Ds);
            }
            else
            {
                //TODO:故障ERROR
                collider2Ds = OverlapCircleAll(point, radius, (int) layerMask);
                // OverlapCircleNonAlloc(point, radius, collider2Ds, (int) layerMask);
            }

            collider2Ds = collider2Ds
                ?.Where(obj => subject.transform != obj.transform)
                ?.ToArray();
            result = collider2Ds.Length != 0 ? collider2Ds : null;
            return !result.IsEmpty();
        }

        /// 回傳是否碰撞。根據條件篩選被碰撞物
        public static bool CircleCast(this Component subject,
            Func<Collider2D, bool> predicate,
            Vector2 point, float radius,
            int? layerMask = null)
        {
            CircleCast(subject, out var result, point, radius, layerMask);
            result = result?.Where(predicate).ToArray();
            return !result.IsEmpty(); // result != null && result.Length != 0
        }

        /// 回傳是否碰撞以及碰撞物。根據條件篩選被碰撞物
        public static bool CircleCast(this Component subject,
            [CanBeNull] out Collider2D[] result,
            Func<Collider2D, bool> predicate,
            Vector2 point, float radius,
            int? layerMask = null)
        {
            CircleCast(subject, out result, point, radius, layerMask);
            result = result?.Where(predicate).ToArray();
            return !result.IsEmpty();
        }

        /// 回傳是否碰撞
        public static bool RayCast(this Component subject, Vector2 origin, Vector2 direction,
            float? distance = null, int? layerMask = null)
        {
            return RayCast(subject, out _, origin, direction, distance);
        }

        /// 回傳是否碰撞以及碰撞物。
        public static bool RayCast(this Component subject,
            [CanBeNull] out Collider2D[] result,
            Vector2 origin, Vector2 direction,
            float? distance = null,
            int? layerMask = null)
        {
            direction = direction.normalized;
            RaycastHit2D[] collider2Ds = new RaycastHit2D[10];
            distance = distance ?? Single.MaxValue;
            if (layerMask == null)
            {
                collider2Ds = RaycastAll(origin, direction, (float) distance);
                // RaycastNonAlloc(origin, direction, collider2Ds, (float) distance);
            }
            else
            {
                collider2Ds = RaycastAll(origin, direction, (float) distance, (int) layerMask);
                // RaycastNonAlloc(origin, direction, collider2Ds, (float) distance, (int) layerMask);
            }

            collider2Ds = collider2Ds
                ?.Where(obj => subject.transform != obj.transform)
                ?.ToArray();
            result = collider2Ds.Length != 0 ? collider2Ds.Select(o => o.collider).ToArray() : null;
            return !result.IsEmpty();
        }

        /// 回傳是否碰撞以及碰撞物。根據條件篩選被碰撞物
        public static bool RayCast(this Component subject,
            Func<Collider2D, bool> predicate,
            Vector2 origin, Vector2 direction,
            float? distance = null,
            int? layerMask = null)
        {
            RayCast(subject, out var result, origin, direction, distance);
            result = result?.Where(predicate).ToArray();
            return !result.IsEmpty();
        }

        /// 回傳是否碰撞以及碰撞物。根據條件篩選被碰撞物
        public static bool RayCast(this Component subject,
            [CanBeNull] out Collider2D[] result,
            Func<Collider2D, bool> predicate,
            Vector2 origin, Vector2 direction,
            float? distance = null,
            int? layerMask = null)
        {
            RayCast(subject, out result, origin, direction, distance);
            result = result?.Where(predicate).ToArray();
            return !result.IsEmpty();
        }
    }
}