using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Main.Entity.Controller;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Main.Util
{
    /// 獲取遊戲物件組件相關方法
    public static partial class UnityTool
    {
        public static bool HasChildren(this Component flag)
        {
            return flag.GetComponentsInChildren<Transform>().Length != 0;
        }

        public static bool HasComponent<T>(this Component flag) where T : Component
        {
            bool hasXXX = flag.GetComponent<T>() != null;
            if (!hasXXX)
                Debug.Log(flag.name + " 沒有 " + typeof(T));
            return hasXXX;
        }

        /// 當組件A身上具有組件B，取得組件B，否則Log警告
        public static T GetOrLogComponent<T>(this Component flag) where T : Component
        {
            T type;
            bool hasXXX = (type = flag.GetComponent<T>()) != null;
            if (!hasXXX)
                Debug.Log(flag.name + " 沒有 " + typeof(T));
            return type;
        }

        /// <summary>
        /// 當組件A身上具有組件B，取得組件B，否則設定傳入狀態為否。
        /// EX: isGrounded。
        /// </summary>
        /// <param name="conditional">狀態機。EX: isGrounded</param>
        public static T GetOrSetFalseComponent<T>(this Component flag, ref bool conditional) where T : Component
        {
            T type;
            conditional = (type = flag.GetComponent<T>()) != null;
            return type;
        }

        public static T GetOrAddComponent<T>(this Component flag) where T : Component
        {
            T type;
            bool hasComponent = (type = flag.GetComponent<T>()) != null;
            if (hasComponent)
            {
                return type;
            }
            else
            {
                return flag.gameObject.AddComponent<T>();
            }
        }

        /// <summary>
        /// 遞迴尋找，一層層向上尋找是否父物件群中至少一個含有某組件
        /// </summary>
        /// <param name="flag"></param>
        /// <typeparam name="T"></typeparam>
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


        /// <summary>
        /// 遞迴尋找並Log所有父物件，不包括container
        /// </summary>
        /// <param name="objectToCheckParent"></param>
        public static void LogParent(this Transform objectToCheckParent)
        {
            objectToCheckParent = objectToCheckParent.parent;
            if (objectToCheckParent != null)
            {
                int i = 1;
                while (true)
                {
                    Debug.Log("Reached parent " + i + ": " + objectToCheckParent.name);
                    if (objectToCheckParent.parent != null)
                    {
                        objectToCheckParent = objectToCheckParent.parent;
                        ++i;
                    }
                    else
                        break;
                }

                Debug.Log("container object is " + objectToCheckParent.name);
            }
            else
                Debug.Log("Instance object has no parents");
        }

        /// <summary>
        /// Log類別內所有資料，並回傳該字串
        /// </summary>
        /// <param name="type"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string LogProperties<T>(this T type)
        {
            //實例化一個字串
            string text = null;
            //獲得一個array物件
            FieldInfo[] data = type.GetFieldInfos();
            //把字符串成字串
            text += "【" + type.GetType() + "】： " + Environment.NewLine;
            foreach (var item in data)
                text += item.Name + " " + item.GetValue(type) + Environment.NewLine;
            //log字串
            Debug.Log(text);
            //end
            return text;
        }

        /// <summary>
        /// Log一個List內所有元素
        /// </summary>
        public static string LogList<T>(this List<T> origin)
        {
            string text = null;
            text += "陣列名：" + nameof(origin) + "\n";
            foreach (var element in origin)
            {
                text += "\t" + "第 " + origin.IndexOf(element) + " 個 " + element + "\n";
            }

            Debug.Log(text);
            return text;
        }

        /// <summary>
        /// Log一個嵌套List內所有元素
        /// </summary>
        /// <param name="origin"></param>
        /// <typeparam name="T"></typeparam>
        public static string LogNestedList<T>(this List<List<T>> origin)
        {
            string text = null;
            foreach (var list in origin)
            {
                text += "第 " + origin.IndexOf(list) + " 個鎮列有 " + list.Count + " 個元素\n";
                foreach (var element in list)
                {
                    text += "\t" + "第 " + list.IndexOf(element) + " 個 " + element + "\n";
                }

                text += "\n";
            }

            Debug.Log(text);
            return text;
        }

        /// <summary>
        /// 沒用不要用！！
        /// </summary>
        /// <param name="objectToCheck"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static bool HasMethod(this object objectToCheck, string methodName)
        {
            var type = objectToCheck.GetType();
            return type.GetMethod(methodName) != null;
        }
    }

    public static partial class UnityTool
    {
        //TODO:
        /// 獲取所有子物件群中所有的某物件群
        public static T[] GetComponents<T>(this Component container, string name) where T : Component
        {
            if (container.IsEmpty())
            {
                var result = GameObject.FindObjectsOfType<T>();
                result = result.Where(obj => obj.name == name).ToArray();
                return result;
            }
            else
            {
                // 先找名稱符合的遊戲物件，接著確認是否含有某組件
                var flag = container.GetComponentsInChildren<Transform>();
                var result = flag
                    .Where(obj => obj.name == name)
                    .Select(obj => obj.GetComponents<T>())
                    .ToArray()
                    .ToSingleArray();
                return result;
            }
        }
    }
    
}