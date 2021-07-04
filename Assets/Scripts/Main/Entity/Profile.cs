using Main.Attribute;
using Main.Common;
using Main.Event;
using Main.Util;
using UnityEngine;
using static System.Reflection.BindingFlags;

namespace Main.Entity
{
    public class Profile : MonoBehaviour, IHitable
    {
        private Team team;
        private IAIState aiState;
        private AbstractCreature abstractCreature;
        private AbstractCreatureAI abstractCreatureAI;
        private ICreatureAttr creatureAttr;

        public void Init(AbstractCreatureAI abstractCreatureAI)
        {
            this.abstractCreatureAI = abstractCreatureAI;
            abstractCreature = abstractCreatureAI.GetCreature();
            aiState = abstractCreatureAI.GetAIState();
            team = abstractCreatureAI.GetTeam(); // 只用來顯示
            creatureAttr = abstractCreature.GetCreatureAttr();
        }

        public bool IsEnemy(Team team) => GetCreatureAI().IsEnemy(team);

        public IAIState GetAIState()
        {
            if (aiState.IsEmpty()) aiState = abstractCreatureAI.GetAIState();
            return aiState;
        }

        public AbstractCreature GetCreature()
        {
            if (abstractCreature.IsEmpty())
                abstractCreature = abstractCreatureAI.GetCreature();
            return abstractCreature;
        }

        public AbstractCreatureAI GetCreatureAI() => abstractCreatureAI;

        public ICreatureAttr GetCreatureAttr()
        {
            if (creatureAttr.IsEmpty())
                creatureAttr = GetCreature().GetCreatureAttr();
            return creatureAttr;
        }
        /// 呼叫creature受傷
        public void Hit(Vector2 direction, float force,
            Transform vfx = null, Vector2 position = default)
            => abstractCreature.Hit(direction, force, vfx, position);

        public bool IsKilled() => abstractCreature.IsKilled();
        public void Killed() => abstractCreature.Killed();
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