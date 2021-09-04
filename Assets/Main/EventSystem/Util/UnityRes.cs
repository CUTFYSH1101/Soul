using Main.AnimAndAudioSystem.Audios.Scripts;
using Main.Entity.Creature;
using Main.EventSystem.Event.UIEvent.CameraShaker;
using Main.EventSystem.Event.UIEvent.QTE;
using Main.Game;
using UnityEngine;

namespace Main.EventSystem.Util
{
    public static partial class UnityRes
    {
        public static MonoBehaviour GetMonoClass(this Creature container)
            => container.Transform.GetOrAddComponent<MonoClass>();

        public static MonoBehaviour GetMonoClass() =>
            GameObject.Find("GameLoop").transform.GetMonoClass();

        public static MonoBehaviour GetMonoClass(this Transform container) =>
            container == null ? null : container.GetOrAddComponent<MonoClass>();

        public static UnityRb2D GetRb2D(this Transform container) =>
            container == null ? null : container.GetOrAddComponent<UnityRb2D>();
        
        public static DictionaryAudioPlayer GetNormalAttackAudioPlayer(string resPath) =>
            Resources.Load<DictionaryAudioPlayer>(resPath);
        public static DictionaryAudioPlayer GetNormalAttackAudioPlayer() =>
            GetNormalAttackAudioPlayer("AttackAudios/NAAudioPlayer");

        public static CameraShaker GetCameraShaker() =>
            CameraShaker.Instance;

        public static QteImageList GetQteImageList() =>
            Resources.Load<QteImageList>("UI/Symbols/QTEImageList");
    }
}