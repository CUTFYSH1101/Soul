using Main.Util;

namespace Main.Entity
{
    public class ChaseAIState : IAIState
    {
        private bool initialized;

        /*public ChaseAIState()
        {
            Move(true);
        }*/
        // unity機制不觸發
        /*~ChaseAIState()
        {
            creatureAI.Move(false);
        }*/

        protected override bool Init() =>
            AbstractCreatureAI.GetCreature().Move(true);

        protected override bool Dispose()
        {
            finished = AbstractCreatureAI.GetCreature().Move(false);
            AbstractCreatureAI.GetCreature().GetRigidbody2D()
                .SetGuideX(0);
            return finished;
        }

        protected bool finished;

        public override void Update()
        {
            // 等待初始化，並切換成對應的動畫
            if (AbstractCreatureAI.IsEmpty()) return;

            // 只執行一次
            if (!initialized)
            {
                initialized = Init();
                return;
            }

            // 當沒有看見敵人，則切換回Idle
            if (!AbstractCreatureAI.IsSeeingEnemy())
            {
                ChangeAIState(new IdleAIState());
            }

            // 當敵人進入攻擊範圍，則切換至Attack
            if (AbstractCreatureAI.IsEnemyInAttackRange())
            {
                ChangeAIState(new AttackAIState());
            }

            
            // Debug.Log("追阿");
            if (GetTarget() != null && !finished)
                AbstractCreatureAI.GetCreature().GetRigidbody2D()
                    .MoveTo(GetTarget().GetPosition(),AbstractCreatureAI.GetCreature().GetCreatureAttr().MoveSpeed,AbstractCreatureAI.GetCreature().GetCreatureAttr().JumpForce);
        }
    }
}