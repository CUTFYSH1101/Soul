using System;
using Main.Attribute;
using Main.Common;
using Main.Extension.Util;
using Main.Util;
using UnityEngine;
using MyRigidbody2D = Main.Entity.Controller.Rigidbody2D;

namespace Main.Entity
{
    [Serializable]
    public class AbstractCreature
    {
        [SerializeField] private CreatureAttr creatureAttr;
        private AbstractCreatureAI abstractCreatureAI;
        private readonly Transform transform;
        private readonly Animator animator;
        private readonly MyRigidbody2D rigidbody2D;
        public bool IsFacingRight => flipController.IsFacingRight;
        private FlipController flipController;
        private readonly CreatureAnimManager animManager;
        private readonly GroundChecker groundChecker;
        private readonly DictionaryAudioPlayer audioPlayer;
        private AbstractCreatureBehavior behavior;
        protected void SetBehavior(AbstractCreatureBehavior behavior) => this.behavior = behavior;
        public AbstractCreatureBehavior GetBehavior() => behavior;

        protected internal AbstractCreature(CreatureAttr creatureAttr, Transform transform,
            DictionaryAudioPlayer audioPlayer)
        {
            this.creatureAttr = creatureAttr;
            this.transform = transform;
            this.audioPlayer = audioPlayer;
            animator = transform.GetOrLogComponent<Animator>();
            rigidbody2D = transform.GetOrAddComponent<MyRigidbody2D>();
            flipController = new FlipController(transform);
            groundChecker = new GroundChecker(animator);
            animManager = new CreatureAnimManager(animator);
        }


        public DictionaryAudioPlayer GetAudioPlayer() => audioPlayer;

        public CreatureAttr GetCreatureAttr() => creatureAttr;

        public void SetCreatureAI(AbstractCreatureAI abstractCreatureAI) =>
            this.abstractCreatureAI = abstractCreatureAI;

        public AbstractCreatureAI GetCreatureAI() => abstractCreatureAI;

        public Transform GetTransform() => transform;
        public Vector2 GetPosition() => GetTransform().position;

        public CreatureAnimManager GetAnimator() => animManager;

        public MyRigidbody2D GetRigidbody2D() => rigidbody2D;
        public void Flip() => flipController.Flip();
        public bool IsKilled() => animManager.IsTag("Die");
        public void Killed() => behavior.Killed();

        public void Revival() => behavior.Revival();

        public void SetMindState(MindState newMindState) => creatureAttr.MindState = newMindState;

        public virtual void Update()
        {
            creatureAttr.Grounded = groundChecker.GetGrounded();
            groundChecker.Update();
            flipController.Update();
        }

        /// 無視任何條件都會被擊退
        public void Hit(Vector2 direction, float force,
            Transform vfx = null, Vector2 position = default) =>
            behavior.Hit(direction, force, vfx == null ? null : vfx, position);

        public override string ToString()
        {
            string info = this.GetType().Name;
            info += "\n" + abstractCreatureAI.GetIsNotNullString();
            info += "\n" + transform.GetIsNotNullString();
            info += "\n" + animator.GetIsNotNullString();
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