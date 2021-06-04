using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Main.Entity.Controller
{
    public class Mono : MonoBehaviour
    {
        public List<PlayerAI> playerAIs;
        public List<MonsterAI> monsterAIs;

        private void Awake()
        {
            // 不支援轉職
            /*PlayerAI playerAI =
                new PlayerAI(
                    new Player(
                        GameObject.FindWithTag("Player").transform));*/
            playerAIs = Factory(Tag.Player).Cast<PlayerAI>().ToList();
            monsterAIs = Factory(Tag.Monster).Cast<MonsterAI>().ToList();
        }
        private void Update()
        {
            foreach (var AI in playerAIs) AI.Update();
            foreach (var AI in monsterAIs) AI.Update();
        }


        private enum Tag
        {
            Monster,
            Player,
            Boss    //TODO:預留
        }

        private ICreatureAI[] Factory([NotNull] Tag tag)
        {
            // A.程式碼tag, a.unity tag, 這兩者務必同名
            ICreatureAI[] AIs = null;
            // 根據A去獲取a包含的場景內激活物件
            // var transforms = GameObject.FindGameObjectsWithTag(tag.ToString()).Cast<Transform>().ToArray();
            var transforms = GameObject.FindGameObjectsWithTag(tag.ToString()).Select(p => p.transform).ToArray();
            AIs = new ICreatureAI[transforms.Length];
            // 根據A去判斷要如何實例化一個物件
            switch (tag)
            {
                case Tag.Player:
                    for (var i = 0; i < transforms.Length; i++)
                    {
                        AIs[i] = new Director(new PlayerBuilder(transforms[i])).GetResult();
                    }
                    break;
                case Tag.Monster:
                    /*for (var i = 0; i < transforms.Length; i++) 
                        AIs[i] = new MonsterAI(new Monster(transforms[i]));*/
                    for (var i = 0; i < transforms.Length; i++)
                        AIs[i] = new Director(new MonsterBuilder(transforms[i])).GetResult();
                    break;
                default:
                    Debug.Log("該類別" + tag + "無法產生物件");
                    return null;
            }
            return AIs;
        }
    }
}