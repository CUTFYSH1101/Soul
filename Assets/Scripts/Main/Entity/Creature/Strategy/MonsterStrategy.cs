namespace Main.Entity
{
    public class MonsterStrategy : IAIStrategy
    {
        private readonly AbstractCreatureAI creatureAI;

        public MonsterStrategy(AbstractCreatureAI creatureAI)
        {
            this.creatureAI = creatureAI;
        }

        public override void IdleUpdate()
        {
            // 當敵人進入視野，進入Chase
            if (creatureAI.IsSeeingEnemy())
                creatureAI.ChangeAIState(new ChaseAIState());

            // 當敵人進入攻擊範圍，則切換至Attack
            if (creatureAI.IsEnemyInAttackRange())
                creatureAI.ChangeAIState(new AttackAIState());
        }

        public override void AttackUpdate()
        {
            // 當初始化後循環執行
            if (!creatureAI.IsEnemyInAttackRange())
                creatureAI.ChangeAIState(new IdleAIState());

            if (creatureAI.GetCreatureAttr().AttackableDyn)
                ((MonsterBehavior) creatureAI.GetCreature().GetBehavior())
                    ?.NormalAttack();
            // creatureAI.GetCreature().NormalAttack(Symbol.Direct);
        }
    }
}