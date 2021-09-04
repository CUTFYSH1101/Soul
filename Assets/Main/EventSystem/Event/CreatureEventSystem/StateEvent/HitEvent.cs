using Main.Entity.Creature;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Attribute;

namespace Main.EventSystem.Event.CreatureEventSystem.StateEvent
{
    public static class HitEvent
    {
        public static void Invoke(Creature theInjured,
            SkillAttr attackerAttr)
        {
            if (theInjured == null || attackerAttr == null) return;

            Effect(new Knockback(theInjured), attackerAttr);

            if (attackerAttr.DeBuff == default) return;

            theInjured.AppendState(attackerAttr.DeBuff)
                ?.Invoke();
        }
        
        public static void Invoke(Creature theInjured, Knockback injuredKnockback,
            SkillAttr attackerSkillAttr)
        {
            if (theInjured == null || injuredKnockback == null || attackerSkillAttr == null) return;

            Effect(injuredKnockback, attackerSkillAttr);

            if (attackerSkillAttr.DeBuff == default) return;

            theInjured.AppendState(attackerSkillAttr.DeBuff)
                ?.Invoke();
        }

        /// 裝填攻擊方技能的參數，之後再根據參數產生vfx和擊退效果，不包含受擊方本身的vfx，不包含受擊方本身的掉血動畫。
        private static void Effect(Knockback theInjured, SkillAttr attackerAttr)
        {
            // 擊退
            if (!attackerAttr.VFX.@switch && attackerAttr.Knockback.Switch)
                theInjured.Invoke(attackerAttr.Knockback.FinForce, attackerAttr.Knockback.Force);
            // 擊退，並在受擊方上生成攻擊方技能特效
            if (attackerAttr.VFX.@switch && attackerAttr.Knockback.Switch)
                theInjured.Invoke(attackerAttr.Knockback.DynDirection(), attackerAttr.Knockback.Force,
                    attackerAttr.VFX.obj,
                    attackerAttr.VFX.offsetPos);
        }

        /// 更改受擊方的精神狀態，使無法控制。
        private static DeBuff AppendState(this Creature target, EventSystem.Common.DeBuff deBuff)
        {
            var _ = new DeBuff(target, () =>
            {
                target.CreatureAttr.AppendDeBuff(deBuff);
                // creature.AnimManager.Knockback(true); 透過creature update更改
            }, () =>
            {
                target.CreatureAttr.RemoveDeBuff(deBuff);
                // creature.AnimManager.Knockback(false);
            }, 0);

            _.Duration = deBuff switch
            {
                EventSystem.Common.DeBuff.Dizzy => 1,
                EventSystem.Common.DeBuff.Stiff => 0.5f,
                _ => _.Duration
            };
            return _;
        }
    }
}