using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Event;
using JetBrains.Annotations;
using Main.Common;
using Main.Entity;
using Main.Util;
using UnityEngine;
using Main.Entity.Attr;
using static Main.Entity.Controller.Player.AttackAnimator.Type;
using Type = Main.Entity.Controller.Player.AttackAnimator.Type;
using static System.Reflection.BindingFlags;
using static Main.Common.Team;
using MyRigidbody2D = Main.Entity.Controller.Rigidbody2D;

namespace Main.Entity.Controller
{
    [Serializable]
    public class ICreatureAI
    {
        private bool AIUsed = true;
        private Message message;
        private Transform transform;
        private IAIState aiState;
        [SerializeField] private ICreature creature;
        private IAIStrategy aiStrategy;
        private float attackTimer = AttackCoolDown + Time.time;
        private const float AttackCoolDown = .2f;
        private float attackRange;
        private float chaseRange;
        [SerializeField] private Team team;
        public Transform GetTransform() => creature.GetTransform();
        public Message GetMessage() => message;
        public ICreature GetCreature() => creature;
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

        public void SetSwitch(bool value) => AIUsed = value;
        public bool GetSwitch() => AIUsed;

        /// 注意attribute類別在creatureAI類別中
        public bool IsSeeingEnemy() => EnemyInSeen.EnemyInRange();

        public bool IsEnemyInAttackRange() => EnemyInAttackRange.EnemyInRange();

        protected internal ICreatureAI(ICreature creature, float attackRange = 0.4f, float chaseRange = 2f,
            Team team = Peace)
        {
            Init(creature, null, attackRange, chaseRange, team);
        }

        protected internal void Init(ICreature creature, [CanBeNull] IAIStrategy aiStrategy, float attackRange = 0.4f,
            float chaseRange = 2f,
            Team team = Peace)
        {
            this.creature = creature;
            this.creature.SetCreatureAI(this);
            this.aiStrategy = aiStrategy;
            this.attackRange = attackRange;
            this.chaseRange = chaseRange;
            // InitAIState(new IdleAIState());
            this.team = team;
            this.transform = creature.GetTransform();
            this.message = transform.GetOrAddComponent<Message>(); // 因此遊戲物件上最少會掛載一個腳本

            EnemyInSeen = new Raycast2D(chaseRange, transform, team: team);
            EnemyInAttackRange = new Raycast2D(attackRange, transform, team: team);
        }

        public void ChangeAIState(IAIState newAIState)
        {
            aiState = null;
            aiState = newAIState;
            aiState.SetCreatureAI(this);
            aiState.SetAIStrategy(aiStrategy);
            EnemyInSeen.SetCallback(aiState.SetTarget);
            EnemyInAttackRange.SetCallback(aiState.SetTarget);
        }

        public IAIState GetAIState() => aiState;

        private Raycast2D EnemyInSeen;
        private Raycast2D EnemyInAttackRange;

        /// 使用Trigger觸發
        /// <param name="type">傷害類型:直接/選擇</param>
        public void Attack(ICreature.AttackAnimator.Type type = Direct)
        {
            // 時間到了才攻擊
            if (attackTimer > Time.time)
                return;
            attackTimer = AttackCoolDown + Time.time;

            // 切換動畫
            creature.Attack(type);
        }


        /// 使用bool觸發，結束時需要外部設定為false以關閉狀態
        public void Move(bool @switch)
        {
            creature.Move(@switch);
        }

        public virtual void Update()
        {
            creature.GroundedCheckUpdate();
            if (!AIUsed)
                return;
            aiState?.Update();
        }

        public override string ToString()
        {
            string info = this.GetType().Name;
            info += "\n" + creature.GetIsNotNullToString();
            info += "\n" + aiState.GetIsNotNullToString();
            info += "\n" + transform.GetIsNotNullToString();
            info += "\n" + attackRange.GetNotZeroToString();
            info += "\n" + attackRange.GetNotZeroToString();
            info += "\n" + "隊伍：\t" + team;
            return info;
        }
    }

    public class Raycast2D
    {
        // x軸向
        private readonly float range;
        private readonly Transform @object;
        private Vector2 origin => @object.position;
        private Vector2 end => new Vector2(origin.x + range, origin.y);
        private Vector2 direction => new Vector2(range, 0);
        private float distance => Math.Abs(range);
        private readonly Team team;

        private Action<ICreature> onTrigger;

        // private bool isFacingRight;
        // private float GetRange() => isFacingRight ? Math.Abs(range) : -Math.Abs(range);
        public void SetCallback(Action<ICreature> callback)
        {
            onTrigger = callback;
        }

        public Raycast2D(float range, Transform @object, bool isFacingRight = false, Team team = Team.Peace)
        {
            // this.isFacingRight = isFacingRight;
            this.range = range;
            this.@object = @object;
            this.team = team;
        }

        public bool AnyInRange()
        {
            var colliders = Physics2D.RaycastAll(origin, direction, distance);
            // 回傳任一一個進入範圍的creature
            return colliders.Any(IsInRange);
        }

        public bool EnemyInRange()
        {
            var colliders = Physics2D.RaycastAll(origin, direction, distance);
            return colliders.Any(hit2D =>
                IsInRange(hit2D) &&
                GetMessage(hit2D).IsEnemy(team));
            /*foreach (var hit2D in colliders)
            {
                if (hit2D.transform != @object)
                {
                    if (hit2D.transform.HasComponent<Message>())
                    {
                        var messenger = hit2D.transform.GetComponent<Message>();
                        if (!messenger.IsKilled() && messenger.IsEnemy(team))
                        {
                            var creature = messenger.GetCreature();
                            onTrigger?.Invoke(creature);
                            return true;
                        }
                    }
                }
            }

            // onTrigger?.Invoke(null);
            return false;*/
        }

        private bool IsInRange(RaycastHit2D hit2D)
        {
            if (IsSame(hit2D.transform))
            {
                onTrigger?.Invoke(lastMessenger.GetCreature());
                return true;
            }

            var hasMediator = false;
            hasMediator =
                hit2D.transform != @object &&
                hit2D.transform.HasComponent<Message>();
            if (hasMediator)
            {
                lastMessenger = hit2D.transform.GetComponent<Message>();
                hasMediator = !lastMessenger.IsKilled();
                if (hasMediator)
                {
                    onTrigger?.Invoke(lastMessenger.GetCreature());
                }

                return hasMediator;
            }
            else
            {
                return false;
            }
        }

        private Message GetMessage(RaycastHit2D hit2D)
        {
            if (IsSame(hit2D.transform))
                return lastMessenger;
            return hit2D.transform.HasComponent<Message>() ? hit2D.transform.GetComponent<Message>() : null;
        }

        private bool IsSame(Transform currentObject) =>
            !(lastObject is null) && lastObject.GetHashCode() == currentObject.GetHashCode();

        private Message lastMessenger;
        private Transform lastObject;
    }
}