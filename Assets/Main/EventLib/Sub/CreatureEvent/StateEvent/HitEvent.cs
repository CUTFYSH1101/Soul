using System;
using Main.Entity.Creature;
using Main.EventLib.Common;
using Main.EventLib.Sub.CreatureEvent.Skill.Attribute;

namespace Main.EventLib.Sub.CreatureEvent.StateEvent
{
    public static class HitEvent
    {
        public static void Execute(Creature theInjured,
            SkillAttr attackerAttr)
        {
            if (theInjured == null || attackerAttr == null) return;
            // Debug.Log("HitEvent.Invoke");
            Effect(new Knockback(theInjured), attackerAttr);

            if (attackerAttr.Debuff == default) return;
            // Debug.Log("attackerAttr.DeBuff != default");
            theInjured.AppendState(attackerAttr.Debuff)
                ?.Execute();
        }
        
        public static void Execute(Creature theInjured, Knockback injuredKnockback,
            SkillAttr attackerSkillAttr)
        {
            if (theInjured == null || injuredKnockback == null || attackerSkillAttr == null) return;

            Effect(injuredKnockback, attackerSkillAttr);

            if (attackerSkillAttr.Debuff == default) return;

            theInjured.AppendState(attackerSkillAttr.Debuff)
                ?.Execute();
        }

        /// 裝填攻擊方技能的參數，之後再根據參數產生vfx和擊退效果，不包含受擊方本身的vfx，不包含受擊方本身的掉血動畫。
        private static void Effect(Knockback theInjured, SkillAttr attackerAttr)
        {
            // 擊退
            if (!attackerAttr.VFX.@switch && attackerAttr.Knockback.Switch)
                theInjured.Execute(attackerAttr.Knockback.FinForce, attackerAttr.Knockback.Force);
            // 擊退，並在受擊方上生成攻擊方技能特效
            if (attackerAttr.VFX.@switch && attackerAttr.Knockback.Switch)
                theInjured.Execute(attackerAttr.Knockback.DynDirection(), attackerAttr.Knockback.Force,
                    attackerAttr.VFX.obj,
                    attackerAttr.VFX.offsetPos);
        }

        /// 更改受擊方的精神狀態，使無法控制。
        public static StateEffect AppendState(this Creature target, EnumDebuff debuff)
        {
            var _ = new StateEffect(target, 
                () => target.CreatureAttr.AppendDeBuff(debuff), 
                () => target.CreatureAttr.RemoveDeBuff(debuff), 0);// knockback為單獨事件

            _.Duration = debuff switch
            {
                EnumDebuff.Dizzy => 1,
                EnumDebuff.Stiff => 0.5f,
                _ => _.Duration
            };
            return _;
        }

        public static void AppendState(this Creature target, EnumOtherState state)
        {
            if (state == EnumOtherState.UnderQte) target.CreatureAttr.UnderQte = true;
            else throw new Exception();
        }

        public static void RemoveState(this Creature target, EnumOtherState state)
        {
            if (state == EnumOtherState.UnderQte) target.CreatureAttr.UnderQte = false;
            else throw new Exception();    
        }
    }
}