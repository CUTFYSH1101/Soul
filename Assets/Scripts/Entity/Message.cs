using Event;
using Main.Common;
using Main.Entity.Attr;
using Main.Util;
using UnityEngine;
using static System.Reflection.BindingFlags;

namespace Main.Entity.Controller
{
    public class Message : MonoBehaviour, IHitable
    {
        private Team team;
        private IAIState aiState;
        private ICreature creature;
        private ICreatureAI creatureAI;
        private ICreatureAttr creatureAttr;

        public void Init(ICreatureAI creatureAI)
        {
            this.creatureAI = creatureAI;
            creature = creatureAI.GetCreature();
            aiState = creatureAI.GetAIState();
            team = creatureAI.GetTeam(); // 只用來顯示
            creatureAttr = creature.GetCreatureAttr();
        }

        public bool IsEnemy(Team team) => GetCreatureAI().IsEnemy(team);

        public IAIState GetAIState()
        {
            if (aiState.IsEmpty()) aiState = creatureAI.GetAIState();
            return aiState;
        }

        public ICreature GetCreature()
        {
            if (creature.IsEmpty())
                creature = creatureAI.GetCreature();
            return creature;
        }

        public ICreatureAI GetCreatureAI() => creatureAI;

        public ICreatureAttr GetCreatureAttr()
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