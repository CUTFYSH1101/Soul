using Main.Attribute;
using Main.Common;
using Main.Entity.Skill_210528;
using Main.Event;
using UnityEngine;
using Input = Main.Game.Input.Input;
using Main.Game.Input;

namespace Main.Entity
{
    public class MoveController
    {
        private readonly AbstractCreature abstractCreature;
        private readonly DBClick dbClick;
        private readonly DashSkill dash;
        private ICreatureAttr GetCreatureAttr() => abstractCreature.GetCreatureAttr();

        private bool Movable => GetCreatureAttr().Movable;

        private bool CanNotControlled => GetCreatureAttr().CanNotControlled();

        private bool CanNotMoving => GetCreatureAttr().CanNotMoving();

        private bool EnableAirControl => GetCreatureAttr().EnableAirControl;

        private bool Grounded => GetCreatureAttr().Grounded;

        private bool IsDashing => GetCreatureAttr().MindState == MindState.Dash;

        private bool AbleDashing
            => GetCreatureAttr().MindState == MindState.Move ||
               GetCreatureAttr().MindState == MindState.Move;

        private void SetState(MindState newState)
            => GetCreatureAttr().MindState = newState;

        public MoveController(AbstractCreature abstractCreature, string key)
        {
            this.abstractCreature = abstractCreature;
            dbClick = new DBClick(key, .3f);
            dash = new DashSkill(abstractCreature, 0.15f,
                () => SetState(MindState.Dash),
                () => SetState(MindState.Idle));
        }

        private void Move(float dir)
        {
            if (dir != 0)
            {
                abstractCreature.Move(true);
                float moveX = dir * GetCreatureAttr().MoveSpeed;
                abstractCreature.GetRigidbody2D().SetActiveX(moveX);
                SetState(MindState.Move);
            }
            else
            {
                abstractCreature.Move(false);
                abstractCreature.GetRigidbody2D().SetActiveX(0); // 當受到攻擊時，速度歸零
                if (abstractCreature.GetAnimator().IsTag("Attack"))
                {
                    return;
                }

                SetState(MindState.Idle);
            }
        }

        private void MoveCycle()
        {
            // 當按下按鍵->確認狀態->移動
            var dir = Input.GetAxisRaw(HotkeySet.Horizontal);
            if (dir != 0)
            {
                // 衝刺時，取消霸體
                /*if (!Movable || CanNotControlled)
                {
                    Move(0);
                }*/

                if ((Grounded || EnableAirControl) && !CanNotMoving)
                {
                    Move(dir);
                }
            }
            // 當鬆開按鍵且在地面上->停下
            else
            {
                if (Grounded)
                {
                    Move(0);
                }
            }
        }

        public void Update()
        {
            if (!@switch)
                return;

            if (dbClick.Cause())
            {
                Dash();
                // dbClick.Reset();
            }

            if (!IsDashing)
            {
                MoveCycle();
            }

            // 一旦受到攻擊，馬上停止移動
            if (CanNotMoving && Grounded)
            {
                Move(0);
            }
        }

        private void Dash()
        {
            // Grounded && Movable && !CanNotMoving
            if (Movable && !CanNotMoving)
            {
                var moveX = Input.GetAxisRaw("Horizontal") *
                            GetCreatureAttr().DashForce;
                dash.Invoke(new Vector2(moveX, 0));
            }
        }

        private bool @switch = true;

        public void Switch(bool value)
        {
            @switch = value;
            if (!@switch)
                Move(0);
        }
    }
}