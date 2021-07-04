using System;
using Main.Attribute;
using Main.Common;
using Main.Entity.Skill_210528;
using Main.Event;
using Main.Game.Anim;
using Main.Util;
using UnityEngine;
using static Main.Common.Symbol;
using MyRigidbody2D = Main.Entity.Controller.Rigidbody2D;

namespace Main.Entity
{
    [Serializable]
    public class AbstractCreature : IHitable
    {
        //TODO:血量
        public string Name => transform.name;

        public string MaxHP { get; private set; }
        public string NowHP { get; private set; }

        [SerializeField] private ICreatureAttr creatureAttr;
        private AbstractCreatureAI abstractCreatureAI;
        private readonly Transform transform;
        private readonly Animator animator;
        private readonly MyRigidbody2D rigidbody2D;
        public bool IsFacingRight => flipController.IsFacingRight;
        private FlipController flipController;
        private readonly KnockbackSkill knockbackSkill; //關聯
        private readonly CreatureAnimManager animManager;
        private readonly GroundChecker groundChecker;
        private readonly DictionaryAudioPlayer audioPlayer;

        private AnimatorStateInfo GetStateInfo() => animator.GetStateInfo();

        protected internal AbstractCreature(ICreatureAttr creatureAttr, Transform transform,
            DictionaryAudioPlayer audioPlayer)
        {
            this.creatureAttr = creatureAttr;
            this.transform = transform;
            animator = transform.GetOrLogComponent<Animator>();
            rigidbody2D = transform.GetOrAddComponent<MyRigidbody2D>();
            animManager = new CreatureAnimManager(animator);
            flipController = new FlipController(transform);
            knockbackSkill = new KnockbackSkill(this, HitEnter, HitExit);
            groundChecker = new GroundChecker(animator);
            this.audioPlayer = audioPlayer;
        }

        public DictionaryAudioPlayer GetAudioPlayer() => audioPlayer;

        public ICreatureAttr GetCreatureAttr() => creatureAttr;

        public void SetCreatureAI(AbstractCreatureAI abstractCreatureAI) =>
            this.abstractCreatureAI = abstractCreatureAI;

        public AbstractCreatureAI GetCreatureAI() => abstractCreatureAI;

        public Transform GetTransform() => transform;
        public Vector2 GetPosition() => GetTransform().position;

        public CreatureAnimManager GetAnimator() => animManager;

        public MyRigidbody2D GetRigidbody2D() => rigidbody2D;
        public void Flip() => flipController.Flip();

        public bool IsKilled() => GetStateInfo().IsTag("Die");
        public void Killed() => animManager.Killed();

        public void Revival() => animManager.Revival();

        public void SetMindState(MindState newMindState) => creatureAttr.MindState = newMindState;

        /// 注意如果@switch為false某些事件不觸發
        public void DiveAttack(Symbol type, Vector2 direction, float force)
        {
            /*// 等動畫播放完才可再次攻擊
            if (GetStateInfo().IsTag("Attack"))
                return;*/
            direction = direction.normalized;
            rigidbody2D.AddForce_OnActive(direction * force, ForceMode2D.Impulse);
            animManager.DiveAttack(type, true);
        }

        public void DiveAttackStop(Symbol type) => animManager.DiveAttack(type, false);

        public void NormalAttack(Symbol type = Direct)
        {
            // 等動畫播放完才可再次攻擊
            if (GetStateInfo().IsTag("Attack"))
                return;
            animManager.Attack(type);
        }

        public void DownAttack()
        {
            if (GetStateInfo().IsTag("Attack"))
                return;
        }

        /// 回傳是否執行成功
        /// <returns>是否執行成功</returns>
        public virtual bool Move(bool @switch)
        {
            var executed = groundChecker.GetGrounded() && creatureAttr.Movable;
            if (executed)
            {
                animManager.Move(@switch);
                // flipController.Invoke();
            }

            return executed;
        }

        public virtual void Jump()
        {
            if (!groundChecker.GetGrounded() || !creatureAttr.Movable || creatureAttr.CanNotControlled())
                return;
            creatureAttr.Grounded = false;
            rigidbody2D.AddForce_OnActive(new Vector2(0, creatureAttr.JumpForce));
        }

        public virtual void Update()
        {
            creatureAttr.Grounded = groundChecker.GetGrounded();
            groundChecker.Update();
            flipController.Update();
        }

        /// 無視任何條件都會被擊退
        public void Hit(Vector2 direction, float force,
            Transform vfx = null, Vector2 position = default) =>
            knockbackSkill.Invoke(direction, force, vfx == null ? null : vfx, position);

        public void HitEnter() => creatureAttr.MindState = MindState.Knockback;

        public void HitExit() => creatureAttr.MindState = MindState.Idle;

        public override string ToString()
        {
            string info = this.GetType().Name;
            info += "\n" + abstractCreatureAI.GetIsNotNullToString();
            info += "\n" + transform.GetIsNotNullToString();
            info += "\n" + animator.GetIsNotNullToString();
            return info;
        }

        // ======
        // 類別
        // ======
        private class GroundChecker
        {
            private readonly CreatureAnimManager moveAnim;
            private readonly IEntity.GroundChecker groundChecker;

            public GroundChecker(Animator animator)
            {
                groundChecker = new IEntity.GroundChecker(animator.transform);
                moveAnim = new CreatureAnimManager(animator);
            }

            public bool GetGrounded() => groundChecker.Grounded();

            public void Update() => moveAnim.JumpUpdate(groundChecker.VerticalDir);
        }


        [Serializable]
        private class FlipController
        {
            public bool IsFacingRight { get; private set; }
            private MyRigidbody2D rigidbody2D;

            public FlipController(Transform transform, bool isFacingRight = false)
            {
                rigidbody2D = transform.GetOrAddComponent<MyRigidbody2D>();
                this.IsFacingRight = isFacingRight;
            }

            public void Flip()
            {
                // Debug.Log("翻面");
                IsFacingRight = !IsFacingRight;
                rigidbody2D.transform.localScale =
                    Vector3.Scale(rigidbody2D.transform.localScale, new Vector3(-1, 1, 1));
            }

            public void Update()
            {
                if (rigidbody2D.GetActiveX() + rigidbody2D.GetGuideX() > 0 && !IsFacingRight)
                {
                    Flip(); //往左翻
                }

                if (rigidbody2D.GetActiveX() + rigidbody2D.GetGuideX() < 0 && IsFacingRight)
                {
                    Flip(); //往右翻
                }
            }
        }
    }
}