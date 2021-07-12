using System;
using System.Collections.Generic;
using System.Linq;
using Main.Attribute;
using Main.Common;
using Main.Util;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Rb2D = Main.Entity.Controller.Rigidbody2D;
using Skills = Main.Entity.Skill_210528;

namespace Main.Entity
{
    public abstract class AbstractCreatureBehavior
    {
        protected readonly AbstractCreature Creature;
        protected readonly CreatureAttr CreatureAttr;
        protected readonly Rb2D Rigidbody2D;
        protected readonly CreatureAnimManager AnimManager;
        protected float GetDirX => Creature.IsFacingRight ? 1 : -1;
        private readonly List<SkillAttr> skillAttrList = new List<SkillAttr>();
        private Dictionary<SkillName, SkillAttr> dictionary;

        private Dictionary<SkillName, SkillAttr> Dictionary
        {
            get
            {
                // 注意name不能為同值
                if (dictionary.IsEmpty() || dictionary.Count != skillAttrList.Count)// 當新增
                    dictionary = skillAttrList.ToDictionary(e => e.SkillName, skillAttr => skillAttr);
                return dictionary;
            }
        }

        protected void AppendAttr(SkillAttr newSkillAttr) =>
            skillAttrList.Add(newSkillAttr);

        public SkillAttr FindByName(SkillName skillName)
        {
            try
            {
                return Dictionary[skillName];
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected AbstractCreatureBehavior(AbstractCreature creature)
        {
            this.Creature = creature;
            this.CreatureAttr = creature.GetCreatureAttr();
            this.Rigidbody2D = creature.GetRigidbody2D();
            AnimManager = creature.GetAnimator();
        }

        public abstract void Jump();

        // public abstract void Move(float dir);
        public abstract void MoveTo(bool @switch, Vector2 targetPos);

        public abstract void Hit(Vector2 direction, float force,
            Transform vfx = null, Vector2 offsetPos = default);

        public abstract void Hit(SkillAttr skillAttr);

        public void Killed()
        {
            AnimManager.Killed(); // IsTag("Die") == true
            CreatureAttr.MindState = MindState.Dead;
            CreatureAttr.Alive = false;
        }

        public void Revival()
        {
            AnimManager.Revival(); // IsTag("Die") == false
            CreatureAttr.MindState = MindState.Idle;
            CreatureAttr.Alive = true;
        }

        public override string ToString()
        {
            foreach (var pair in Dictionary)
            {
                pair.Value.LogLine();
            }

            return $"--【{GetType().Name}】--\n" +
                   $"{Dictionary?.Select(pp => $"名稱: {pp.Key}\n內容:\n{pp.Value}").ToArray().ArrayToString('\n', false)}";
        }
    }
}