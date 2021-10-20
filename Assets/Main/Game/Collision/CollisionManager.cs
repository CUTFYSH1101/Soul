using System;
using System.Collections.Generic;
using System.Linq;
using Main.Util;
using UnityEngine;
using UnityEngine.Rendering;
using UnityPhysics2D = UnityEngine.Physics2D;

namespace Main.Game.Collision
{
    public static class CollisionManager
    {
        /*private static void Update()
        {
            Debug.Log(this.GetDistance(Vector2.down));
            Debug.Log(this.CircleCastAll(this.transform.position, 3).Any());
            GetGrounded().LogLine();
            AnyInView(3,hit=>hit.transform).LogLine();
            AnyInView(3,"Ground").LogLine();
            AnyDistance(this).LogLine();
            Debug.Log(array.ArrayToString());
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
        
        /// <summary>
        /// 三層地板。
        /// 只讓角色能夠踩在對應的地磚上。
        /// 自帶圖層排序：依照先後距離、體型大小。
        /// </summary>
        /// <remarks>
        /// 1.環境布置出有任一tag:Ground1, Ground2, Ground3的地面
        /// 2.想要套用效果的角色，在角色物件下增加子物件，命名為"GroundChecker"，並移到最底下，使跟地面接觸（通常是腳底的位置）。通常內建已經有了可以省略這步。
        /// Ground{n}，n表示正整數值，1≦n≦3，值越大代表越前面 / 畫面位置越下面
        /// </remarks>
        public static void ThreeLevelGroundSetting() => ThreeLevelGround.CollisionIgnoreANDLayerSort();

        private static class ThreeLevelGround
        {
            private static Transform[] _footArr;
            private static Collider2D[] _groundArr;
            
            public static void CollisionIgnoreANDLayerSort()
            {
                // 尋找場景中、符合條件的碰撞物件
                FindAllCollidersInTheScene(
                    out var character1, out var character2, out var character3, 
                    out var ground1, out var ground2, out var ground3);
                // 碰撞忽略
                CollisionIgnoreANDLayerSort(ground1, character2); // 忽略 ground1 與 ground2, ground3 物件的碰撞
                CollisionIgnoreANDLayerSort(ground1, character3);
                CollisionIgnoreANDLayerSort(ground2, character1);
                CollisionIgnoreANDLayerSort(ground2, character3);
                CollisionIgnoreANDLayerSort(ground3, character1);
                CollisionIgnoreANDLayerSort(ground3, character2);
                // 圖層排序
                Sort(character1.Get(character => character.GetOrAddComponent<SortingGroup>()),
                    10);
                Sort(character2.Get(character => character.GetOrAddComponent<SortingGroup>()),
                    20);
                Sort(character3.Get(character => character.GetOrAddComponent<SortingGroup>()),
                    30);
            }
            
            private static Collider2D[] FindCharacterColliders(string tag)
            {
                var queue = new Queue<Collider2D>();
                var objs = _footArr.Filter(trans => trans.CompareTag(tag));
                foreach (var obj in objs)
                foreach (var collider2D in obj.root.GetComponents<Collider2D>())
                    queue.Enqueue(collider2D);
                return queue.ToArray();
            }

            private static Collider2D[] FindGroundColliders(string tag) =>
                _groundArr.Filter(collider => collider.CompareTag(tag));
            // 尋找場景中所有符合條件的碰撞體
            private static void FindAllCollidersInTheScene(
                out Collider2D[] character1, out Collider2D[] character2, out Collider2D[] character3,
                out Collider2D[] ground1, out Collider2D[] ground2, out Collider2D[] ground3)
            {
                _footArr = UnityTool.GetComponents<Transform>(
                    "GroundChecker");
                character1 = FindCharacterColliders("Ground1");
                character2 = FindCharacterColliders("Ground2");
                character3 = FindCharacterColliders("Ground3");

                _groundArr = UnityTool.GetComponents<Collider2D>(collider1 => collider1
                    .CompareLayer("Ground"));
                ground1 = FindGroundColliders("Ground1");
                ground2 = FindGroundColliders("Ground2");
                ground3 = FindGroundColliders("Ground3");
                // return character1;
            }
            // 碰撞忽略
            private static void CollisionIgnoreANDLayerSort(Collider2D[] array1, Collider2D[] array2)
            {
                for (var i = 0; i < array1.Length; i++)
                for (var j = 0; j < array2.Length; j++)
                    UnityPhysics2D.IgnoreCollision(array1[i], array2[j]);
            }
            
            // 角色圖層排序
            private static void Sort(SortingGroup[] array, int sortingOrder)
            {
                // first, order by a const(先設定常量基準值)
                var count = sortingOrder;
                // second, order by body size(再根據設定圖層、體型大小排序)
                foreach (var sortingGroup in array.OrderBy(element => element.GetColliderSize().sqrMagnitude)) sortingGroup.sortingOrder = count++;
            }
        }
    }
}