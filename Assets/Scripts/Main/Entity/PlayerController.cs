using System;
using Main.Attribute;
using static Main.Common.Symbol;
using Input = Main.Game.Input.Input;
using Main.Game.Input;
using UnityEngine;
using AttackType = Main.Common.Symbol;
using AudioKey = Main.Attribute.DictionaryAudioPlayer.Key;

namespace Main.Entity
{
    [Serializable]
    public class PlayerController
    {
        private readonly AbstractCreature creature;
        public bool Switch { get; set; } = true;

        public PlayerController(AbstractCreature creature)
        {
            this.creature = creature;
            playerBehavior = (PlayerBehavior) creature.GetBehavior();
            // playerBehavior = new PlayerBehavior(creature, () => creature.GetCreatureAttr().Grounded);
            /*InputManager inputManager = new InputManager();
            inputManager.AddEventListener(InputManager.Event.onKeyDown, Hotkeys.Control, () =>
            {

            });*/
        }

        public void Update()
        {
            if (!Switch)
                return;
            // MoveCycle();
            // moveController.Update();
            playerBehavior.Move();

            if (UnityEngine.Input.GetKeyDown(KeyCode.N))
            {
                creature.Hit(Vector2.right,0);
                // playerBehavior.Hit(new Vector2(1,1), 30);
                // playerBehavior.Killed();
            }

            if (Grounded)
            {
                if (Input.GetButton(HotkeySet.Horizontal))
                {
                    if (Input.GetButtonDown(HotkeySet.Fire1))
                    {
                        // moveController.Switch(false);
                        playerBehavior.SpurAttack(Square);
                    }

                    if (Input.GetButtonDown(HotkeySet.Fire2))
                    {
                        playerBehavior.SpurAttack(Cross);
                    }

                    if (Input.GetButtonDown(HotkeySet.Fire3))
                    {
                        playerBehavior.SpurAttack(Circle);
                    }
                }
                else
                {
                    if (Input.GetButtonDown(HotkeySet.Fire1))
                    {
                        playerBehavior.NormalAttack(Square);
                    }

                    if (Input.GetButtonDown(HotkeySet.Fire2))
                    {
                        playerBehavior.NormalAttack(Cross);
                    }

                    if (Input.GetButtonDown(HotkeySet.Fire3))
                    {
                        playerBehavior.NormalAttack(Circle);
                    }
                }
            }
            else
            {
                if (Input.GetButtonDown(HotkeySet.Control))
                {
                    playerBehavior.DiveAttack(Direct); // todo
                }

                if (Input.GetButtonDown(HotkeySet.Fire1))
                {
                    // moveController.Switch(false);
                    playerBehavior.JumpAttack(Square);
                }

                if (Input.GetButtonDown(HotkeySet.Fire2))
                {
                    playerBehavior.JumpAttack(Cross);
                }

                if (Input.GetButtonDown(HotkeySet.Fire3))
                {
                    playerBehavior.JumpAttack(Circle);
                }
            }

            // 當按下按鍵->確認狀態->跳躍
            if (Input.GetButtonDown("Jump"))
            {
                playerBehavior.Jump();
                // jumpController1.Jump();
            }
        }

        private PlayerBehavior playerBehavior;


        // ======
        // quick short
        // ======
        private bool Grounded => GetCreatureAttr().Grounded;

        private CreatureAttr GetCreatureAttr() => creature.GetCreatureAttr();

        private AbstractCreatureAI GetCreatureAI() => creature.GetCreatureAI();
    }
}