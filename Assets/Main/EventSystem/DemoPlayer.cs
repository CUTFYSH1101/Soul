using System;
using Main.AnimAndAudioSystem.Audios.Scripts;
using JetBrains.Annotations;
using Main.CreatureAndBehavior.Behavior;
using Main.Entity.Creature;
using Main.EventSystem.Common;
using Main.EventSystem.Cause;
using Main.EventSystem.Event.CreatureEventSystem;
using Main.EventSystem.Event.CreatureEventSystem.MoveEvent;
using Main.EventSystem.Event.CreatureEventSystem.Skill;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Attribute;
using Main.EventSystem.Event.CreatureEventSystem.StateEvent;
using Main.Input;
using UnityEngine;
using static Main.AnimAndAudioSystem.Main.Common.EnumSymbol;
using DeBuff = Main.EventSystem.Common.DeBuff;
using Input = Main.Input.Input;

namespace Main.EventSystem
{
    public class DemoPlayer : AbstractCreature
    {
        private bool Grounded => base.CreatureAttr.Grounded;
        private new CreatureAttr CreatureAttr => base.CreatureAttr;
        private readonly SpurAttack _spurAttack;
        private readonly NormalAttack _normalAttack;
        private readonly DiveAttack _diveAttack;
        private readonly JumpAttack _jumpAttack;
        private readonly DashEvent _dashEvent;
        private readonly DBAxisClick _dbAxisClick;
        private readonly Knockback _knockback;
        private readonly MovingEvent _moveEvent;
        private bool _allowMoving = true;
        private JumpOrWallJump _jumpOrWallJump;

        private void InvokeNewEvent(Action thread)
        {
            thread?.Invoke();
        }

        internal DemoPlayer(Transform transform, [CanBeNull] DictionaryAudioPlayer audioPlayer = null) :
            base(new CreatureAttr(jumpForce: 30, dashForce: 30, diveForce: 60), transform, audioPlayer)
        {
            _normalAttack = new NormalAttack(this);
            _normalAttack.SkillAttr
                .SetKnockBack(force: 80, dynDirection: () => Vector2.right)
                .SetDeBuff(DeBuff.Stiff); // 給予擊退，搭配event
            _spurAttack = new SpurAttack(this);
            _spurAttack.SkillAttr.DeBuff = DeBuff.Stiff;
            _jumpAttack = new JumpAttack(this);
            _diveAttack = new DiveAttack(this, 0); // 沒有cd
            _dashEvent = new DashEvent(this);
            _dbAxisClick = new DBAxisClick(HotkeySet.Horizontal, .3f); // 雙擊時長0.3秒
            var behavior = new BaseBehaviorInterface(new CreatureInterface(this));
            _jumpOrWallJump = new JumpOrWallJump(this)
                .InitAction(behavior.Jump, behavior.WallJump);
            _knockback = new Knockback(this);
            _moveEvent = new MovingEvent(this, HotkeySet.Horizontal);

            // _diveAttack.CdTime = 7;
            UserInterface.CreateCdUI("UI/PanelCd/Skill", _diveAttack);
            _diveAttack.PreAction2 += UserInterface.CreateCameraShakerEvent(0.02f,0.1f).Invoke;
        }


        /// 更改受擊方的精神狀態，並產生對應行為
        public void Hit(SkillAttr skillAttr)
        {
            if (skillAttr == null)
                return;
            HitEvent.Invoke(this, _knockback, skillAttr);
        }

        public new void Update()
        {
            base.Update();
            if (_dbAxisClick.AndCause())
            {
                Debug.Log(_dbAxisClick.AxisRaw());
                _dashEvent.Invoke(new Vector2(_dbAxisClick.AxisRaw() * CreatureAttr.DashForce, 0));
            }

            // 當按下按鍵->確認狀態->跳躍
            /*AnimManager.JumpUpdate(
                jumpInterrupt
                    ? 0
                    : Math.Sign(Rigidbody2D.Velocity.y));*/

            if (Input.Input.GetButtonDown("Jump"))
            {
                // Jump();
                _jumpOrWallJump.Invoke();
            }

            if (Grounded)
            {
                if (Input.Input.GetButton(HotkeySet.Horizontal))
                {
                    if (Input.Input.GetButtonDown(HotkeySet.Fire1))
                    {
                        _spurAttack.Invoke(Square);
                    }

                    if (Input.Input.GetButtonDown(HotkeySet.Fire2))
                        _spurAttack.Invoke(Cross);

                    if (Input.Input.GetButtonDown(HotkeySet.Fire3))
                        _spurAttack.Invoke(Circle);
                }
                else
                {
                    if (Input.Input.GetButtonDown(HotkeySet.Fire1))
                    {
                        _normalAttack.Invoke(Square);
                    }

                    if (Input.Input.GetButtonDown(HotkeySet.Fire2))
                    {
                        _normalAttack.Invoke(Cross);
                    }

                    if (Input.Input.GetButtonDown(HotkeySet.Fire3))
                    {
                        _normalAttack.Invoke(Circle);
                    }
                }
            }
            else
            {
                if (Input.Input.GetButtonDown(HotkeySet.Control))
                {
                    _diveAttack.Invoke(Direct);
                }

                if (Input.Input.GetButtonDown(HotkeySet.Fire1))
                {
                    _jumpAttack.Invoke(Square);
                }

                if (Input.Input.GetButtonDown(HotkeySet.Fire2))
                {
                    _jumpAttack.Invoke(Cross);
                }

                if (Input.Input.GetButtonDown(HotkeySet.Fire3))
                {
                    _jumpAttack.Invoke(Circle);
                }
            }


            _moveEvent.MoveUpdate();

            if (UnityEngine.Input.GetKeyDown(KeyCode.G))
            {
                Hit(_normalAttack.SkillAttr);
            }

            AnimManager.Knockback(CreatureAttr.DuringDeBuff);
        }
    }
}