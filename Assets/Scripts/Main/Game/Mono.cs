using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Main.Attribute;
using Main.Entity;
using Main.Entity.Controller;
using Main.Extension.Util;
using Main.Game.Input;
using Main.Util;
using UnityEngine;
using Physics2D = UnityEngine.Physics2D;

namespace Main.Game
{
    public class Mono : MonoBehaviour
    {
        public List<PlayerAI> playerAIs;
        public List<MonsterAI> monsterAIs;
        private readonly InputSystem inputSystem = new InputSystem();
        public static Mono Instance => FindObjectOfType<Mono>();

        private void Awake()
        {
            // 不支援轉職
            /*PlayerAI playerAI =
                new PlayerAI(
                    new Player(
                        GameObject.FindWithTag("Player").transform));*/
            playerAIs = Factory(Tag.Player).Cast<PlayerAI>().ToList();
            monsterAIs = Factory(Tag.Monster).Cast<MonsterAI>().ToList();
            inputSystem.Awake();
            IgnoreCollision();
            // Time.timeScale = .3f;
        }

        private void Update()
        {
            foreach (var AI in playerAIs) AI.Update();
            foreach (var AI in monsterAIs) AI.Update();
            inputSystem.Update();

            /*if (UnityEngine.Input.anyKeyDown)
            {
                var f= FindAI(GameObject.Find("Warrior").transform);
                Debug.Log(f.GetTeam());
            }*/
        }


        private enum Tag
        {
            Monster,
            Player,
            Boss //TODO:預留
        }

        private AbstractCreatureAI[] Factory([NotNull] Tag tag)
        {
            // A.程式碼tag, a.unity tag, 這兩者務必同名
            AbstractCreatureAI[] AIs = null;
            // 根據A去獲取a包含的場景內激活物件
            // var transforms = GameObject.FindGameObjectsWithTag(tag.ToString()).Cast<Transform>().ToArray();
            var transforms = GameObject.FindGameObjectsWithTag(tag.ToString()).Select(p => p.transform).ToArray();
            AIs = new AbstractCreatureAI[transforms.Length];
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

        private Dictionary<Transform, AbstractCreatureAI> dictionary;

        private Dictionary<Transform, AbstractCreatureAI> Dictionary =>
            dictionary ??= playerAIs.ToArray()
                .Concat<AbstractCreatureAI>(monsterAIs.ToArray())
                .ToDictionary(ai => ai.GetTransform(), creatureAI => creatureAI);

        public AbstractCreatureAI FindAI(Transform transform)
        {
            try
            {
                return Dictionary[transform];
            }
            catch (Exception)
            {
                return null;
            }
        }

        // todo 一開始抓取不到
        public SkillAttr FindSkillAttr(Transform transform)
        {
            var creature = FindAI(transform).GetCreature();
            try
            {
                return creature.GetBehavior().FindByName(creature.GetCreatureAttr().SkillName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void IgnoreCollision()
        {
            /*var pp = Dictionary.Values.ToArray();
            pp.Combination(pp.Length, 2)
                .Select(p => p).ToArray().ArrayToString('\n', false)
                .LogLine();
            // UnityTool.GetComponentsInChildren<Transform>();
            var rr = new[] {1, 2, 3, 4};
            foreach (var ints in rr.CombinationBinary(i => i != 2)) 
                ints.ArrayToString().LogLine();*/

            var foot = FindObjectsOfType<Transform>().Filter(collider1 => collider1.name == "GroundChecker");
            var c1 = foot.Filter(transform1 => transform1.CompareTag("Ground1"))
                .Select(t => t.root.GetComponent<Collider2D>());
            var c2 = foot.Filter(transform1 => transform1.CompareTag("Ground2"))
                .Select(t => t.root.GetComponent<Collider2D>());
            var c3 = foot.Filter(transform1 => transform1.CompareTag("Ground3"))
                .Select(t => t.root.GetComponent<Collider2D>());
            Debug.Log(c2.ToArray().ArrayToString());
            var grounds = FindObjectsOfType<Collider2D>().Filter(collider1 => collider1.CompareLayer("Ground"));
            var g1 = grounds.Filter(collider2D1 => collider2D1.CompareTag("Ground1"));
            var g2 = grounds.Filter(collider2D1 => collider2D1.CompareTag("Ground2"));
            var g3 = grounds.Filter(collider2D1 => collider2D1.CompareTag("Ground3"));
            foreach (var c in c1)
            {
                foreach (var g in g2) Physics2D.IgnoreCollision(c, g);
                foreach (var g in g3) Physics2D.IgnoreCollision(c, g);
            }

            foreach (var c in c2)
            {
                foreach (var g in g1) Physics2D.IgnoreCollision(c, g);
                foreach (var g in g3) Physics2D.IgnoreCollision(c, g);
            }

            foreach (var c in c3)
            {
                foreach (var g in g1) Physics2D.IgnoreCollision(c, g);
                foreach (var g in g2) Physics2D.IgnoreCollision(c, g);
            }
        }
    }
}