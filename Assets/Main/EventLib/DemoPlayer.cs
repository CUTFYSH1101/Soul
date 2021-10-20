using System;
using JetBrains.Annotations;
using Main.Entity.Creature;
using Main.EventLib.Common;
using Main.EventLib.Condition;
using Main.EventLib.Sub.CreatureEvent;
using Main.EventLib.Sub.CreatureEvent.MoveEvent;
using Main.EventLib.Sub.CreatureEvent.Skill;
using Main.EventLib.Sub.CreatureEvent.Skill.Attribute;
using Main.EventLib.Sub.CreatureEvent.StateEvent;
using Main.Input;
using Main.Res.Script;
using Main.Res.Script.Audio;
using UnityEngine;
using static Main.Res.Script.EnumShape;

namespace Main.EventLib
{
    public class DemoPlayer : Creature
    {
        private bool Grounded => base.CreatureAttr.Grounded;
        private new CreatureAttr CreatureAttr => base.CreatureAttr;
        private readonly AtkSpur _atkSpur;
        private readonly AtkNormal _atkNormal;
        private readonly AtkDive _atkDive;
        private readonly AtkJump _atkJump;
        private readonly DashEvent _dashEvent;
        private readonly DBAxisClick _dbAxisClick;
        private readonly Knockback _knockback;
        private readonly MovingEvent _moveEvent;
        // private bool _allowMoving = true;
        private JumpOrWallJump _jumpOrWallJump;

        private void InvokeNewEvent(Action thread)
        {
            thread?.Invoke();
        }

        internal DemoPlayer(Transform transform, [CanBeNull] DictAudioPlayer audioPlayer = null) :
            base(transform, new CreatureAttr(jumpForce: 30, dashForce: 30, diveForce: 60), audioPlayer)
        {
            _atkNormal = new AtkNormal(this);
            _atkNormal.SkillAttr
                .SetKnockBack(force: 80, dynDirection: () => Vector2.right)
                .SetDebuff(EnumDebuff.Stiff); // 給予擊退，搭配event
            _atkSpur = new AtkSpur(this)
            {
                SkillAttr =
                {
                    Debuff = EnumDebuff.Stiff
                }
            };
            _atkJump = new AtkJump(this);
            _atkDive = new AtkDive(this); // 沒有cd
            _dashEvent = new DashEvent(this);
            _dbAxisClick = new DBAxisClick(HotkeySet.Horizontal, .3f); // 雙擊時長0.3秒
            var behavior = new BaseBehaviorInterface(new CreatureInterface(this));
            _jumpOrWallJump = new JumpOrWallJump(this)
                .InitAction(behavior.Jump, behavior.WallJump);
            _knockback = new Knockback(this);
            _moveEvent = new MovingEvent(this, HotkeySet.Horizontal);

            // _diveAttack.CdTime = 7;
            UserInterface.CreateCdUI("UI/PanelCd/Skill", _atkDive);
            _atkDive.AfterTouchGround += UserInterface.CreateCameraShakerEvent(0.02f,0.1f).Execute;
        }


        /// 更改受擊方的精神狀態，並產生對應行為
        public void Hit(SkillAttr skillAttr)
        {
            if (skillAttr == null)
                return;
            HitEvent.Execute(this, _knockback, skillAttr);
        }
        
        private const EnumShape Fire1 = Square;
        private const EnumShape Fire2 = Cross;
        private const EnumShape Fire3 = Circle;
        public new void Update()
        {
            base.Update();
            if (_dbAxisClick.AndCause())
            {
                // Debug.Log(_dbAxisClick.AxisRaw());
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
                        _atkSpur.Execute(Fire1);

                    if (Input.Input.GetButtonDown(HotkeySet.Fire2))
                        _atkSpur.Execute(Fire2);

                    if (Input.Input.GetButtonDown(HotkeySet.Fire3))
                        _atkSpur.Execute(Fire3);
                }
                else
                {
                    if (Input.Input.GetButtonDown(HotkeySet.Fire1))
                    {
                        _atkNormal.Execute(Fire1);
                    }

                    if (Input.Input.GetButtonDown(HotkeySet.Fire2))
                    {
                        _atkNormal.Execute(Fire2);
                    }

                    if (Input.Input.GetButtonDown(HotkeySet.Fire3))
                    {
                        _atkNormal.Execute(Fire3);
                    }
                }
            }
            else
            {
                if (Input.Input.GetButtonDown(HotkeySet.Control))
                {
                    _atkDive.Execute(Direct);
                }

                if (Input.Input.GetButtonDown(HotkeySet.Fire1))
                {
                    _atkJump.Execute(Fire1);
                }

                if (Input.Input.GetButtonDown(HotkeySet.Fire2))
                {
                    _atkJump.Execute(Fire2);
                }

                if (Input.Input.GetButtonDown(HotkeySet.Fire3))
                {
                    _atkJump.Execute(Fire3);
                }
            }


            _moveEvent.MoveUpdate();

            if (UnityEngine.Input.GetKeyDown(KeyCode.G))
            {
                Hit(_atkNormal.SkillAttr);
            }

            AnimInterface.Knockback(CreatureAttr.DuringDeBuff);
        }
    }
}