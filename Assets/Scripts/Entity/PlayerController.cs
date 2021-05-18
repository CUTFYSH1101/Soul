using System;
using System.Collections;
using System.Linq;
using Event;
using Extension.Common;
using Extension.Entity.Controller;
using JetBrains.Annotations;
using Main.Entity.Attr;
using Main.Util;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static Main.Util.Timers;
using Type = Main.Entity.Controller.ICreature.AttackAnimator.Type;

namespace Main.Entity.Controller
{
    [Serializable]
    public class PlayerController
    {
        private readonly ICreature creature;
        private MoveController moveController;
        public bool Switch { get; set; } = true;

        public PlayerController(ICreature creature)
        {
            this.creature = creature;
            moveController = new MoveController(creature, "Horizontal");
            // moveController.ToString().LogLine();
        }

        // TODO:later delete...
        private Rigidbody2D rb => GetCreatureAI().GetTransform()
            .GetComponent<Rigidbody2D>();

        public void Update()
        {
            /*if (!Input.anyKey)
                return;*/
            if (!Switch)
                return;
            // MoveCycle();
            moveController.Update();

            if (Input.GetButtonDown("Fire1"))
            {
                GetCreatureAI().Attack(ICreature.AttackAnimator.Type.Rect);
                // TODO: mindState=空中時，俯衝
                // if whereState==Air
                if (!GetCreatureAttr().Grounded)
                {
                    var dir = new Vector2(creature.IsFacingRight ? 1 : -1, -1);
                    rb.AddForce(dir * 10, ForceMode2D.Impulse);
                }
            }

            if (Input.GetButtonDown("Fire2"))
            {
                GetCreatureAI().Attack(ICreature.AttackAnimator.Type.Round);
            }

            if (Input.GetButtonDown("Fire3"))
            {
                GetCreatureAI().Attack(ICreature.AttackAnimator.Type.Cross);
            }

            // 當按下按鍵->確認狀態->跳躍
            if (Input.GetButtonDown("Jump"))
            {
                if (!Grounded && !EnableAirControl || CanNotControlled)
                    return;
                if (OnCollision())
                {
                    if (WallPos != default)
                    {
                        var dir = Math.Sign(creature.GetPosition().x - ((Vector2) WallPos).x);
                        rb.AddForce(new Vector2(dir, 2) * GetCreatureAttr().JumpForce);
                        if (dir > 0 && !creature.IsFacingRight)
                        {
                            creature.Flip();
                        }

                        if (dir < 0 && creature.IsFacingRight)
                        {
                            creature.Flip();
                        }
                    }

                    //TODO:flip
                }
                else
                {
                    creature.Jump();
                }
            }
        }

        private bool inited;
        private float size;

        private float Size
        {
            get
            {
                if (!inited)
                {
                    inited = true;
                    size = creature.GetTransform().GetComponent<CapsuleCollider2D>().size.x * .58f;
                }

                return size;
            }
        }

        private Collider2D[] wall;
        private Vector2? WallPos => wall.IsEmpty() ? default : wall[0].transform.position;

        private bool OnCollision()
        {
            // return creature.GetTransform().CircleCast(out var collider2Ds,creature.GetTransform().position, Size);
            return creature.GetTransform().CircleCast(out wall, collider2D => collider2D.CompareTag("Wall"),
                creature.GetTransform().position, Size);
        }

        // ======
        // quick short
        // ======
        private bool Movable => GetCreatureAttr().Movable;

        private bool CanNotControlled => GetCreatureAttr().CanNotControlled();

        private bool EnableAirControl => GetCreatureAttr().EnableAirControl;

        private bool Grounded => GetCreatureAttr().Grounded;

        private ICreatureAttr GetCreatureAttr() => creature.GetCreatureAttr();

        private ICreatureAI GetCreatureAI() => creature.GetCreatureAI();
    }

    public abstract class Skill
    {
        private enum State
        {
            First,
            Progressing,
            Finished
        }

        // TODO:尚未初始化
        private readonly Duration duration;

        // private readonly CoolDown coolDown;
        /// 獲取技能時長
        public float MDuration
        {
            get => duration.Timer;
            set => duration.Timer = value;
        }

        public Skill(Component component, Action onEvent, Action offEvent,
            float duration = .8f, float coolDown = 1)
        {
            Action on = onEvent + Enter;
            Action off = offEvent + Exit;
            this.duration = new Duration(component, duration, on, off);
            // this.coolDown = new CoolDown(component, coolDown);
        }

        public void Invoke()
        {
            /*if (duration.MindState != MindState.First || coolDown.MindState != MindState.First)
                return;*/
            Debug.Log($"skill: {duration.State}");
            if (duration.State != State.First)
                return;
            duration.CallInvoke();
        }

        protected abstract void Enter();

        protected abstract void Exit();

        private class Duration : Timers.RepeatMethod
        {
            private readonly Timer timer;
            private readonly Action onEvent, offEvent;
            private readonly RepeatMethod nextMethod;

            public float Timer
            {
                get => timer.Lag;
                set => timer.Lag = value;
            }

            public State State { get; private set; } = State.First;

            public Duration(Component component, float timer,
                Action onEvent = null, Action offEvent = null) : base(component)
            {
                this.timer = new Timer {Lag = timer};
                this.offEvent = offEvent;
                this.onEvent = onEvent;
            }

            public void CallInvoke() => Invoke();

            protected override void Invoke()
            {
                if (State != State.First)
                    return;
                base.Invoke();
            }

            protected override void Enter()
            {
                State = State.Progressing;
                onEvent?.Invoke();
                timer.Reset();
                Debug.Log($"Duration: {timer.IsTimeUp}");
            }

            protected override void Exit()
            {
                offEvent?.Invoke();
                timer.Reset();
                State = State.First;
            }

            protected override IEnumerator Update()
            {
                while (!timer.IsTimeUp)
                {
                    // Debug.Log($"Duration Update: {timer.IsTimeUp}");
                    timer.Update();
                    yield return new Update();
                }

                Exit();
            }

            // 會影響其他同名計時器!不要用!
            // mono.StopAllCoroutines();
        }

        private class CoolDown : RepeatMethod
        {
            private readonly Timer timer;
            public State State { get; private set; } = State.First;

            public CoolDown(Component component, float timer) : base(component)
            {
                this.timer = new Timer(timer);
            }

            protected override void Enter()
            {
                State = State.Progressing;
            }

            protected override void Exit()
            {
                State = State.First;
            }

            protected override IEnumerator Update()
            {
                while (!timer.IsTimeUp)
                {
                    timer.Update();
                    yield return new Update();
                }

                Exit();
            }
        }
    }

    // 封包移動邏輯
    internal class SprintController : Skill
    {
        /*private readonly Timer cd = new Timer {Lag = 2f};
        private readonly Timer duration = new Timer {Lag = .8f};*/
        private readonly ICreature creature;
        private Vector2 force;

        public SprintController(ICreature creature,
            [NotNull] Action onEvent, [NotNull] Action offEvent) : base(creature.GetRigidbody2D(),
            onEvent, offEvent)
        {
            this.creature = creature;
        }

        protected override void Enter()
        {
            creature.GetRigidbody2D().AddForce(force);
            // 開啟動畫
        }

        protected override void Exit()
        {
            creature.GetRigidbody2D().StopForceX();
            // 關閉動畫
        }

        public void Invoke(Vector2 force)
        {
            this.force = force;
            base.Invoke();
        }

        public override string ToString() => this.GetMembersToString();
    }

    internal class MoveController
    {
        private readonly ICreature creature;
        private readonly DoubleInput input;
        private readonly SprintController controller;
        private bool Movable => GetCreatureAttr().Movable;

        private bool CanNotControlled => GetCreatureAttr().CanNotControlled();

        private bool EnableAirControl => GetCreatureAttr().EnableAirControl;

        private bool Grounded => GetCreatureAttr().Grounded;

        public MoveController(ICreature creature, string key)
        {
            this.creature = creature;
            controller = new SprintController(creature, OnSprint, OffSprint);
            this.input = new DoubleInput(key,
                null, null, creature.GetRigidbody2D());
        }

        public void Update()
        {
            // Debug.Log("MoveUpdate " + input.ToString());
            if (input.DoubleClick)
            {
                Move(false);
                var moveX = new Vector2(GetCreatureAttr().SprintForce * Input.GetAxisRaw("Horizontal"), 0);
                controller.Invoke(moveX);
            }
            else
            {
                // 在衝刺中無法進行其他操作
                if (state == State.Sprint)
                {
                    return;
                }

                MoveCycle();
            }
        }

        private ICreatureAttr GetCreatureAttr()
        {
            return creature.GetCreatureAttr();
        }

        // ======
        // Debug
        // ======
        private void OnSprint() => state = State.Sprint;
        private void OffSprint() => state = State.Idle;
        private void OnMove() => state = State.Move;
        private void OffMove() => state = State.Idle;

        private void MoveCycle()
        {
            // 當按下按鍵->確認狀態->移動
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                if (!Movable || CanNotControlled)
                {
                    Move(false);
                }

                if (!Grounded && !EnableAirControl)
                    return;
                Move(true);
            }
            // 當鬆開按鍵且在地面上->停下
            else
            {
                if (!Grounded)
                    return;
                Move(false);
            }
        }

        private void Move(bool value)
        {
            if (value)
            {
                creature.Move(true);
                float moveX = Input.GetAxisRaw("Horizontal") * GetCreatureAttr().MoveSpeed;
                creature.GetRigidbody2D().SetMoveX(moveX);
                state = State.Move;
            }
            else
            {
                creature.Move(false);
                creature.GetRigidbody2D().SetMoveX(0);
                state = State.Idle;
            }
        }

        public override string ToString() => this.GetMembersToString();


        private enum State
        {
            Idle,
            Move,
            Sprint
        }

        private State state;

        public string GetState() => "狀態:" + state;

        /// 獲取衝刺時長(秒)，預設為0.5
        public float GetSprintTime() => controller.MDuration;

        /// 設定衝刺時長(秒)
        public void SetSprintTime(float second) => controller.MDuration = second;
    }
}