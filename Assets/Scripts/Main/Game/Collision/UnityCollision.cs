using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Main.Util
{
    public static class UnityCollision
    {
        public static int GetLayer(this string layerName) =>
            LayerMask.NameToLayer(layerName);

        public static int GetLayerMask(this string layerName) =>
            1 << LayerMask.NameToLayer(layerName);

        /// 獲取主體(subject)與三者以上物體的中心點距離，並回傳最小值。例如；主體和最近的地面距離多遠。
        /// 介於0~Infinity
        /// <param name="subject"></param>
        /// <param name="direction"></param>
        /// <param name="layerName"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static float GetDistance(this Component subject, Vector2 direction,
            [CanBeNull] string layerName = null, [CanBeNull] Func<RaycastHit2D, bool> predicate = null)
        {
            // 不能用non因為會製造0值
            RaycastHit2D[] array = subject.RayCastAll(direction);

            if (array.IsEmpty())
                return Mathf.Infinity;

            if (layerName != null)
                array = array?.Where(hit => hit.transform.gameObject.layer == layerName.GetLayer()).ToArray(); // 是否能夠執行

            if (predicate != null)
                array = array?.Where(predicate).ToArray();

            return array!.Select(t => t.distance).Prepend(Mathf.Infinity).Min();
        }

        /// <summary>
        /// 與unity直接相關
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static RaycastHit2D[] RayCastAll(this Component subject, Vector2 direction) =>
            Physics2D.RaycastAll(subject.transform.position + new Vector3(0, 0.1f, 0), direction)
                .Where(hit => hit.transform != subject.transform).ToArray();

        /// <summary>
        /// 與unity直接相關
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static Collider2D[] CircleCastAll(this Component subject, float radius) =>
            Physics2D.OverlapCircleAll(subject.transform.position + new Vector3(0, 0.1f, 0), radius)
                .Where(hit => hit.transform != subject.transform).ToArray();

        /// <summary>
        /// 與unity直接相關
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Collider2D[] BoxCastAll(this Component subject, Vector2 size) =>
            Physics2D.OverlapBoxAll(subject.transform.position, size, 0)
                .Where(hit => hit.transform != subject.transform).ToArray();

        public static RaycastHit2D[] RayCastAll(this Component subject, Vector2 direction,
            Func<RaycastHit2D, bool> predicate) =>
            subject.RayCastAll(direction).Where(predicate).ToArray();

        public static RaycastHit2D[] RayCastAll(this Component subject, Vector2 direction,
            string layerName) =>
            subject.RayCastAll(direction).Where(hit => hit.transform.gameObject.layer == GetLayer(layerName))
                .ToArray();

        public static Collider2D[] CircleCastAll(this Component subject, float radius,
            Func<Collider2D, bool> predicate) =>
            subject.CircleCastAll(radius).Where(predicate).ToArray();

        public static Collider2D[] CircleCastAll(this Component subject, float radius,
            string layerName) =>
            subject.CircleCastAll(radius).Where(hit => hit.gameObject.layer == GetLayer(layerName)).ToArray();

        public static Collider2D[] BoxCastAll(this Component subject, float w, float h,
            Func<Collider2D, bool> predicate) =>
            subject.BoxCastAll(new Vector2(w, h)).Where(predicate).ToArray(); // w:5<->.85, h:1.5
    }
}