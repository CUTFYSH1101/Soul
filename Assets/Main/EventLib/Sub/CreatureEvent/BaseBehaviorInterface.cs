using System;
using Main.Entity.Creature;
using Main.EventLib.Common;
using Main.EventLib.Sub.CreatureEvent.Skill;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Main.EventLib.Sub.CreatureEvent
{
    public class BaseBehaviorInterface
    {
        private readonly CreatureInterface _interface;
        public BaseBehaviorInterface(CreatureInterface @interface) => _interface = @interface;
        public BaseBehaviorInterface(Creature creature) => _interface = new CreatureInterface(creature);

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

            _interface.GetAnim().Move(dir != 0 && dir != null);
            // 如果在空中，保留舊數值
            // 當受到攻擊時，速度歸零
            if (dir != null)
                _interface.GetRb2D().ActiveX =
                    Math.Sign((int) dir) *
                    _interface.GetAttr().MoveSpeed;
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
            _interface.GetAnim().Move(targetPos != null && targetPos != Vector2.zero);
            // 如果在空中，保留舊數值
            // 當受到攻擊時，速度歸零
            if (targetPos != null)
            {
                if (targetPos == Vector2.zero)
                    _interface.GetRb2D().GuideX = 0;
                else
                    _interface.GetRb2D().MoveTo((Vector2) targetPos, _interface.GetAttr().MoveSpeed,
                        _interface.GetAttr().JumpForce);
            }

            if (mindState != null)
                _interface.MindState = (EnumMindState) mindState;
        }

        public void Jump()
        {
            // 不在地面、設定為不可空中移動時、不能移動、正在負面狀態中
            if (!_interface.GetAttr().EnableMoveDyn)
                return;
            if (!_interface.GetAttr().Grounded && !_interface.GetAttr().EnableAirControl)
                return;
            _interface.GetAttr().Grounded = false; // 避免小跳
            _interface.GetRb2D().ResetY();
            _interface.GetRb2D().AddForce_OnActive(new Vector2(0, _interface.GetAttr().JumpForce),
                ForceMode2D.Impulse);
        }

        public void WallJump(int? dir)
        {
            if (!_interface.GetAttr().EnableMoveDyn)
                return;
            // 在地面上
            if (_interface.GetAttr().Grounded)
                return;

            _interface.GetAnim().WallJump(dir != null && dir != 0);

            if (dir != null)
            {
                _interface.GetRb2D().ResetAll(); // 避免bug
                _interface.GetRb2D()
                    .AddForce_OnActive(
                        new Vector2((int) dir * 0.6f, 0.8f) *
                        _interface.GetAttr().JumpForce,
                        ForceMode2D.Impulse);
            }

            // Debug.Log(_interface.GetRigidbody2D().Velocity.x);
        }

        public void Killed()
        {
            _interface.GetAnim().Killed(); // IsTag("Die") == true
            _interface.GetAttr().Alive = false;
        }

        public void Revival()
        {
            _interface.GetAnim().Revival(); // IsTag("Die") == false
            _interface.GetAttr().Alive = true;
        }
    }
}