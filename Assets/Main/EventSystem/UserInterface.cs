using System;
using JetBrains.Annotations;
using Main.Entity.Creature;
using Main.EventSystem.Cause;
using Main.EventSystem.Event;
using Main.EventSystem.Event.CreatureEventSystem;
using Main.EventSystem.Event.CreatureEventSystem.Skill;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Attribute;
using Main.EventSystem.Event.CreatureEventSystem.StateEvent;
using Main.EventSystem.Event.UIEvent.CameraShaker;
using Main.EventSystem.Event.UIEvent.CD;
using Main.EventSystem.Event.UIEvent.Combo;
using Main.EventSystem.Event.UIEvent.QTE;
using Main.Util;
using UnityEngine;

namespace Main.EventSystem
{
    public static class UserInterface
    {
        /// <summary>
        /// 全部角色的行為
        /// 攻擊、移動
        /// 注意每個角色能夠執行的行為有限!
        /// </summary>
        /// <param name="creature"></param>
        /// <returns></returns>
        public static CreatureBehaviorInterface CreateCreatureBehaviorInterface(Creature creature) =>
            new CreatureBehaviorInterface(creature);

        public static CreatureInterface GetCreatureInterface(Creature creature) =>
            new CreatureInterface(creature);

        public static float GetLookAtAxisX(Creature creature) => creature.IsFacingRight ? 1 : -1;
        public static Vector2 GetLookAt(Creature creature) => new Vector2(GetLookAtAxisX(creature), 0);
        public static bool Grounded(Creature creature) => creature.CreatureAttr.Grounded;

        public static SkillAttr SetKnockBack(ISkill skill,
            float force = 80, Func<Vector2> dynDirection = null,
            bool @switch = true) =>
            skill.SkillAttr.SetKnockBack(force, dynDirection, @switch);

        public static SkillAttr SetVFX(ISkill skill,
            [CanBeNull] Transform obj, Func<Vector2> dynDirection = null, Vector2 offsetPos = default,
            bool @switch = false) =>
            skill.SkillAttr.SetVFX(obj, dynDirection, offsetPos, @switch);

        /// <summary>
        /// 技能攻擊到對方
        /// 造成傷害、擊退、特效
        /// </summary>
        /// <param name="theInjured">受擊方角色</param>
        /// <param name="attacker">攻擊方技能</param>
        public static void Hit(Creature theInjured, SkillAttr attacker) =>
            HitEvent.Invoke(theInjured, attacker);

        public static void Killed(Creature target) =>
            CreateCreatureBehaviorInterface(target).Killed();

        public static void Revival(Creature target) => 
            CreateCreatureBehaviorInterface(target).Revival();

        public static CameraShaker CreateCameraShakerEvent(float duration = 0.05f, float quake = 0.2f) =>
            new CameraShaker("Main Camera", duration, quake);

        public static ComboUIEvent CreateComboUI() => new ComboUIEvent("UI/PanelCombo");
        public static QteUIEvent CreateQteUI() => new QteUIEvent("UI/PanelQTE");

        public static CdEvent CreateCdUI(string hierarchyPath) =>
            new CdEvent(hierarchyPath, 0);

        public static CdEvent CreateCdUI(string hierarchyPath, [NotNull] AbstractEvent @event) =>
            (CdEvent) new CdEvent(hierarchyPath, @event.CdTime).AppendToCdTime(@event);

        public static CdCause CreateCdCause(float timer, Stopwatch.Mode mode) => new CdCause(timer, mode);
    }
}