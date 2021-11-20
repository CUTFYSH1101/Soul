using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Main.Util;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Main.Game
{
    /// 獲取遊戲物件組件相關方法
    public static partial class UnityTool
    {
        /// 搭配where, firstOrNull, firstOrDefault語法，篩選除了自身以外的物件
        private static Func<T, bool> NotSelf<T>(this T tSubject) where T : Component =>
            component => component.transform != tSubject.transform;

        // ======
        // 
        // ======
        public static bool HasComponent<T>(this Component flag) where T : Component
        {
            bool hasXxx = flag.GetComponent<T>() != null;
            if (!hasXxx)
                Debug.Log(flag.name + " 沒有 " + typeof(T));
            return hasXxx;
        }

        public static T GetComponent<T>(this RaycastHit2D hit2D) =>
            hit2D.transform.GetComponent<T>();

        /// 當組件A身上具有組件B，取得組件B，否則添加後獲取
        public static T GetOrAddComponent<T>([CanBeNull] this Component flag) where T : Component
        {
            if (flag == null)
                return null;
            T type;
            var hasComponent = (type = flag.GetComponent<T>()) != null;
            return hasComponent
                ? type
                : flag.gameObject.AddComponent<T>();
        }

        /// 當組件A身上具有組件B，取得組件B，否則Log警告
        public static T GetOrLogComponent<T>(this Component flag) where T : Component
        {
            T type;
            var hasComponent = (type = flag.GetComponent<T>()) != null;
            if (!hasComponent) Debug.Log(flag.name + " 沒有 " + typeof(T));
            return type;
        }

        /// 當組件A身上具有組件B，取得組件B，否則設定傳入狀態為否。
        /// EX: isGrounded。
        /// <param skillName="conditional">狀態機。EX: isGrounded</param>
        public static T GetOrSetFalseComponent<T>(this Component flag, ref bool conditional) where T : Component
        {
            T type;
            conditional = (type = flag.GetComponent<T>()) != null;
            return type;
        }

        public static T GetComponent<T>(string name) where T : Component =>
            GetComponents<T>(name)?.FirstOrNull();

        public static T[] GetComponents<T>(string name) where T : Component =>
            Object.FindObjectsOfType<T>() // 從場景中所有物件
                ?.Filter(obj => obj.name == name);

        public static T[] GetComponents<T>([NotNull] Func<T, bool> filter) where T : Component =>
            Object.FindObjectsOfType<T>() // 從場景中所有物件
                ?.Filter(filter);

        public static T GetComponentByPath<T>([NotNull] string path) where T : Component
        {
            if (!path.Contains('/')) return GetComponent<T>(path); // 該物件為根物件

            var names = path.Split('/');
            var o = GetComponent<Transform>(path);
            foreach (var name in names)
            {
                o = o.GetFirstComponentInChildren<Transform>(name);
                if (o == null) return null; // 路徑錯誤
            }

            return o.GetComponent<T>();
        }

        // ======
        // 
        // ======
        public static bool HasChildren(this Component flag) =>
            flag.GetComponentsInChildren<Transform>().Any(NotSelf(flag)); // 不包含自身

        /// 獲取所有子物件群中所有的某物件群
        public static T[] GetComponentsInChildren<T>(this Component container, string name) where T : Component
        {
            return container.IsEmpty()
                ? GetComponents<T>(name)
                : container.GetComponentsInChildren<T>() // 從container底下尋找
                    .Filter(obj => obj.name == name);
        }

        public static T GetFirstComponentInChildren<T>(this Component container, string name) where T : Component
        {
            var list = GetComponentsInChildren<T>(container, name);
            return list.IsEmpty() ? null : list[0];
        }

        /// foreach尋找，向下尋找是否子物件群中至少一個含有某組件
        public static bool HasComponentInChildren<T>(this Component container) where T : Component
        {
            // 避免container為空
            if (!container.IsEmpty())
                return container.GetComponentsInChildren<T>().Any(container.NotSelf());
            Debug.LogError("錯誤獲取子對象，子對象為空！");
            return false;
        }

        // ======
        // 
        // ======
        /// <summary>
        /// 遞迴尋找並所有父物件，不包括container
        /// </summary>
        /// <param skillName="leaf"></param>
        public static Transform[] GetParents(this Transform leaf)
        {
            if (leaf == null)
                return null;
            var stack = new Stack<Transform>();
            var flag = leaf.transform;
            stack.Push(flag);
            while ((flag = flag.parent) != null)
                stack.Push(flag);
            return stack.ToArray();
        }

        public static T GetComponentInParent<T>(this Component component, Func<T, bool> filter) where T : class =>
            (T) component.GetComponentsInParent<T>().FirstOrNull(filter);

        /// <summary>
        /// 遞迴尋找，一層層向上尋找是否父物件群中至少一個含有某組件，注意：不包含自身(這點不同於unity的)
        /// </summary>
        public static bool HasComponentInParent<T>(this Component component) where T : Component
        {
            var stack = new Stack<T>();
            var flag = component.GetComponent<T>();
            while (flag.transform.parent != null &&
                   (flag = flag.transform.parent.GetComponent<T>()) != null)
                stack.Push(flag);
            return stack.Any(); // 是否有除此之外的父物件
        }

        public static Transform GetRoot(this Component component)
        {
            var stack = new Stack<Transform>();
            var flag = component.transform;
            stack.Push(flag);
            while ((flag = flag.parent) != null)
                stack.Push(flag);
            return stack.Pop();
        }
    }

    public static partial class UnityTool
    {
        /// 音效相關
        public static AudioSource CreateAudioSource()
        {
            var newGO = new GameObject();
            var audioSource = newGO.AddComponent<AudioSource>();
            // Object.Destroy(newGO, 5);
            return audioSource;
        }

        /// 用法同CompareTag
        public static bool CompareLayer(this Component component, string layerName) =>
            component.gameObject.layer == LayerMask.NameToLayer(layerName);
    }

    public static partial class UnityTool
    {
        public static void SetDontDestroy(Component target, bool open = true)
        {
            if (open)
                Object.DontDestroyOnLoad(target.gameObject);
            else
                Object.Destroy(target.gameObject, 0.1f);
        }
    }
}