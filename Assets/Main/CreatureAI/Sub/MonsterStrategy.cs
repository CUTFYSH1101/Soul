using System;
using Main.CreatureBehavior.Behavior.Sub;
using Main.Entity;
using Main.Entity.Creature;
using Main.EventLib.Condition;
using Main.EventLib.Sub.BattleSystem;
using Main.EventLib.Sub.CreatureEvent.Skill;
using UnityEngine;

namespace Main.CreatureAI.Sub
{
    public class MonsterStrategy : ICreatureStrategy, IComponent
    {
        /*
        Creature.GetCreatureAI().Init(new MonsterStrategy(monster), team: Enemy,
        chaseRange: new Vector2(5f, 1.5f), attackRange: new Vector2(0.85f, 1.5f));
            // 當敵人進入視野，進入Chase
            if (creatureAI.IsSeeingEnemy() && creatureAI.GetCreatureAttr().MovableDyn)
        creatureAI.ChangeAIState(new ChaseAIState());
        enemyInView = new Test3.AnyEnemyInView(transform, chaseRange, team);
        enemyInAttackRange = new Test3.AnyEnemyInView(transform, attackRange, team);
        public bool IsSeeingEnemy() => enemyInView.UpdateCreatureInView();

        public bool IsEnemyInAttackRange() => enemyInAttackRange.UpdateCreatureInView();
        */
        private (EnvironmentCheck chaseSensor, Func<bool> isSeeingEnemy, EnvironmentCheck attackSensor, Func<bool>
            isEnemyInAttackRange, Creature target) Sensors;

        private readonly CreatureInterface _creatureInterface;

        private readonly MonsterBehavior _behavior;
        
        // 注意，attackRange<chaseRange
        // 傳入的behavior要設置cd
        public MonsterStrategy(CreatureInterface creatureInterface, MonsterBehavior behaviorData, Team team,
            Vector2 chaseRange = default, Vector2 attackRange = default)
        {
            _creatureInterface = creatureInterface;
            _behavior = behaviorData;
            ChangeState(EnumCreatureStateTag.Aware); // init state

            // 註冊敵人探測器
            Sensors.chaseSensor =
                BattleInterface.CreatePhysicalSensorToSearchEnemy(_creatureInterface.GetCreature().Transform,
                    chaseRange, team);
            Sensors.isSeeingEnemy = () => Sensors.chaseSensor.UpdateCreatureInView();
            Sensors.attackSensor =
                BattleInterface.CreatePhysicalSensorToSearchEnemy(_creatureInterface.GetCreature().Transform,
                    attackRange, team);
            Sensors.isEnemyInAttackRange = () => Sensors.attackSensor.UpdateCreatureInView();
            Sensors.chaseSensor.OnSetTarget = newTarget => Sensors.target = newTarget; // 自動更新目標
        }

        public void IdleEnter()
        {
        }

        public void IdleUpdate()
        {
        }

        public void AwareEnter()
        {
        }

        public void AwareUpdate()
        {
            // 當敵人進入攻擊範圍，則切換至Attack
            if (Sensors.isEnemyInAttackRange() && _creatureInterface.AttackableDyn)
                ChangeState(EnumCreatureStateTag.Attack);

            // 當敵人進入視野，進入Chase
            else if (Sensors.isSeeingEnemy() && _creatureInterface.MovableDyn)
                ChangeState(EnumCreatureStateTag.Chase);
        }

        public void AttackEnter()
        {
        }
        private CdCondition _attackGap = new(5f);

        public void AttackUpdate()
        {
            if (!Sensors.isEnemyInAttackRange() && !_creatureInterface.GetAnim().IsTag("Attack"))
                ChangeState(EnumCreatureStateTag.Aware);
            // 每__秒打一次玩家
            if (_creatureInterface.AttackableDyn && _attackGap.AndCause())
            {
                _behavior.NormalAttack();
                _attackGap.Reset();//false
            }
        }

        public void ChaseEnter()
        {
        }

        public void ChaseUpdate()
        {
            // 當沒有看見敵人，則切換回Idle/Aware
            if (!Sensors.isSeeingEnemy())
            {
                // 關閉追擊行為 todo
                _behavior.StopMoveTo();
                ChangeState(EnumCreatureStateTag.Aware);
            }

            // 當敵人進入攻擊範圍，則切換至Attack
            if (Sensors.isEnemyInAttackRange())
            {
                // 關閉追擊行為 todo
                _behavior.StopMoveTo();
                ChangeState(EnumCreatureStateTag.Attack);
            }

            // 每1秒，就更新玩家位置，並再追上去
            // moveTo
            if (Sensors.target != null)
                _behavior.MoveTo(Sensors.target.Transform.position);
        }

        public EnumComponentTag Tag => EnumComponentTag.CreatureStrategy;
        public CreatureState CurrentState { get; private set; }

        public void Update()
        {
            CurrentState?.Update();
            // CurrentState?.State.ToString().LogLine();
        }

        public void ChangeState(EnumCreatureStateTag newState)
        {
            CurrentState = new CreatureState(newState, this);
            CurrentState.Enter();
        }
    }
}