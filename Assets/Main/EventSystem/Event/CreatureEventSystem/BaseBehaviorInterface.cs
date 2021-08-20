using System;
using System.Numerics;
using JetBrains.Annotations;
using Main.EventSystem.Common;
using Main.EventSystem.Event.CreatureEventSystem.Skill;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Main.EventSystem.Event.CreatureEventSystem
{
    public class BaseBehaviorInterface
    {
        private readonly CreatureInterface _interface;
        public BaseBehaviorInterface(CreatureInterface @interface) => _interface = @interface;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir">null表示不設定，0表示物體速度歸零</param>
        /// <param name="mindState">null表示不設定</param>
        public void Move(int? dir, EnumMindState? mindState)
        {
            // 如果為負面狀態，SetActiveX為0，由外部更改state
            // 如果成功執行，狀態更改為move
            // 如果自主停下，而非負面，狀態更改為idle
            // 如果在空中，停止動畫，保留ActiveX的速度，不設定為0

            _interface.GetAnimManager().Move(dir != 0 && dir != null);
            // 如果在空中，保留舊數值
            // 當受到攻擊時，速度歸零
            if (dir != null)
                _interface.GetRigidbody2D().ActiveX =
                    Math.Sign((int) dir) *
                    _interface.GetCreatureAttr().MoveSpeed;
            // 當受到攻擊，不更改狀態，以免干擾
            if (mindState != null)
                _interface.MindState = (EnumMindState) mindState;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetPos">null表示不設定，Vector2.zero表示物體速度歸零</param>
        /// <param name="mindState">null表示不設定</param>
        public void MoveTo(Vector2? targetPos, EnumMindState? mindState)
        {
            _interface.GetAnimManager().Move(targetPos != null && targetPos != Vector2.zero);
            // 如果在空中，保留舊數值
            // 當受到攻擊時，速度歸零
            if (targetPos != null)
            {
                if (targetPos == Vector2.zero)
                    _interface.GetRigidbody2D().GuideX = 0;
                else
                    _interface.GetRigidbody2D().MoveTo((Vector2) targetPos, _interface.GetCreatureAttr().MoveSpeed,
                        _interface.GetCreatureAttr().JumpForce);
            }

            if (mindState != null)
                _interface.MindState = (EnumMindState) mindState;
        }

        public void Jump()
        {
            // 不在地面、設定為不可空中移動時、不能移動、正在負面狀態中
            if (!_interface.GetCreatureAttr().MovableDyn)
                return;
            if (!_interface.GetCreatureAttr().Grounded && !_interface.GetCreatureAttr().EnableAirControl)
                return;
            _interface.GetCreatureAttr().Grounded = false; // 避免小跳
            _interface.GetRigidbody2D().ResetY();
            _interface.GetRigidbody2D().AddForce_OnActive(new Vector2(0, _interface.GetCreatureAttr().JumpForce),
                ForceMode2D.Impulse);
        }

        public void WallJump(int? dir)
        {
            if (!_interface.GetCreatureAttr().MovableDyn)
                return;
            // 在地面上
            if (_interface.GetCreatureAttr().Grounded)
                return;

            _interface.GetAnimManager().WallJump(dir != null && dir != 0);

            if (dir != null)
            {
                _interface.GetRigidbody2D().ResetAll(); // 避免bug
                _interface.GetRigidbody2D()
                    .AddForce_OnActive(
                        new Vector2((int) dir * 0.6f, 0.8f) *
                        _interface.GetCreatureAttr().JumpForce,
                        ForceMode2D.Impulse);
            }

            Debug.Log(_interface.GetRigidbody2D().Velocity.x);
        }

        public void Killed()
        {
            _interface.GetAnimManager().Killed(); // IsTag("Die") == true
            _interface.GetCreatureAttr().MindState = EnumMindState.Dead;
            _interface.GetCreatureAttr().Alive = false;
        }

        public void Revival()
        {
            _interface.GetAnimManager().Revival(); // IsTag("Die") == false
            _interface.GetCreatureAttr().MindState = EnumMindState.Idle;
            _interface.GetCreatureAttr().Alive = true;
        }
    }
}