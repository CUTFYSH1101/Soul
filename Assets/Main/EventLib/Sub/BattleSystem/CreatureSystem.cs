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

        public void Init()
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

        public EnumSkillTag FindInUsingSkillTag(Transform obj) =>
            FindCreatureByObj(obj) != null
                ? FindCreatureByObj(obj).CreatureAttr.CurrentSkill
                : EnumSkillTag.None;

        [CanBeNull]
        public SkillAttr FindInUsingSkillAttr(Transform obj) =>
            FindCreatureByObj(obj) != null
                ? FindCreatureByObj(obj).CurrentSkill
                : null;

        [CanBeNull]
        public static Creature FindCreatureByObj(Transform obj) =>
            Factory.FindCreatureByObj(obj);

        [CanBeNull]
        public static Creature[] FindCreaturesByTag(Factory.EnumCreatureTag tag) =>
            Factory.FindCreaturesByTag(tag);

        [CanBeNull]
        public IComponent FindComponent(Transform obj, EnumComponentTag tag) =>
            FindCreatureByObj(obj)?.FindByTag(tag);

        [CanBeNull]
        public T FindComponent<T>(Transform obj) where T : class, IComponent => 
            FindCreatureByObj(obj)?.FindComponent<T>();

        [CanBeNull]
        public IData FindData(Transform obj, EnumDataTag tag) =>
            FindCreatureByObj(obj)?.FindByTag(tag);
    }
}