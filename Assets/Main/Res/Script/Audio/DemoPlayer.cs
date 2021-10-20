using Main.Util;
using Main.Res.Script.Util;
using UnityEngine;
using Math = Main.Res.Script.Util.Math;
using Random = UnityEngine.Random;

namespace Main.Res.Script.Audio
{
    [CreateAssetMenu(fileName = "DemoPlayer", menuName = "Registration List/Audio Player/Demo Player", order = 0)]
    public class DemoPlayer : AbstractAudioPlayer
    {
        [SerializeField] protected AudioClip[] clips;

        public void Play(AudioSource source)
        {
            if (clips.IsEmpty() || source.IsEmpty()) return;
            source.clip = clips[Random.Range(0, clips.Length)];
            // 隨機音量
            source.volume = Random.Range(volume.minValue, volume.maxValue);
            source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
            source.Play();
        }

        /// 在 AudioSource 上播放任一 AudioClip
        public void Play(AudioSource source, float nowDistance, float maxDistance)
        {
            if (clips.IsEmpty() || source.IsEmpty()) return;
            source.clip = clips[Random.Range(0, clips.Length)];
            // 根據距離線性插值音量，越遠聲音越小聲，反之亦然
            source.volume = Math.Remap(volume.minValue, volume.maxValue, maxDistance, 0, nowDistance).ToFloat();
            source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
            source.Play();
        }
    }
}