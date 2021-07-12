using System;
using JetBrains.Annotations;
using Main.Attribute;
using Main.Common;
using Main.Extension.Util;
using Main.Game.Collision;
using Main.Util;
using Main.Util.Timers;
using UnityEngine;
using UnityEngine.Serialization;
using static Main.Common.Team;

namespace Main.Entity
{
    [Serializable]
    public class AbstractCreatureAI
    {
        private bool aiUsed = true;
        private Profile profile;
        private Transform transform;
        private IAIState aiState;
        [FormerlySerializedAs("_creature")] [SerializeField] private AbstractCreature abstractCreature;
        private IAIStrategy aiStrategy;
        private CDTimer attackCdTimer = new CDTimer(.2f, Stopwatch.Mode.LocalGame);

        [FormerlySerializedAs("_team")] [SerializeField] private Team team;
        public Transform GetTransform() => abstractCreature.GetTransform();
        public Profile GetProfile() => profile;
        public AbstractCreature GetCreature() => abstractCreature;
        public CreatureAttr GetCreatureAttr() => abstractCreature.GetCreatureAttr();
        public Team GetTeam() => team;
        public Team SetTeam(Team team) => this.team = team;

        public bool IsEnemy(Team team)
        {
            switch (team)
            {
                case Evil:
                    return true;
                case Peace:
                    return false;
                default:
                    return this.team != team;
            }
        }

        public void SetSwitch(bool value) => aiUsed = value;
        public bool GetSwitch() => aiUsed;
        public AbstractCreatureAI(AbstractCreature abstractCreature)
        {
            this.abstractCreature = abstractCreature;
            this.abstractCreature.SetCreatureAI(this);
            transform = abstractCreature.GetTransform();
        }

        protected internal void Init([CanBeNull] IAIStrategy aiStrategy,
            Team team = Peace,
            Vector2 chaseRange = default, Vector2 attackRange = default)
        {
            this.aiStrategy = aiStrategy;
            // InitAIState(new IdleAIState());
            this.team = team;
            profile = transform.GetOrAddComponent<Profile>(); // 因此遊戲物件上最少會掛載一個腳本

            enemyInView =
                new AnyEnemyInView(transform, () => abstractCreature.IsFacingRight, chaseRange, team);
            enemyInAttackRange =
                new AnyEnemyInView(transform, () => abstractCreature.IsFacingRight, attackRange, team);
        }

        public void ChangeAIState(IAIState newAIState)
        {
            aiState = null;
            aiState = newAIState;
            aiState.SetCreatureAI(this);
            aiState.SetAIStrategy(aiStrategy);
            enemyInView.SetTarget = newAIState.SetTarget;
            // enemyInAttackRange.SetTarget = newAIState.SetTarget;// 不要讓他一直設定為null
        }

        public IAIState GetAIState() => aiState;
        private Vector2 ChaseRange => enemyInView.Size;
        private Vector2 AttackRange => enemyInAttackRange.Size;
        private AnyEnemyInView enemyInView;
        private AnyEnemyInView enemyInAttackRange;

        /// 注意attribute類別在creatureAI類別中
        public bool IsSeeingEnemy() => enemyInView.UpdateCreatureInView();

        public bool IsEnemyInAttackRange() => enemyInAttackRange.UpdateCreatureInView();

        public virtual void Update()
        {
            abstractCreature.Update();
            
            // 如果不使用ai
            if (!aiUsed) return;
            
            // 等待初始化，並切換成對應的動畫
            if (!aiState.Inited) return;

            /*if (GetType()==typeof(MonsterAI))
                Debug.Log(aiState.GetType().Name);*/

            aiState?.Update();
        }

        public override string ToString()
        {
            string info = GetType().Name;
            info += "\n" + abstractCreature.GetIsNotNullString();
            info += "\n" + aiState.GetIsNotNullString();
            info += "\n" + transform.GetIsNotNullString();
            info += "\n" + ChaseRange.GetIsNotNullString();
            info += "\n" + AttackRange.GetIsNotNullString();
            info += "\n" + "隊伍：\t" + team;
            return info;
        }
    }
}