using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Main.Extension.Util;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Main.Util
{
    /// 獲取遊戲物件組件相關方法
    public static partial class UnityTool
    {
        // ======
        // 
        // ======
        public static bool HasComponent<T>(this Component flag) where T : Component
        {
            bool hasXXX = flag.GetComponent<T>() != null;
            if (!hasXXX)
                Debug.Log(flag.name + " 沒有 " + typeof(T));
            return hasXXX;
        }

        public static T GetComponent<T>(this RaycastHit2D hit2D) =>
            hit2D.transform.GetComponent<T>();

        /// 當組件A身上具有組件B，取得組件B，否則添加後獲取
        public static T GetOrAddComponent<T>(this Component flag) where T : Component
        {
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

        public static T[] GetComponents<T>(string name) where T : Component =>
            Object.FindObjectsOfType<T>() // 從場景中所有物件
                .Filter(obj => obj.name == name);


        // ======
        // 
        // ======
        public static bool HasChildren(this Component flag)
        {
            return flag.GetComponentsInChildren<Transform>().Length != 0;
        }

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
            if (container.IsEmpty())
            {
                Debug.LogError("錯誤獲取子對象，子對象為空！");
                return false;
            }

            // 避免不含任何子物件
            Transform[] _flag = container.GetComponentsInChildren<Transform>();
            if (_flag.Length == 0)
            {
                Debug.Log($"{container.name} 沒有 任何子物件");
                return false;
            }

            bool hasComponent = _flag.Any(obj => obj.HasComponent<T>());
            if (!hasComponent)
            {
                Debug.Log($"{container.name} 沒有 {typeof(T)}類型的子物件");
            }

            return hasComponent;
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
            List<Transform> list = new List<Transform>();
            leaf = leaf.parent;
            if (leaf != null)
            {
                int i = 1;
                list.Add(leaf);
                while (true)
                {
                    if (leaf.parent != null)
                    {
                        leaf = leaf.parent;
                        ++i;
                        list.Add(leaf);
                        Debug.Log("Reached parent " + i + ": " + leaf.name);
                    }
                    else
                        break;
                }

                Debug.Log("container object is " + leaf.name);
            }
            else
                Debug.Log("instance object has no parents");

            return list.ToArray();
        }

        public static T GetComponentInParent<T>(this Component component, Func<T, bool> filter) =>
            (T) component.GetComponentsInParent<T>().FirstOrNull(filter);

        /// <summary>
        /// 遞迴尋找，一層層向上尋找是否父物件群中至少一個含有某組件
        /// </summary>
        /// <param skillName="flag"></param>
        /// <typeparam skillName="T"></typeparam>
        /// <returns></returns>
        public static bool HasComponentInParent<T>(this Component flag) where T : Component
        {
            /*
             * 尋找是否有父母？或是回傳false
             * 尋找是否有組件？或是繼續找
             */
            Transform _flag = flag.transform.parent;
            if (_flag != null)
            {
                // int i = 1;
                if (_flag.GetComponent<T>() != null)
                    return true;
                while (true)
                {
                    if (_flag.GetComponent<T>() != null)
                        return true;
                    if (_flag.parent != null)
                    {
                        _flag = _flag.parent;
                        // ++i;
                        if (_flag.GetComponent<T>() != null)
                            return true;
                    }
                    else
                        break;
                }

                Debug.Log(flag.name + " 沒有 " + typeof(T));
                return false;
            }

            Debug.Log(flag.name + " 沒有 " + typeof(T));
            return false;
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
            component.gameObject.layer == layerName.GetLayer();
    }
}