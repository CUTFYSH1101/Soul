using Main.Util;
using UnityEngine;
using static System.Reflection.BindingFlags;

namespace Main.Entity.Controller
{
    public abstract class IAIState
    {
        protected IAIStrategy aiStrategy;
        protected ICreatureAI creatureAI;
        private Animator animator;
        private ICreature target;
        private Vector2? attackPosition;
        // ======
        // Debug
        // ======
        ~IAIState()
        {
            Debug.Log("關閉 " + GetType().Name + " 此類別");
        }

        public IAIState()
        {
            Debug.Log("開啟 " + GetType().Name + " 此類別");
        }
        public Animator GetAnimator() => animator;
        public void SetAIStrategy(IAIStrategy aiStrategy)
        {
            if (aiStrategy.IsEmpty())
            {
                Debug.Log("錯誤不含有aiStrategy");
            }
            if (creatureAI.IsEmpty())
            {
                Debug.Log("錯誤不含有creatureAI");
                return;
            }
            this.aiStrategy = aiStrategy ?? new MonsterStrategy(creatureAI);
        }
        public void SetCreatureAI(ICreatureAI creatureAI)
        {
            this.creatureAI = creatureAI;
            this.animator = creatureAI.GetTransform().GetOrLogComponent<Animator>();
        }

        public void SetAttackPosition(Vector2 attackPosition) => this.attackPosition = attackPosition;
        public ICreature GetTarget() => target;
        public void SetTarget(ICreature target) => this.target = target;

        public void RemoveTarget() => target = null;
        
        public abstract void Update();

        public override string ToString()
        {
            string info = this.GetType().Name;
            /*foreach (var fieldInfo in this.GetType().GetFields(Instance|NonPublic))
            {
                info += "\n" + fieldInfo.Name + "\t" + fieldInfo.GetValue(this);
            }*/
            info += "\n" + creatureAI.GetIsNotNullToString();
            info += "\n" + "目標" + target.GetIsNotNullToString();
            info += "\n" + animator.GetIsNotNullToString();
            return info;
        }
    }
}