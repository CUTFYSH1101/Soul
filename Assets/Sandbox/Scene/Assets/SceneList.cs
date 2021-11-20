using System;
using Main.Res.Resources;
using Main.Res.Script.ScriptableObjectList;
using Test.Scene.Editor;
using UnityEngine;
namespace Test.Scene.Scripts.Game
{
    [CreateAssetMenu(fileName = "SceneList", menuName = "Registration List/Scene", order = 0)]
    public class SceneList : AbsObjList<EnumMapTag, SceneAsset>
    {
        public static string ReflectSceneName(EnumMapTag key) =>
            Loader.Find<SceneList>(Loader.Tag.Scene)?.Find(key).Name;
    }

    public enum EnumMapTag
    {
        MainMenu,
        VictoryMap1,
        VictoryMap2,
        Tutorial,
        MapTest,
    }

    public static class SceneListTagExtensions
    {
        public static string ReflectSceneName(this EnumMapTag key) =>
            SceneList.ReflectSceneName(key);
    }

    [Serializable]
    public class SceneAsset
    {
        [Scene,SerializeField] public string scene;
        public string Name => SceneAttribute.GetSceneNameByPath(scene);
    }
}