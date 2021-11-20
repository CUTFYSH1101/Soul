using System.Linq;
using JetBrains.Annotations;
using Main.Entity;
using Main.Entity.Creature;
using Main.EventLib.Sub.CreatureEvent.Skill.Attribute;
using Main.EventLib.Sub.CreatureEvent.Skill.Common;
using Main.Res.Resources;
using Main.Res.Script.Audio;
using UnityEngine;

namespace Main.EventLib.Sub.BattleSystem
{
    /// <summary>
    /// 掌管場景中的物件
    /// </summary>
    public class CreatureSystem : IComponent
    {
        public static CreatureSystem Instance => new();
        public static Creature[] Creatures => Factory.CreatureDictionary.Values.ToArray();

        public static void Init()
        {
            // 使用Builder.Factory
            Factory.CreateCreatures(Factory.EnumCreatureTag.Player, Loader.Find<DictAudioPlayer>(Loader.Tag.Audio));
            Factory.CreateCreatures(Factory.EnumCreatureTag.Monster);
            Factory.CreateCreatures(Factory.EnumCreatureTag.MiddleMonster);
        }

        public EnumComponentTag Tag => EnumComponentTag.CreatureSystem;

        public void Update()
        {
            foreach (var creature in Factory.CreatureDictionary.Values) creature.Update();
        }
        public static void Destroy() => Factory.Destroy();
        public static EnumSkillTag FindInUsingSkillTag(Transform obj) =>
            FindCreature(obj) != null
                ? FindCreature(obj).CreatureAttr.CurrentSkill
                : EnumSkillTag.None;

        [CanBeNull]
        public static SkillAttr FindInUsingSkillAttr(Transform obj) =>
            FindCreature(obj) != null
                ? FindCreature(obj).CurrentSkill
                : null;

        [CanBeNull]
        public static Creature FindCreature(Transform obj) =>
            Factory.FindCreatureByObj(obj);

        [CanBeNull]
        public static Creature[] FindCreaturesByTag(Factory.EnumCreatureTag tag) =>
            Factory.FindCreaturesByTag(tag);

        [CanBeNull]
        public static IComponent FindComponent(Transform obj, EnumComponentTag tag) =>
            FindCreature(obj)?.FindByTag(tag);

        [CanBeNull]
        public static T FindComponent<T>(Transform obj) where T : class, IComponent => 
            FindCreature(obj)?.FindComponent<T>();

        [CanBeNull]
        public static IData FindData(Transform obj, EnumDataTag tag) =>
            FindCreature(obj)?.FindByTag(tag);
    }
}