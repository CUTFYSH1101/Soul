using System;
using System.Collections;
using System.Linq;
using Event;
using Extension.Common;
using Extension.Entity.Controller;
using Main.Entity.Attr;
using Main.Util;
using UnityEngine;
using static System.Reflection.BindingFlags;
using Type = Main.Entity.Controller.ICreature.AttackAnimator.Type;
using static Main.Entity.Controller.ICreature.AttackAnimator.Type;
using MyRigidbody2D = Main.Entity.Controller.Rigidbody2D;

namespace Main.Entity.Controller
{
    [Serializable]
    public class ICreature : IHitable
    {
        //TODO:血量
        public string Name => transform.name;

        public string MaxHP { get; private set; }
        public string NowHP { get; private set; }

        [SerializeField] private ICreatureAttr creatureAttr;
        private ICreatureAI creatureAI;
        private readonly Transform transform;
        private readonly Animator animator;
        private readonly Rigidbody2D rigidbody2D;
        public bool IsFacingRight => flipController.IsFacingRight;
        private FlipController flipController;
        private readonly Knockback _knockback; //關聯
        private readonly UseAnimator useAnimator;
        private AnimatorStateInfo GetStateInfo() => animator.GetCurrentAnimatorStateInfo(0);

        protected internal ICreature(ICreatureAttr creatureAttr, Transform transform)
        {
            this.creatureAttr = creatureAttr;
            this.transform = transform;
            animator = transform.GetOrLogComponent<Animator>();
            rigidbody2D = transform.GetOrAddComponent<Rigidbody2D>();
            useAnimator = new UseAnimator(animator, creatureAttr);
            flipController = new FlipController(transform);
            _knockback = new Knockback(transform.GetOrAddComponent<Rigidbody2D>(), HitEnter, HitExit);
        }

        public ICreatureAttr GetCreatureAttr() => creatureAttr;
        public void SetCreatureAI(ICreatureAI creatureAI) => this.creatureAI = creatureAI;
        public ICreatureAI GetCreatureAI() => creatureAI;

        public Transform GetTransform() => transform;
        public Vector2 GetPosition() => (Vector2) GetTransform().position;

        public Animator GetAnimator() => animator;

        public Rigidbody2D GetRigidbody2D() => rigidbody2D;
        public void Flip() => flipController.Filp();

        public bool IsKilled() => GetStateInfo().IsTag("Die");
        public void Killed() => useAnimator.Killed();

        public void Revival() => useAnimator.Revival();

        public void Attack(AttackAnimator.Type type = Direct)
        {
            // 等動畫播放完才可再次攻擊
            if (GetStateInfo().IsTag("Attack"))
                return;
            useAnimator.Attack(type);
        }

        public void DownAttack()
        {
            if (GetStateInfo().IsTag("Attack"))
                return;
            
        }

        public virtual void Move(bool @switch)
        {
            if (!useAnimator.GetGrounded())
                return;
            useAnimator.Move(@switch);
            flipController.Update();
        }

        public virtual void Jump()
        {
            if (!useAnimator.GetGrounded())
                return;
            creatureAttr.Grounded = false;
            rigidbody2D.AddForce(new Vector2(0, creatureAttr.JumpForce));
        }

        public virtual void GroundedCheckUpdate() => useAnimator.Update();

        /// 無視任何條件都會被擊退
        public void Hit(Vector2 direction, float force,
            Transform vfx = null, Vector2 position = default) =>
            _knockback.Use(direction, force, vfx == null ? null : vfx, position);

        public void HitEnter() => creatureAttr.MindState = MindState.Knockback;

        public void HitExit() => creatureAttr.MindState = MindState.Idle;

        public override string ToString()
        {
            string info = this.GetType().Name;
            info += "\n" + creatureAI.GetIsNotNullToString();
            info += "\n" + transform.GetIsNotNullToString();
            info += "\n" + animator.GetIsNotNullToString();
            return info;
        }

        // ======
        // 類別
        // ======
        /// 部分與animator相關的，包含attack、move、jump、landing，不包含knockback
        private class UseAnimator
        {
            private readonly Animator animator;
            private readonly AttackAnimator attackAnimator;
            private static readonly int Alive = Animator.StringToHash("Alive");
            private readonly MoveAnimator moveAnimator;
            private readonly GroundChecker groundChecker;

            private readonly ICreatureAttr attr;

            public UseAnimator(Animator animator, ICreatureAttr attr)
            {
                this.animator = animator;
                this.attr = attr;
                attackAnimator = new AttackAnimator(animator);
                moveAnimator = new MoveAnimator(animator);
                groundChecker = new GroundChecker(animator);
            }

            public void DownAttack()
            {
                
            }

            public void Attack(AttackAnimator.Type type) => attackAnimator.Attack(type);
            public void Move(bool @switch) => moveAnimator.Move(@switch);
            public bool GetGrounded() => groundChecker.GetGrounded();
            public void Killed() => animator.SetBool(Alive, false);
            public void Revival() => animator.SetBool(Alive, true);

            public void Update()
            {
                GroundedCheckUpdate();
            }

            private void GroundedCheckUpdate()
            {
                groundChecker.Update();
                attr.Grounded = groundChecker.GetGrounded();
            }
        }

        /// 你可以選擇要使用直接攻擊或是攻擊
        public class AttackAnimator
        {
            public static AttackAnimator CreateInstance(Animator animator)
            {
                return new AttackAnimator(animator);
            }

            private static readonly int ToAttack = Animator.StringToHash("ToAttack");
            private static readonly int ToNormalAttackRect = Animator.StringToHash("ToNormalAttackRect");
            private static readonly int ToNormalAttackRound = Animator.StringToHash("ToNormalAttackRound");
            private static readonly int ToNormalAttackCross = Animator.StringToHash("ToNormalAttackCross");
            private readonly Animator animator;
            private bool HasParameter(int nameHash) => animator.parameters.Any(value => value.nameHash == nameHash);
            private bool NotHasParameter(int nameHash) => animator.parameters.All(value => value.nameHash != nameHash);
            private readonly bool hasDirect;
            private readonly bool hasChoice;

            public AttackAnimator(Animator animator)
            {
                this.animator = animator;
                hasDirect = HasParameter(ToAttack);
                hasChoice = HasParameter(ToNormalAttackRect);
                /*hasChoice = HasParameter(ToNormalAttackRect) &&
                            HasParameter(ToNormalAttackRound) &&
                            HasParameter(ToNormalAttackCross);*/
            }

            public enum Type
            {
                Direct,
                Rect,
                Round,
                Cross
            }

            public void Attack(Type type)
            {
                switch (type)
                {
                    case Direct:
                        if (!hasDirect)
                        {
                            Debug.Log("動畫機不含有" + type);
                            return;
                        }

                        animator.SetTrigger(ToAttack);
                        break;
                    case Type.Rect:
                        if (!hasChoice)
                        {
                            Debug.Log("動畫機不含有" + type);
                            return;
                        }

                        animator.SetTrigger(ToNormalAttackRect);
                        break;
                    case Round:
                        if (!hasChoice)
                        {
                            Debug.Log("動畫機不含有" + type);
                            return;
                        }

                        animator.SetTrigger(ToNormalAttackRound);
                        break;
                    case Cross:
                        if (!hasChoice)
                        {
                            Debug.Log("動畫機不含有" + type);
                            return;
                        }

                        animator.SetTrigger(ToNormalAttackCross);
                        break;
                }
            }
        }

        /// 你只能選擇要用float或是bool二選一
        private class MoveAnimator
        {
            private static readonly int Moving = Animator.StringToHash("Moving");
            private static readonly int Speed = Animator.StringToHash("Speed");
            private readonly Animator animator;

            private enum Type
            {
                Float,
                Bool
            }

            private readonly Type type;

            public MoveAnimator(Animator animator)
            {
                this.animator = animator;
                type = animator.parameters.Any(value => value.nameHash == Moving) ? Type.Bool : Type.Float;
            }

            public void Move(bool @switch)
            {
                switch (type)
                {
                    case Type.Bool:
                        animator.SetBool(Moving, @switch);
                        break;
                    case Type.Float:
                        animator.SetFloat(Speed, @switch ? 1 : 0);
                        break;
                }
            }
        }

        [Serializable]
        private class FlipController
        {
            public bool IsFacingRight { get; set; } = false;
            private Rigidbody2D _rigidbody2D;

            public FlipController(Transform transform, bool isFacingRight = false)
            {
                _rigidbody2D = transform.GetOrAddComponent<Rigidbody2D>();
                this.IsFacingRight = isFacingRight;
            }

            public void Filp()
            {
                // Debug.Log("翻面");
                IsFacingRight = !IsFacingRight;
                _rigidbody2D.transform.localScale =
                    Vector3.Scale(_rigidbody2D.transform.localScale, new Vector3(-1, 1, 1));
            }

            public void Update()
            {
                if (_rigidbody2D.GetMoveX() > 0 && !IsFacingRight)
                {
                    Filp(); //往左翻
                }

                if (_rigidbody2D.GetMoveX() < 0 && IsFacingRight)
                {
                    Filp(); //往右翻
                }
            }
        }

        private class GroundChecker
        {
            private static readonly int Up = Animator.StringToHash("Up");
            private static readonly int Down = Animator.StringToHash("Down");
            private readonly Animator animator;
            private readonly Rigidbody2D rigidbody2D;
            private bool grounded;
            private readonly Vector2 offset;
            private bool inited;
            private float size;
            private static readonly int GroundMask = LayerMask.NameToLayer("Ground").GetLayerMask();

            private float Size
            {
                get
                {
                    if (!inited)
                    {
                        inited = true;
                        size = animator.GetOrAddComponent<CapsuleCollider2D>().size.x * .6f;
                    }

                    return size;
                }
            }

            public bool GetGrounded()
            {
                // grounded = type == Type.Platform;
                grounded = animator.CircleCast((Vector2) animator.transform.position + offset, Size,
                    GroundMask);
                return grounded;
            }

            private enum Type
            {
                Up,
                Down,
                Platform
            }

            private Type type;

            private readonly bool @switch;

            public GroundChecker(Animator animator)
            {
                this.animator = animator;
                this.rigidbody2D = animator.GetOrAddComponent<Rigidbody2D>();
                @switch = animator.parameters.Any(value => value.nameHash == Up);

                // 調整位置
                offset = new Vector2(0, Size);
            }

            public void Update()
            {
                if (!@switch)
                    return;
                if (!GetGrounded())
                {
                    var speed = rigidbody2D.Instance.velocity.y;
                    if (speed > .1)
                        type = Type.Up;
                    else if (speed < .1)
                        type = Type.Down;
                }
                else
                    type = Type.Platform;

                switch (type)
                {
                    case Type.Up:
                        animator.SetBool(Up, true);
                        animator.SetBool(Down, false);
                        break;
                    case Type.Down:
                        animator.SetBool(Up, false);
                        animator.SetBool(Down, true);
                        break;
                    case Type.Platform:
                        animator.SetBool(Up, false);
                        animator.SetBool(Down, false);
                        break;
                }
            }
        }

        private class Knockback
        {
            private bool knockbacked;
            private readonly Rigidbody2D rigidbody2D;
            private readonly MonoBehaviour mono;
            private readonly Action enter, exit;
            private const float Duration = .1f;
            private float timer = Duration + Time.time;
            private readonly KnockbackAnimator animator;

            public Knockback(Rigidbody2D rigidbody2D, Action enter, Action exit)
            {
                this.rigidbody2D = rigidbody2D;
                mono = rigidbody2D.GetComponent<MonoBehaviour>();
                this.enter = enter;
                this.exit = exit;

                animator = new KnockbackAnimator(rigidbody2D.GetOrAddComponent<Animator>());
            }

            // 使用擊退和特效
            public void Use(Vector2 direction, float force,
                Transform vfx = null, Vector2 position = default)
            {
                timer = Duration + Time.time;
                enter?.Invoke();
                Enter();
                mono.StartCoroutine(Update()); // update
                direction = direction.normalized;
                // direction = (direction + Vector2.down * direction.y).normalized; //調整方向
                // direction = direction.normalized + Vector2.down * direction.y; //調整方向
                rigidbody2D.AddForce(direction * force, ForceMode2D.Impulse);
                Use(vfx, direction, position);
            }

            // 使用特效
            private void Use(Transform vfx = null, Vector2 direction = default, Vector2 position = default)
            {
                if (vfx.IsEmpty())
                    return;
                UnityEngine.Component.Instantiate(vfx, position, Quaternion.LookRotation(direction));
            }

            private void Enter()
            {
                knockbacked = true;
                enter?.Invoke();
                animator.Use(true);
            }

            private void Exit()
            {
                knockbacked = false;
                exit?.Invoke();
                animator.Use(false);
                rigidbody2D.StopForceX();
            }

            private IEnumerator Update()
            {
                while (knockbacked)
                {
                    if (Time.time > timer)
                        Exit();
                    // if (rigidbody2D.Instance.velocity.magnitude < .1f) Exit();
                    yield return null;
                }
            }
        }

        private class KnockbackAnimator
        {
            private readonly Animator animator;
            private static readonly int Knockbacked = Animator.StringToHash("Knockbacked");
            private readonly bool hasValue;

            public KnockbackAnimator(Animator animator)
            {
                this.animator = animator;
                hasValue = animator.parameters.Any(value => value.nameHash == Knockbacked);
            }

            public void Use(bool value)
            {
                if (hasValue)
                    animator.SetBool(Knockbacked, value);
            }
        }
    }
}