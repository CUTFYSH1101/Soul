using Main.Res.Script.Audio.Attribute;
using UnityEngine;

namespace Main.Res.Script.Audio
{
    public abstract class AbstractAudioPlayer : ScriptableObject
    {
        [SerializeField] protected RangedFloat volume;
        [SerializeField] protected RangedFloat pitch;
        private static AudioSource CreateAudioSource() =>
            new GameObject().AddComponent<AudioSource>();
        private AudioSource _audioSource;
        public AudioSource AudioSource
        {
            get
            {
                if (_audioSource == false) _audioSource = CreateAudioSource();
                return _audioSource;
            }
        }
        ~AbstractAudioPlayer()
        {
            if (_audioSource != null)
                Destroy(_audioSource.transform);
        }
    }
}