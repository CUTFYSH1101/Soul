using Main.Attribute;
using Main.Common;
using Main.Event;
using Main.Util;
using UnityEngine;
using static System.Reflection.BindingFlags;

namespace Main.Entity
{
    public class Profile : MonoBehaviour, IHittable
    {
        private Team team;
        private IAIState aiState;
        private AbstractCreature creature;
        private AbstractCreatureAI creatureAI;
        private CreatureAttr creatureAttr;

        public void Init(AbstractCreatureAI abstractCreatureAI)
        {
            this.creatureAI = abstractCreatureAI;
            creature = abstractCreatureAI.GetCreature();
            aiState = abstractCreatureAI.GetAIState();
            team = abstractCreatureAI.GetTeam(); // 只用來顯示
            creatureAttr = creature.GetCreatureAttr();
        }

        public bool IsEnemy(Team team) => GetCreatureAI().IsEnemy(team);

        /*public IAIState GetAIState()
        {
            if (aiState.IsEmpty()) aiState = creatureAI.GetAIState();
            return aiState;
        }*/

        public AbstractCreature GetCreature()
        {
            if (creature.IsEmpty())
                creature = creatureAI.GetCreature();
            return creature;
        }

        public AbstractCreatureAI GetCreatureAI() => creatureAI;

        public CreatureAttr GetCreatureAttr()
        {
            if (creatureAttr.IsEmpty())
                creatureAttr = GetCreature().GetCreatureAttr();
            return creatureAttr;
        }
        /// 呼叫creature受傷
        public void Hit(Vector2 direction, float force,
            Transform vfx = null, Vector2 position = default)
            => creature.Hit(direction, force, vfx, position);

        public bool IsKilled() => creature.IsKilled();
        public void Killed() => creature.Killed();
        public void Destroy() => gameObject.SetActive(false);

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