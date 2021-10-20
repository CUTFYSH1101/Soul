using UnityEngine;

namespace Test.Scene.Editor
{
    public class SceneAttribute : PropertyAttribute
    {
        public static string GetSceneNameByPath(string path)
        {
            string[] array = path.Split('/');
            return array[array.Length - 1].Replace(".unity", "");
        }
    }
}