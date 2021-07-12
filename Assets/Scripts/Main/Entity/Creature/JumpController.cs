using System;
using Main.Game.Collision;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Key = Main.Attribute.DictionaryAudioPlayer.Key;

namespace Main.Entity
{
    public class JumpController
    {
        private readonly AbstractCreature creature;
        private readonly BaseBehavior behavior;
        private readonly SkillTemplate skillTemplate;
        private Vector2 wallPos;
        private float bodyWidth;

        public JumpController(AbstractCreature creature, BaseBehavior behavior)
        {
            this.creature = creature;
            this.behavior = behavior;
            skillTemplate = new SkillTemplate(creature);
        }

        // 注意使用的是CapsuleCollider2D
        private float BodyWidth
        {
            get
            {
                if (bodyWidth != 0) return bodyWidth;
                bodyWidth = creature.GetTransform().GetComponent<CapsuleCollider2D>().size.x * 1.2f;
                return bodyWidth;
            }
        }

        private bool OnCollision()
        {
            wallPos = creature.GetTransform().GetLeanOnWallPos(BodyWidth);
            return wallPos != default;
        }

        public void Jump()
        {
            if (OnCollision() && !creature.GetCreatureAttr().Grounded)
            {
                var dir = Math.Sign(creature.GetPosition().x - ((Vector2) wallPos).x);
                behavior.WallJump(dir);
                skillTemplate.Play(Key.WallJump);
            }
            else
            {
                behavior.Jump();
            }
        }
    }
}