using Main.Util;

namespace Main.Entity
{
    public class MonsterStrategy : IAIStrategy
    {
        private readonly AbstractCreatureAI abstractCreatureAI;

        public MonsterStrategy(AbstractCreatureAI abstractCreatureAI)
        {
            this.abstractCreatureAI = abstractCreatureAI;
        }

        public override void IdleUpdate()
        {
            // 當敵人進入視野，進入Chase
            if (abstractCreatureAI.IsSeeingEnemy()) 
                abstractCreatureAI.ChangeAIState(new ChaseAIState());

            // 當敵人進入攻擊範圍，則切換至Attack
            if (abstractCreatureAI.IsEnemyInAttackRange())
                abstractCreatureAI.ChangeAIState(new AttackAIState());
        }

        public override void AttackUpdate()
        {
            // 等待初始化，並切換成對應的動畫
            if (abstractCreatureAI.IsEmpty()) return;
            
            // 當初始化後循環執行
            abstractCreatureAI.Attack();
            if (!abstractCreatureAI.IsEnemyInAttackRange())
                abstractCreatureAI.ChangeAIState(new IdleAIState());
        }
    }
}