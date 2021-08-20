using System;
using System.Collections.Generic;
using System.Linq;
using Main.Entity.Creature;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Attribute;
using UnityEngine;

namespace Main.EventSystem.Event.BattleSystem
{
    public static class CreatureList
    {
        private static DemoPlayer[] _demoPlayers;

        public static void Init()
        {
            _demoPlayers = new DemoPlayer[1];
            // 使用Builder.Factory
            /*
            var transforms = GameObject.FindGameObjectsWithTag(tag.ToString()).Select(p => p.transform).ToArray();
            creatures[i] = new Director(new PlayerBuilder(transforms[i])).GetResult();
        */
        }
        private static Dictionary<Transform, AbstractCreature> _dictionary;

        private static Dictionary<Transform, AbstractCreature> Dictionary
        {
            get
            {
                // 記得避免陣列中含有相同key
                if (_dictionary == null || !_dictionary.Any())
                    _dictionary = _demoPlayers.Cast<AbstractCreature>()
                        .ToDictionary(creature => creature.Transform, creature => creature);

                return _dictionary;
            }
        }

        public static SkillAttr FindSkillAttr(Transform obj)
        {
            var creature = FindCreature(obj);
            try
            {
                // return creature.Behavior.FindByName(creature.GetCreatureAttr().SkillName); todo
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        public static AbstractCreature FindCreature(Transform obj) =>
            Dictionary.ContainsKey(obj) ? Dictionary[obj] : null;
    }
}