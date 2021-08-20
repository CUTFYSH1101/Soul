using System;
using System.Collections.Generic;
using System.Linq;
using Main.AnimAndAudioSystem.Main.Common;
using Main.AnimAndAudioSystem.Main.Util;
using Main.Util;
using UnityEngine;
using Math = Main.AnimAndAudioSystem.Main.Util.Math;
using Random = UnityEngine.Random;

namespace Main.AnimAndAudioSystem.Audios.Scripts
{
    [CreateAssetMenu(fileName = "new Simple Player", menuName = "Audio Player/Simple", order = 1)]
    public class DictionaryAudioPlayer : AbstractAudioPlayer
    {
        [Serializable]
        public class Dictionary
        {
            [Serializable]
            public class Item
            {
                [SerializeField] public Key key;
                [SerializeField] public AudioClip clip;
            }

            [SerializeField] private Item[] clips;

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

        public void Play(EnumSymbol symbol)
        {
            switch (symbol)
            {
                case EnumSymbol.Direct:
                    Play(Key.NaDirect);
                    return;
                case EnumSymbol.Square:
                    Play(Key.NaSquare);
                    return;
                case EnumSymbol.Circle:
                    Play(Key.NaCircle);
                    return;
                case EnumSymbol.Cross:
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
            source.volume = Math.Remap(volume.minValue, volume.maxValue, maxDistance, 0, nowDistance).ToFloat();
            source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
            source.Play();
        }
    }
}