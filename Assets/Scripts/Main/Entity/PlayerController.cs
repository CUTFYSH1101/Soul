using System;
using Main.Attribute;
using Main.Entity.Skill_210528;
using Main.Game.Collision;
using UnityEngine;
using static Main.Common.Symbol;
using Input = Main.Game.Input.Input;
using Main.Game.Input;
using Math = System.Math;
using AttackType = Main.Common.Symbol;
using AudioKey = Main.Attribute.DictionaryAudioPlayer.Key;
using Rigidbody2D = Main.Entity.Controller.Rigidbody2D;

namespace Main.Entity
{
    [Serializable]
    public class PlayerController
    {
        private readonly AbstractCreature abstractCreature;
        private MoveController moveController;
        public bool Switch { get; set; } = true;
        private DiveAttack diveAttack;
        private NormalAttack normalAttack;
        private SpurAttack spurAttack;

        public PlayerController(AbstractCreature abstractCreature)
        {
            this.abstractCreature = abstractCreature;
            moveController = new MoveController(abstractCreature, "Horizontal");
            // moveController.ToString().LogLine();

            diveAttack = new DiveAttack(abstractCreature); // todo
            normalAttack = new NormalAttack(abstractCreature);
            spurAttack = new SpurAttack(abstractCreature,
                () => moveController.Switch(false),
                () => moveController.Switch(true));

            /*InputManager inputManager = new InputManager();
            inputManager.AddEventListener(InputManager.Event.onKeyDown, Hotkeys.Control, () =>
            {

            });*/
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

            /*if (Input.GetButtonDown(Hotkeys.Control))
            {
                if (!Grounded)
                {
                    diveAttack.Invoke(AttackType.Direct);
                }
            }*/
            // Debug.Log(Hotkeys.GetPositive(Hotkeys.Fire1));
            if (Input.GetButtonDown(HotkeySet.Control))
            {
                diveAttack.Invoke(AttackType.Direct);
            }

            // TODO 問要怎麼更新到create裡面？ 這是專屬於角色的嗎
            if (Grounded)
            {
                if (Input.GetButton(HotkeySet.Horizontal))
                {
                    var dir = GetCreatureAI().GetCreature().IsFacingRight ? Vector2.right : Vector2.left;
                    if (Input.GetButtonDown(HotkeySet.Fire1))
                    {
                        // 播放動畫
                        // rb2D.AddForce.X
                        // moveController.Switch(false);
                        spurAttack.Invoke(Square);
                    }

                    if (Input.GetButtonDown(HotkeySet.Fire2))
                    {
                        // rb.AddForce_OnActive(dir * 20, ForceMode2D.Impulse);
                        spurAttack.Invoke(Cross);
                    }

                    if (Input.GetButtonDown(HotkeySet.Fire3))
                    {
                        // rb.AddForce_OnActive(dir * 20, ForceMode2D.Impulse);
                        spurAttack.Invoke(Circle);
                    }
                }
                else
                {
                    if (Input.GetButtonDown(HotkeySet.Fire1))
                    {
                        // GetCreatureAI().Attack(AbstractCreature.NormalAttackAnimator.Type.Square);
                        normalAttack.Invoke(AttackType.Square);
                        // audioAudioPlayer.Play(AudioSource, AudioKey.NASquare);
                    }

                    if (Input.GetButtonDown(HotkeySet.Fire2))
                    {
                        normalAttack.Invoke(AttackType.Cross);
                        // audioAudioPlayer.Play(AudioSource, AudioKey.NACircle);
                    }

                    if (Input.GetButtonDown(HotkeySet.Fire3))
                    {
                        normalAttack.Invoke(AttackType.Circle);
                        // audioAudioPlayer.Play(AudioSource, AudioKey.NACross);
                    }
                }
            }

            // 當按下按鍵->確認狀態->跳躍
            if (Input.GetButtonDown("Jump"))
            {
                if (CanNotControlled)
                    return;
                if (OnCollision())
                {
                    var dir = Math.Sign(abstractCreature.GetPosition().x - ((Vector2) wallPos).x);
                    rb.AddForce_OnActive(new Vector2(dir * 0.6f, 0.8f) * GetCreatureAttr().JumpForce);
                }
                else if (Grounded || EnableAirControl)
                {
                    abstractCreature.Jump();
                }
            }
        }

        private bool inited;
        private float bodyWidth;

        private float BodyWidth
        {
            get
            {
                if (inited) return bodyWidth;

                inited = true;
                bodyWidth = abstractCreature.GetTransform().GetComponent<CapsuleCollider2D>().size.x * 1.2f;
                return bodyWidth;
            }
        }

        private Vector2 wallPos;

        private bool OnCollision()
        {
            wallPos = abstractCreature.GetTransform().GetLeanOnWallPos(BodyWidth);
            return wallPos != default;
        }


        // ======
        // quick short
        // ======
        private bool Movable => GetCreatureAttr().Movable;

        private bool CanNotControlled => GetCreatureAttr().CanNotControlled();

        private bool EnableAirControl => GetCreatureAttr().EnableAirControl;

        private bool Grounded => GetCreatureAttr().Grounded;

        private ICreatureAttr GetCreatureAttr() => abstractCreature.GetCreatureAttr();

        private AbstractCreatureAI GetCreatureAI() => abstractCreature.GetCreatureAI();
    }
}