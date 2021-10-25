using System;
using System.Collections.Generic;
using System.Linq;
using Main.Blood;
using Main.Res.Script.Util;
using Main.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main.Res.Script.Audio
{
    [CreateAssetMenu(fileName = "NaAudioPlayer", menuName = "Registration List/Audio Player/Atk Normal Audio Player", order = 1)]
    public class DictAudioPlayer : AbstractAudioPlayer
    {
        [Serializable]
        public class Dictionary
        {
            
            [Serializable]
            public class Element
            {
                [SerializeField] public Key key;
                [SerializeField] public AudioClip clip;
            }

            [SerializeField] private Element[] clips;

            // 注意Key值必須唯一
            private Dictionary<Key, AudioClip> Instance =>
                clips.ToDictionary(clip => clip.key, clip => clip.clip);

            public int Length => Instance.Count;
            
            public AudioClip Get(Key key)
            {
                try
                {
                    return Instance[key];
                }
                catch (KeyNotFoundException)
                {
                    return null;
                }
            }

            public AudioClip GetByRandom() =>
                clips[Random.Range(0, clips.Length)].clip;
        }

        public enum Key
        {
            NaDirect,
            NaSquare,
            NaCross,
            NaCircle,
            Dash,
            WallJump
        }

        [SerializeField] protected Dictionary clips;

        public void Play(BloodType shape)
        {
            switch (shape)
            {
                case BloodType.Direct:
                    Play(Key.NaDirect);
                    return;
                case BloodType.CSquare:
                    Play(Key.NaSquare);
                    return;
                case BloodType.CCircle:
                    Play(Key.NaCircle);
                    return;
                case BloodType.CCrossx:
                    Play(Key.NaCross);
                    return;
                default:
                    Debug.LogError("超出範圍");
                    return;
            }
        }

        public void Play(Key key)
        {
            var source = AudioSource;
            if (clips.IsEmpty() || clips.Length < 3 || source.IsEmpty() || clips.Get(key) == null) return; // 避免資料為空
            source.clip = clips.Get(key);
            // source.volume = 1;
            source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
            source.Play();
        }

        public void Play(Key key, float nowDistance, float maxDistance)
        {
            var source = AudioSource;
            if (clips.IsEmpty() || clips.Length < 3 || source.IsEmpty() || clips.Get(key) == null) return;
            source.clip = clips.Get(key);
            source.volume = Util.Math.Remap(volume.minValue, volume.maxValue, maxDistance, 0, nowDistance).ToFloat();
            source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
            source.Play();
        }
    }
}