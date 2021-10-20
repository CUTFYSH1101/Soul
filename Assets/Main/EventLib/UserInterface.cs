using System;
using JetBrains.Annotations;
using Main.Entity.Creature;
using Main.EventLib.Common;
using Main.EventLib.Condition;
using Main.EventLib.Main.EventSystem.Main.Interface;
using Main.EventLib.Sub.CreatureEvent;
using Main.EventLib.Sub.CreatureEvent.Skill;
using Main.EventLib.Sub.CreatureEvent.Skill.Attribute;
using Main.EventLib.Sub.CreatureEvent.StateEvent;
using Main.EventLib.Sub.UIEvent.CameraShaker;
using Main.EventLib.Sub.UIEvent.CD;
using Main.EventLib.Sub.UIEvent.Combo;
using Main.EventLib.Sub.UIEvent.QTE;
using Main.Util;
using UnityEngine;

namespace Main.EventLib
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
            new(creature);

        public static CreatureInterface GetCreatureInterface(Creature creature) =>
            new(creature);

        public static float GetLookAtAxisX(Creature creature) => creature.IsFacingRight ? 1 : -1;
        public static Vector2 GetLookAt(Creature creature) => new(GetLookAtAxisX(creature), 0);
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
            HitEvent.Execute(theInjured, attacker);

        public static void AppendState(Creature theInjured, EnumDebuff debuff) =>
            HitEvent.AppendState(theInjured, debuff).Execute();

        public static void AppendState(Creature theInjured, EnumOtherState state) =>
            HitEvent.AppendState(theInjured, state);

        public static void RemoveState(Creature theInjured, EnumOtherState state) =>
            HitEvent.RemoveState(theInjured, state);

        public static void Killed(Creature target) =>
            CreateCreatureBehaviorInterface(target).Killed();

        public static void Revival(Creature target) =>
            CreateCreatureBehaviorInterface(target).Revival();

        public static CameraShaker CreateCameraShakerEvent(float duration = 0.05f, float quake = 0.2f) =>
            new("Main Camera", duration, quake);

        public static ComboUIEvent CreateComboUI() => new("UI/PanelCombo");
        public static QteUIEvent CreateQteUI() => new("UI/PanelQTE");

        /*
        public static CdEvent CreateCdUI(string hierarchyPath) =>
            new CdEvent(hierarchyPath, 0);
            */

        /*public static CdEvent CreateCdUI(string hierarchyPath, [NotNull] AbstractEvent @event) =>
            (CdEvent) new CdEvent(hierarchyPath, @event.CdTime).AppendToCdTime(@event);*/
        public static CdUIEvent CreateCdUI(string hierarchyPath, [NotNull] IEvent @event) =>
            (CdUIEvent)new CdUIEvent(hierarchyPath, @event.EventAttr.CdTime).AppendToCdTime(@event);

        public static CdCondition CreateCdCause(float timer, Stopwatch.Mode mode) => new(timer, mode);
    }
}