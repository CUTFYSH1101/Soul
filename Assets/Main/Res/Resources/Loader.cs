using System;
using JetBrains.Annotations;
using Main.Res.Script.ScriptableObjectList;
using Main.Util;
using Test.Scene.Scripts.Game;
using UnityEngine;

namespace Main.Res.Resources
{
    [CreateAssetMenu(fileName = "Loader", menuName = "Resource Loader", order = 0)]
    public class Loader : AbsObjList<Loader.Tag, ScriptableObject>
    {
        [CanBeNull]
        private static Loader Instance => UnityEngine.Resources.Load<Loader>("Loader");

        public static T Find<T>(Tag tag) where T : ScriptableObject
        {
            var _ = Instance;
            if (_ == null) throw new Exception("在Resources資料夾中，尋找不到Loader物件！");
            return _.Find(tag) as T;
        }

        public enum Tag
        {
            BloodQte,
            Audio,
            Scene
        }
    }
}