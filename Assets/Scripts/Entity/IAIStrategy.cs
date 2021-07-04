using Main.Entity.Controller;
using Main.Util;
using UnityEngine;

namespace Main.Entity
{
    // 不支援自動導航/自動戰鬥，一旦初始結束，則角色行為終身固定
    public abstract class IAIStrategy
    {
        public abstract void IdleUpdate();
        public abstract void AttackUpdate();
    }

    public class PlayerStrategy : IAIStrategy
    {
        private readonly ICreatureAI creatureAI;
        private readonly PlayerController controller;

        public PlayerStrategy(ICreatureAI creatureAI)
        {
            this.creatureAI = creatureAI;
            controller = new PlayerController(creatureAI.GetCreature());
            (creatureAI as PlayerAI)?.SetController(controller);
        }

        public override void IdleUpdate()
        {
            controller.Update();
        }

        public override void AttackUpdate()
        {
        }
    }

    public class MonsterStrategy : IAIStrategy
    {
        private readonly ICreatureAI creatureAI;

        public MonsterStrategy(ICreatureAI creatureAI)
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
            // 等待初始化，並切換成對應的動畫
            if (!creatureAI.IsEmpty()) creatureAI.Attack();
            // 當敵人離開攻擊範圍，則切換回Idle
            if (!creatureAI.IsEnemyInAttackRange())
                creatureAI.ChangeAIState(new IdleAIState());
        }
    }
}