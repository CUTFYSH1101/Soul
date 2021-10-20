using Main.EventLib.Sub.UIEvent.CameraShaker;
using Main.EventLib.Sub.UIEvent.QTE;
using Main.Game;
using Main.Res.Resources;
using Main.Res.Script.Audio;
using UnityEngine;
using static Main.Res.Resources.Loader;

namespace Main.Res.Script
{
    public static partial class UnityRes
    {
        public static MonoBehaviour GetMonoClass() =>
            MonoClass.Instance;

        public static ProxyUnityRb2D GetRb2D(this Transform container) =>
            container == null ? null : container.GetOrAddComponent<ProxyUnityRb2D>();

        public static DictAudioPlayer GetNormalAttackAudioPlayer() =>
           Find<DictAudioPlayer>(Loader.Tag.Audio);

        public static CameraShaker GetCameraShaker() =>
            CameraShaker.Instance;
        public static QteImageList GetQteImageList() =>
            Find<QteImageList>(Loader.Tag.BloodQte);
    }
}