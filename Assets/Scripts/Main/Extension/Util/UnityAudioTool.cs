using Main.Attribute;
using UnityEngine;

namespace Main.Util
{
    public class UnityAudioTool
    {
        private readonly AudioPlayer audioPlayer;
        private AudioSource audioSource;
        private AudioSource AudioSource
        {
            get
            {
                if (audioSource == null)
                {
                    var newGo = new GameObject().transform;
                    audioSource = newGo.GetOrAddComponent<AudioSource>();
                    Object.Destroy(newGo, 30);
                }

                return audioSource;
            }
        }

        public UnityAudioTool(AudioPlayer audioPlayer)
        {
            this.audioPlayer = audioPlayer;
        }

        public void Play() => audioPlayer.Play(audioSource);

        public void Play(float nowDistance, float maxDistance) => audioPlayer.Play(audioSource, nowDistance, maxDistance);

        ~UnityAudioTool()
        {
            if (audioSource != null)
                UnityEngine.Object.Destroy(audioSource);
        }
    }
}