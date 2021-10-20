using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Main.Util;
using Main.Entity.Creature.Sub;
using Main.Res.Script.Audio;
using UnityEngine;

namespace Main.Entity.Creature
{
    public class Factory : IData
    {
        public enum EnumCreatureTag
        {
            Player,
            Monster,
            MiddleMonster
        }

        // public (EnumCreatureTag tag, string unityTag) data;

        public static Dictionary<Transform, Creature> CreatureDictionary { get; } =
            new();

        // 1.產生data
        // 2.收集場景物件
        // 3.指派同一個數值到含有相同data的場景物件，並存到字典中
        /// <summary>
        /// 產生角色物件並添加到字典中
        /// 回傳新增的物件
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="audioPlayer"></param>
        public static Dictionary<Transform, Creature> CreateCreatures(EnumCreatureTag tag,
            DictAudioPlayer audioPlayer = null)
        {
            var objs = FindObjArray(FindData(tag));
            var append = new Dictionary<Transform, Creature>();
            switch (tag)
            {
                case EnumCreatureTag.Player:
                    foreach (var obj in objs)
                        if (!CreatureDictionary.ContainsKey(obj))
                        {
                            var (transform, creature) = Append(obj, new PlayerBuilder(obj, audioPlayer));
                            append.Add(transform, creature);
                        }

                    break;
                case EnumCreatureTag.Monster:
                    foreach (var obj in objs)
                        if (!CreatureDictionary.ContainsKey(obj))
                        {
                            var (transform, creature) = Append(obj, new MonsterBuilder(obj));
                            append.Add(transform, creature);
                        }

                    break;
                case EnumCreatureTag.MiddleMonster:
                    foreach (var obj in objs)
                        if (!CreatureDictionary.ContainsKey(obj))
                        {
                            var (transform, creature) = Append(obj, new MiddleMonsterBuilder(obj));
                            append.Add(transform, creature);
                        }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tag), tag, "超出範圍");
            }

            return append;
        }

        /// <summary>
        /// 搜索任何已經生成的物件
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [CanBeNull]
        public static Creature FindCreatureByObj(Transform obj) =>
            CreatureDictionary.ContainsKey(obj) ? CreatureDictionary[obj] : null;
        [CanBeNull]
        public static Creature[] FindCreaturesByTag(EnumCreatureTag tag)
        {
            var newArray = new Queue<Creature>();
            foreach (var obj in FindObjArray(FindData(tag)))
                if (CreatureDictionary.ContainsKey(obj))
                    newArray.Enqueue(FindCreatureByObj(obj));
            return newArray.ToArray();//場景物件當key，去搜尋Dictionary裡的物件
        }
        private static (EnumCreatureTag tag, string unityTag) FindData(EnumCreatureTag tag)
            => (tag, tag.ToString());

        private static Transform[] FindObjArray((EnumCreatureTag tag, string unityTag) data) =>
            GameObject.FindGameObjectsWithTag(data.unityTag).Get(obj => obj.transform);

        private static (Transform obj, Creature creature) Append(Transform obj, Builder builder)
        {
            var _ = (obj, new Director(builder).GetResult());
            CreatureDictionary.Add(obj, _.Item2);
            return _;
        }

        public EnumDataTag Tag => EnumDataTag.Factory;
    }
}