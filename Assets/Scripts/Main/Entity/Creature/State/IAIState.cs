using Main.Extension.Util;
using Main.Util;
using UnityEngine;

namespace Main.Entity
{
    public abstract class IAIState
    {
        private bool inited;

        public bool Inited
        {
            get
            {
                if (!inited && !creatureAI.IsEmpty()) 
                    inited = Init(); // 只執行一次
                return inited;
            }
        }

        protected AbstractCreatureAI creatureAI;
        protected IAIStrategy aiStrategy;
        private Animator animator;
        private AbstractCreature target;
        private Vector2? attackPosition;

        // ======
        // Debug
        // ======
        protected IAIState()
        {
            Debug.Log("開啟 " + GetType().Name + " 此類別");
        }

        /// 自定義結構子。回傳是否執行成功
        /// <returns>是否執行成功</returns>
        public virtual bool Init() => true;

        /// 自定義析構。回傳是否執行成功
        /// <returns>是否執行成功</returns>
        protected virtual bool Dispose()
        {
            Debug.Log("關閉 " + GetType().Name + " 此類別");
            return true;
        }

        protected void ChangeAIState(IAIState newState)
        {
            Dispose();
            creatureAI.ChangeAIState(newState);
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

            // this.aiStrategy = aiStrategy ?? new MonsterStrategy(creatureAI);// 預設為monster
            this.aiStrategy = aiStrategy;
        }

        public void SetCreatureAI(AbstractCreatureAI abstractCreatureAI)
        {
            this.creatureAI = abstractCreatureAI;
            this.animator = abstractCreatureAI.GetTransform().GetOrLogComponent<Animator>();
        }

        public void SetAttackPosition(Vector2 attackPosition) => this.attackPosition = attackPosition;
        public AbstractCreature GetTarget() => target;
        public void SetTarget(AbstractCreature target) => this.target = target;

        public void RemoveTarget() => target = null;

        public abstract void Update();

        public override string ToString()
        {
            string info = this.GetType().Name;
            /*foreach (var fieldInfo in this.GetType().GetFields(instance|NonPublic))
            {
                info += "\n" + fieldInfo.SkillName + "\t" + fieldInfo.GetValue(this);
            }*/
            info += "\n" + creatureAI.GetIsNotNullString();
            info += "\n" + "目標" + target.GetIsNotNullString();
            info += "\n" + animator.GetIsNotNullString();
            return info;
        }
    }
}