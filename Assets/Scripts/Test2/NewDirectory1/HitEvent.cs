using JetBrains.Annotations;
using Main.Attribute;
using Main.Common;
using Main.Entity;
using Main.Entity.Skill_210528;
using UnityEngine;

namespace Main.Event
{
    public static class HitEvent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="creature">我方</param>
        /// <param name="knockbackSkill">我方</param>
        /// <param name="attr">對方</param>
        public static void Invoke(this AbstractCreature creature, KnockbackSkill knockbackSkill, SkillAttr attr)
        {
            if (creature == null || knockbackSkill == null || attr == null)
                return;


            knockbackSkill.Invoke(attr.Direction(), attr.Knockback + 20, attr.VFX, attr.OffsetPos);

            AppendState(creature, attr.DeBuffBuff);
        }

// todo 改成真正的append
        public static void AppendState(this AbstractCreature creature, DeBuff deBuff)
        {
            if (creature == null || deBuff == default)
                return;

            StateEvent stateEvent = null;
            switch (deBuff)
            {
                case DeBuff.Dizzy:
                    stateEvent = new StateEvent(creature, () =>
                        {
                            creature.SetMindState(MindState.Dizzy);
                            creature.GetAnimator().Knockback(true);
                        },
                        () =>
                        {
                            creature.SetMindState(MindState.Idle);
                            creature.GetAnimator().Knockback(false);
                        }, 1);
                    break;
                case DeBuff.Stiff:
                    stateEvent = new StateEvent(creature, () =>
                        {
                            creature.SetMindState(MindState.Stiff);
                            creature.GetAnimator().Knockback(true);
                        },
                        () =>
                        {
                            creature.SetMindState(MindState.Idle);
                            creature.GetAnimator().Knockback(false);
                        }, 0.5f);
                    break;
                default:
                    return;
            }

            stateEvent?.Invoke();
        }
    }
}