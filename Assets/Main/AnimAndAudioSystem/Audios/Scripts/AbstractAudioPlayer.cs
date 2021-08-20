using Main.AnimAndAudioSystem.Audios.Scripts.Attribute;
using UnityEngine;

namespace Main.AnimAndAudioSystem.Audios.Scripts
{
    public abstract class AbstractAudioPlayer : ScriptableObject
    {
        [SerializeField] protected RangedFloat volume;
        [SerializeField] protected RangedFloat pitch;
        private static AudioSource CreateAudioSource() =>
            new GameObject().AddComponent<AudioSource>();
        private AudioSource audioSource;
        public AudioSource AudioSource
        {
            get
            {
                if (audioSource == false) audioSource = CreateAudioSource();
                return audioSource;
            }
        }
        ~AbstractAudioPlayer()
        {
            if (audioSource != null)
                Destroy(audioSource.transform);
        }
    }
}