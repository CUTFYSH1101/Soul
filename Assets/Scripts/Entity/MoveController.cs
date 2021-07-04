using Event;
using Main.Common;
using Main.Entity.Attr;
using Main.Util;
using Test2;
using Test2.Causes;
using UnityEngine;
using Input = Main.Common.Input;

namespace Main.Entity.Controller
{
    public class MoveController
    {
        private readonly ICreature creature;
        private readonly DBClick dbClick;
        private readonly DashSkill dash;
        private ICreatureAttr GetCreatureAttr() => creature.GetCreatureAttr();

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

        public MoveController(ICreature creature, string key)
        {
            this.creature = creature;
            dbClick = new DBClick(key, .3f);
            dash = new DashSkill(creature, key, 0.25f,
                () => SetState(MindState.Dash),
                () => SetState(MindState.Idle));
        }

        private void Move(float dir)
        {
            if (dir != 0)
            {
                creature.Move(true);
                float moveX = dir * GetCreatureAttr().MoveSpeed;
                creature.GetRigidbody2D().SetActiveX(moveX);
                SetState(MindState.Move);
            }
            else
            {
                creature.Move(false);
                creature.GetRigidbody2D().SetActiveX(0); // 當受到攻擊時，速度歸零
                if (creature.GetAnimator().GetStateInfo().IsTag("Attack"))
                {
                    return;
                }
                SetState(MindState.Idle);
            }
        }

        private void MoveCycle()
        {
            // 當按下按鍵->確認狀態->移動
            var dir = Input.GetAxisRaw("Horizontal");
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
            if (Grounded && Movable && !CanNotMoving)
            {
                var moveX = Input.GetAxisRaw("Horizontal") *
                            GetCreatureAttr().DashForce;
                dash.Invoke(new Vector2(moveX, 0));
            }
        }
    }
}