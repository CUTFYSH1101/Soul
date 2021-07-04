using Main.Util;
using UnityEngine;

namespace Main.Attribute
{
    public abstract class AbstractAudioPlayer : ScriptableObject
    {
        [SerializeField] protected RangedFloat volume;
        [SerializeField] protected RangedFloat pitch;
        private AudioSource audioSource;
        public AudioSource AudioSource
        {
            get
            {
                if (audioSource == false)
                    audioSource = UnityTool.CreateAudioSource();
                return audioSource;
            }
        }
        ~AbstractAudioPlayer()
        {
            if (audioSource!=null)
                Destroy(audioSource.transform);
        }
    }
}