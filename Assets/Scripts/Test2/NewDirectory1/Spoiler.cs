using System;
using Main.Attribute;
using UnityEngine;
using Main.Common;
using Main.Entity;
using Main.Game;
using Main.Util;
using static System.Reflection.BindingFlags;

namespace Main.Event
{
    public class Spoiler : MonoBehaviour
    {
        private AbstractCreatureAI creatureAI;
        private Mono mono;
        private Transform other;
        public bool IsKilled() => creatureAI.GetCreature().IsKilled();

        public bool IsEnemy(Team team) =>
            creatureAI.IsEnemy(team);

        private SkillAttr CurrentSkillAttr => mono.FindSkillAttr(transform);

        private void Awake()
        {
            mono = Mono.Instance;
            creatureAI = mono.FindAI(this.transform);
        }

        // 受到傷害
        private void OnTriggerEnter2D(Collider2D other)
        {
            // new UnityCoroutine().Create(this, () => DelayAction(other), .15f);
            DelayAction(other);
        }

        private void DelayAction(Collider2D other)
        {
            // 確認對方為敵人才可攻擊到
            var target = (AbstractCreatureAI) mono
                .FindAI(GetContainer(other))
                .Filter(ai => ai.IsEnemy(creatureAI.GetTeam()));
            this.other = other.transform;
            // 確認是我在攻擊
            if (target != null && creatureAI.GetCreature().GetCreatureAttr().MindState == MindState.Attack)
            {
                if (CurrentSkillAttr != null && ForceRight(creatureAI.GetCreature(), target.GetCreature()))// todo 先屏蔽後方的攻擊
                    target.GetCreature().GetBehavior().Hit(CurrentSkillAttr);

                /*// 假設A攻擊B，(A使用普通攻擊，B什麼都不做)
                (this.name + " " +                                      // A
                 other.name + " " +                                     // B
                 target.GetCreature().GetCreatureAttr().Name + " " +    // B
                 target.GetCreature().GetCreatureAttr().SkillName)      // None
                    .LogLine();*/
            }
        }

        private Transform GetContainer(Component leaf) =>
            leaf.GetComponentInParent<Transform>(transform => transform.CompareLayer("Creature"));

        private bool ForceRight(AbstractCreature creature, AbstractCreature target)
        {
            var relativePosX = System.Math.Sign(target.GetPosition().x - creature.GetPosition().x);
            if (relativePosX > 0 && !creature.IsFacingRight)
                return false;
            if (relativePosX < 0 && creature.IsFacingRight)
                return false;
            return true;
        }

        public override string ToString()
        {
            string info = base.ToString();
            foreach (var fieldInfo in this.GetType().GetFields(NonPublic | Instance))
            {
                info += "\n" +
                        fieldInfo.Name + "\t" +
                        (fieldInfo != null) + "\t" +
                        fieldInfo.GetValue(this);
            }

            return info;
        }
    }
}