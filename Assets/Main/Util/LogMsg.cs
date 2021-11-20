using System.ComponentModel;

namespace Main.Util
{
    public static class LogMsg
    {
        public static string NoChildrenType<T>(this string parentName) =>
            $"{parentName} 沒有 {typeof(T)} 類型的子物件";

        public static string HasntComponent<T>(this string objName) =>
            $"{objName} 沒有 {typeof(T)} 類型的組件";

        public static string SceneCannotFind<T>() =>
            $"場景中找不到 {typeof(T)} 類型的物件";
        
        public static string SceneCannotFind(this string path) =>
            $"場景中找不到 {path} 路徑";
    }
}