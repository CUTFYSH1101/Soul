using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Main.Util;
using Main.Entity;
using Main.Entity.Creature;
using Main.EventLib.Sub.CreatureEvent.Skill.Attribute;
using Main.EventLib.Sub.CreatureEvent.Skill.Common;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Main.EventLib.Sub.BattleSystem
{
    public static class BattleInterface
    {
        // ======
        // 碰撞偵測
        // 主要依賴類別：UnityCollision
        // ======
        public static EnvironmentCheck CreatePhysicalSensorToSearchEnemy(
            Transform selfEyeObj, Vector2 sensingRange, Team selfTeam) =>
            new AnyEnemyInView(selfEyeObj, sensingRange, selfTeam);

        public static EnvironmentCheck CreatePhysicalSensorToSearchAnyone(Transform selfEyeObj, Vector2 sensingRange) =>
            new AnyInView(selfEyeObj, sensingRange);

        // ======
        // 角色系統
        // 主要依賴類別：Factory, Creature
        // ======
        public static CreatureSystem CreateCreatureSystemComponent() => CreatureSystem.Instance;

        public static CreatureSystem CreateAndAppendCreatureSystemComponent(Creature self) =>
            (CreatureSystem)self.Append(CreatureSystem.Instance);

        public static IComponent[] FindComponents(EnumComponentTag tag)
        {
            var creatureSystem = CreatureSystem.Instance;
            var queue = new Queue<IComponent>();
            IComponent component;
            foreach (var obj in Object.FindObjectsOfType<Transform>().Get(go => go.transform))
                if ((component = CreatureSystem.FindComponent(obj, tag)) != null)
                    queue.Enqueue(component);
            return queue.Any() ? queue.ToArray() : null;
        }

        public static Dictionary<Transform, IComponent> GetComponentDictionary(EnumComponentTag tag)
        {
            var creatureSystem = CreatureSystem.Instance;
            var dict = new Dictionary<Transform, IComponent>();
            IComponent component;
            foreach (var obj in Object.FindObjectsOfType<Transform>().Get(go => go.transform))
                if ((component = CreatureSystem.FindComponent(obj, tag)) != null)
                    dict.Add(obj, component);
            return dict.Any() ? dict : null;
        }

        public static Dictionary<Transform, T> GetComponentDictionary<T>(EnumComponentTag tag) where T : IComponent
        {
            var creatureSystem = CreatureSystem.Instance;
            var dict = new Dictionary<Transform, T>();
            IComponent component;
            foreach (var obj in Object.FindObjectsOfType<Transform>().Get(go => go.transform))
                if ((component = CreatureSystem.FindComponent(obj, tag)) != null)
                    dict.Add(obj, (T)component);
            return dict.Any() ? dict : null;
        }

        // ======
        // 傷害與受擊系統
        // 受擊碰撞判定，並添加特效
        // 主要依賴類別：ComboUI, HitEvent, UnityCollision
        // ======
        public static Spoiler CreateAndAppendSpoilerComponent(Creature self, Team selfTeam) =>
            (Spoiler)self.Append(Spoiler.Instance(self, selfTeam));

        public static Spoiler CreateAndAppendSpoilerComponent(Creature self, Team selfTeam, Action onHit) =>
            (Spoiler)self.Append(Spoiler.Instance(self, selfTeam).InitEvent(onHit));
    }
}