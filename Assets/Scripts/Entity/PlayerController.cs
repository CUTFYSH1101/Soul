using System;
using System.Collections;
using System.Linq;
using Event;
using Main.Common;
using Main.Entity.Controller;
using JetBrains.Annotations;
using Main.Entity.Attr;
using Main.Util;
using Test2;
using Test2.Causes;
using UnityEngine;
using UnityEngine.PlayerLoop;
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
        private DiveAttack diveAttack;


        public PlayerController(ICreature creature)
        {
            this.creature = creature;
            moveController = new MoveController(creature, "Horizontal");
            // moveController.ToString().LogLine();

            diveAttack = new DiveAttack(creature, 0.2f);
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

                diveAttack.Invoke();
                /*if (!GetCreatureAttr().Grounded)
                {
                    var dir = new Vector2(creature.IsFacingRight ? 1 : -1, -1);
                    rb.AddForce(dir * 10, ForceMode2D.Impulse);
                }*/
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
}