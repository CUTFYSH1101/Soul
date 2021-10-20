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
    public class MiddleMonsterStrategy : ICreatureStrategy, IComponent
    {
        private (EnvironmentCheck chaseSensor, Func<bool> isSeeingEnemy, EnvironmentCheck attackSensor, Func<bool>
            isEnemyInAttackRange, Creature target) Sensors;

        private readonly CreatureInterface _creatureInterface;

        private readonly MiddleMonsterBehavior _behavior;

        private readonly MonsterStrategy _decoratedStrategy;
        // 注意，attackRange<chaseRange
        // 傳入的behavior要設置cd
        public MiddleMonsterStrategy(CreatureInterface creatureInterface, MiddleMonsterBehavior behaviorData, Team team,
            Vector2 chaseRange = default, Vector2 attackRange = default)
        {
            _creatureInterface = creatureInterface;
            _behavior = behaviorData;
            _decoratedStrategy = new MonsterStrategy(creatureInterface,
                new MonsterBehavior(creatureInterface.GetCreature()), team,
                chaseRange, attackRange);
            _decoratedStrategy.ChangeState(EnumCreatureStateTag.Aware);
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

        public void IdleEnter() => _decoratedStrategy.IdleEnter();

        public void IdleUpdate() => _decoratedStrategy.IdleUpdate();

        public void AwareEnter() => _decoratedStrategy.AwareEnter();

        public void AwareUpdate() => _decoratedStrategy.AwareUpdate();

        public void AttackEnter()
        {
            _decoratedStrategy.AttackEnter();
            Sensors.attackSensor.OnSetTarget = target => Sensors.target = target;
        }
        private CdCondition _attackGap = new(2f);
        public void AttackUpdate()
        {
            // _monsterStrategy.AttackUpdate();
            if (!Sensors.isEnemyInAttackRange() && !_creatureInterface.GetAnim().IsTag("Attack"))
                ChangeState(EnumCreatureStateTag.Aware);
            
            // 每__秒打一次玩家
            if (_creatureInterface.AttackableDyn && _attackGap.AndCause())
            {
                _behavior.QteAttack(Sensors.target);
                _attackGap.Reset();//false
                Debug.Log("QteAttack");
            }
            /*
            // 每兩秒打一次玩家
            if (_creatureInterface.AttackableDyn)
                _behavior.QteAttack(Sensors.target);
        */
        }

        public void ChaseEnter() => _decoratedStrategy.ChaseEnter();

        public void ChaseUpdate() => _decoratedStrategy.ChaseUpdate();

        public EnumComponentTag Tag => EnumComponentTag.CreatureStrategy;
        public CreatureState CurrentState { get; private set; }

        public void Update()
        {
            // _monsterStrategy.CurrentState?.Update();
            CurrentState?.Update();
            if (_decoratedStrategy.CurrentState.State != CurrentState.State)
                ChangeState(_decoratedStrategy.CurrentState.State);
        }

        public void ChangeState(EnumCreatureStateTag newState)
        {
            _decoratedStrategy.ChangeState(newState);
            CurrentState = new CreatureState(newState, this);
            CurrentState.Enter();
        }
    }
}