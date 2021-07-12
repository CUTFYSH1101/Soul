namespace Main.Entity
{
    public class ChaseAIState : IAIState
    {
        private bool finished;
        /*public ChaseAIState()
        {
            Move(true);
        }*/
        // unity機制不觸發
        /*~ChaseAIState()
        {
            creatureAI.Move(false);
        }*/

        /*public override bool Init()
        {
            var inited = creatureAI.GetCreature().Move(true);
            if (inited) creatureAI.GetCreature().SetMindState(MindState.Move);
            return inited;
        }*/

        protected override bool Dispose()
        {
            creatureAI.GetCreature().GetBehavior()
                .MoveTo(false, default);
            finished = creatureAI.GetCreature().GetRigidbody2D().GetGuideX() == 0;
            // 會引起奇怪錯誤
            /*finished = creatureAI.GetCreature().Move(false);
            if (finished)
            {
                creatureAI.GetCreature().GetRigidbody2D().SetGuideX(0);
                creatureAI.GetCreature().SetMindState(MindState.Idle);
            }*/
            return finished;
        }

        public override void Update()
        {
            // 當沒有看見敵人，則切換回Idle
            if (!creatureAI.IsSeeingEnemy())
            {
                ChangeAIState(new IdleAIState());
            }

            // 當敵人進入攻擊範圍，則切換至Attack
            if (creatureAI.IsEnemyInAttackRange())
            {
                ChangeAIState(new AttackAIState());
            }
            
            if (GetTarget() != null && !finished && creatureAI.GetCreatureAttr().MovableDyn)
                creatureAI.GetCreature().GetBehavior()
                    .MoveTo(true, GetTarget().GetPosition());
        }
    }
}