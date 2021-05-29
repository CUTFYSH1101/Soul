using System;
using System.Collections;
using System.Linq;
using Event;
using Extension.Common;
using Extension.Entity.Controller;
using JetBrains.Annotations;
using Main.Entity.Attr;
using Main.Util;
using Test2;
using Test2.Causes;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static Main.Util.Timers;
using Input = UnityEngine.Input;
using Type = Main.Entity.Controller.ICreature.AttackAnimator.Type;

namespace Main.Entity.Controller
{
    [Serializable]
    public class PlayerController
    {
        private readonly ICreature creature;
        private MoveController moveController;
        public bool Switch { get; set; } = true;
        private DashSkill dashSkill;


        public PlayerController(ICreature creature)
        {
            this.creature = creature;
            moveController = new MoveController(creature, "Horizontal");
            // moveController.ToString().LogLine();

            dashSkill = new DashSkill(creature, "Horizontal");
        }

        // TODO:later delete...
        private Rigidbody2D rb => GetCreatureAI().GetTransform()
            .GetComponent<Rigidbody2D>();

        public void Update()
        {
            if (Input.anyKeyDown)
            {
                // dashSkill.Invoke(new Vector2(60,0));
            }
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

    internal class MoveController
    {
        private readonly ICreature creature;
        private readonly DoubleInput dbClick;
        private readonly DashSkill dash;
        private bool Movable => GetCreatureAttr().Movable;

        private bool CanNotControlled => GetCreatureAttr().CanNotControlled();

        private bool EnableAirControl => GetCreatureAttr().EnableAirControl;

        private bool Grounded => GetCreatureAttr().Grounded;

        public MoveController(ICreature creature, string key)
        {
            this.creature = creature;
            // dbClick = new DBClick(key, .5f);
            this.dbClick = new DoubleInput(key,
                null, null, creature.GetRigidbody2D());

            dash = new DashSkill(creature, key);
        }

        public void Update()
        {
            if (dbClick.DoubleClick)
            {
                Move(false);
                var moveX = new Vector2(GetCreatureAttr().SprintForce * Input.GetAxisRaw("Horizontal"), 0);
                dash.Invoke(moveX);
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

        /*
        /// 獲取衝刺時長(秒)，預設為0.5
        public float GetSprintTime() => controller.MDuration;

        /// 設定衝刺時長(秒)
        public void SetSprintTime(float second) => controller.MDuration = second;
    */
    }
}