using System;
using JetBrains.Annotations;
using Main.Common;
using Main.Game.Collision;
using Main.Util;
using Main.Util.Timers;
using UnityEngine;
using UnityEngine.Serialization;
using static Main.Common.Team;
using static Main.Common.Symbol;

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

        /*protected internal ICreatureAI(AbstractCreature abstractCreature, float attackRange = 0.4f, float chaseRange = 2f,
            Team team = Peace)
        {
            Init(abstractCreature, null, attackRange, chaseRange, team);
        }*/
        /*protected static readonly Vector2 AttackRange = new Vector2(0.85f, 1.5f);
        protected static readonly Vector2 ChaseRange = new Vector2(5f, 1.5f);*/
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

        /// 使用Trigger觸發
        /// <param name="type">傷害類型:直接/選擇</param>
        public void Attack(Symbol type = Direct)
        {
            // 時間到了才攻擊
            if (!attackCdTimer.IsTimeUp)
                return;
            attackCdTimer.Reset();

            // 切換動畫
            abstractCreature.NormalAttack(type);
        }


        public virtual void Update()
        {
            abstractCreature.Update();
            if (!aiUsed)
                return;
            aiState?.Update();
        }

        public override string ToString()
        {
            string info = GetType().Name;
            info += "\n" + abstractCreature.GetIsNotNullToString();
            info += "\n" + aiState.GetIsNotNullToString();
            info += "\n" + transform.GetIsNotNullToString();
            info += "\n" + ChaseRange.GetIsNotNullToString();
            info += "\n" + AttackRange.GetIsNotNullToString();
            info += "\n" + "隊伍：\t" + team;
            return info;
        }
    }
}