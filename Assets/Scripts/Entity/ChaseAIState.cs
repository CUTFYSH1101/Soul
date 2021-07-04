using System;
using System.Linq;
using System.Threading.Tasks;
using Main.Util;
using UnityEngine;

namespace Main.Entity.Controller
{
    public class ChaseAIState : IAIState
    {
        private bool initialized;

        /*public ChaseAIState()
        {
            Move(true);
        }*/
        private void Init()
        {
            creatureAI.Move(true);
        }
        ~ChaseAIState()
        {
            creatureAI.Move(false);
        }

        private void ChangeAIState(IAIState newState)
        {
            creatureAI.Move(false);
            creatureAI.ChangeAIState(newState);
        }
        public override void Update()
        {
            // 等待初始化，並切換成對應的動畫
            if (!initialized)
            {
                if (creatureAI.IsEmpty()) return;
                Init();
                initialized = true;
            }

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
        }
    }
}