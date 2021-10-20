using System;
using System.Linq;
using JetBrains.Annotations;
using Main.Util;
using UnityEngine;
using UnityPhysics2D = UnityEngine.Physics2D;

namespace Main.Game.Collision
{
    public static class UnityCollision
    {
        public static int GetLayer(this string layerName) =>
            LayerMask.NameToLayer(layerName);

        public static int GetLayerMask(this string layerName) =>
            1 << LayerMask.NameToLayer(layerName);

        public static bool NotSelf_C(Collider2D component, Component tSubject) =>
            component.transform != tSubject.transform;

        public static Func<Collider2D, bool> NotSelf_C(Component tSubject) =>
            component => component.transform != tSubject.transform;

        public static Func<RaycastHit2D, bool> NotSelf_R(Component tSubject) =>
            hit2D => hit2D.transform != tSubject.transform;

        public static Func<Collider2D, bool> IsLayer_C(string layerName) =>
            component => component.transform.gameObject.layer == GetLayer(layerName);

        public static Func<RaycastHit2D, bool> IsLayer_R(string layerName) =>
            hit2D => hit2D.transform.gameObject.layer == GetLayer(layerName);

        /// 獲取主體(tSubject)與三者以上物體的中心點距離，並回傳最小值。例如；主體和最近的地面距離多遠。
        /// 介於0~Infinity
        /// <param name="subject"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static float GetDistance(this Component subject, Vector2 direction)
        {
            // 不能用non因為會製造0值
            RaycastHit2D[] array = subject.RayCastFirst(direction);

            if (array.IsEmpty())
                return Mathf.Infinity;

            // return array!.Get(t => t.distance).Prepend(Mathf.Infinity).Min();
            return array!.Get(t => t.distance).Min();
        }

        /// 獲取主體(tSubject)與三者以上物體的中心點距離，並回傳最小值。例如；主體和最近的地面距離多遠。
        /// 介於0~Infinity
        /// <param name="subject"></param>
        /// <param name="direction"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static float GetDistance(this Component subject, Vector2 direction,
            [NotNull] Func<RaycastHit2D, bool> filter)
        {
            // 不能用non因為會製造0值
            RaycastHit2D[] array = subject.RayCastAll(direction);

            if (array.IsEmpty())
                return Mathf.Infinity;

            array = array?.Filter(filter).ToArray();

            // return array!.Get(t => t.distance).Prepend(Mathf.Infinity).Min();
            return array!.Get(t => t.distance).Min();
        }

        /// 獲取主體(tSubject)與三者以上物體的中心點距離，並回傳最小值。例如；主體和最近的地面距離多遠。
        /// 介於0~Infinity
        /// <param name="subject"></param>
        /// <param name="direction"></param>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public static float GetDistance(this Component subject, Vector2 direction,
            [NotNull] string layerName) =>
            subject.GetDistance(direction, hit => hit.transform.CompareLayer(layerName));

        public static RaycastHit2D[] RayCastFirst(this Component subject, Vector2 direction) =>
            UnityPhysics2D.RaycastAll(subject.transform.position + new Vector3(0, 0.0001f, 0), direction)
                .Filter(NotSelf_R(subject)).ToArray();

        /// <summary>
        /// 與unity直接相關
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static RaycastHit2D[] RayCastAll(this Component subject, Vector2 direction) =>
            UnityPhysics2D.RaycastAll(subject.transform.position + new Vector3(0, 0.0001f, 0), direction)
                .Filter(NotSelf_R(subject)).ToArray();

        /// <summary>
        /// 與unity直接相關
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static Collider2D[] CircleCastAll(this Component subject, float radius) =>
            (Collider2D[]) UnityPhysics2D.OverlapCircleAll(subject.transform.position + new Vector3(0, 0.1f, 0), radius)
                ?.Filter(NotSelf_C(subject)).ToArray();

        /// <summary>
        /// 與unity直接相關
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Collider2D[] BoxCastAll(this Component subject, Vector2 size) =>
            (Collider2D[]) UnityPhysics2D.OverlapBoxAll(subject.transform.position, size, 0)
                ?.Filter(NotSelf_C(subject)).ToArray();

// tt
        public static Collider2D[] BoxCastAll(this Component subject, Vector2 size,
            [NotNull] Func<Collider2D, bool> filter)
        {
            Collider2D[] _ = new Collider2D[10];
            return UnityPhysics2D.OverlapBoxNonAlloc(subject.transform.position, size, 0, _) != 0
                ? _.Filter(collider2D => collider2D != null && NotSelf_C(collider2D, subject) && filter(collider2D))
                : null;
            /*return (Collider2D[]) UnityPhysics2D.OverlapBoxAll(subject.transform.position, size, 0)
                ?.Filter(collider2D => NotSelf_C(collider2D, subject) && filter(collider2D)).ToArray();*/
        }

        /// 條件式
        public static RaycastHit2D[] RayCastAll(this Component subject, Vector2 direction,
            Func<RaycastHit2D, bool> filter) =>
            subject.RayCastAll(direction)
                ?.Filter(filter).ToArray();

        /// 條件式
        public static Collider2D[] CircleCastAll(this Component subject, float radius,
            Func<Collider2D, bool> filter) =>
            subject.CircleCastAll(radius)
                .Filter(filter).ToArray();

        public static RaycastHit2D[] RayCastAll(this Component subject, Vector2 direction,
            string layerName) =>
            subject.RayCastAll(direction)
                .Filter(IsLayer_R(layerName)).ToArray();

        public static Collider2D[] CircleCastAll(this Component subject, float radius,
            string layerName) =>
            (Collider2D[]) subject.CircleCastAll(radius)
                .Filter(IsLayer_C(layerName)).ToArray();
    }
}