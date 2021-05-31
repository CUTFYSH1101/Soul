using Event;
using Main.Common;
using Main.Entity.Attr;
using Test2;
using UnityEngine;

namespace Main.Entity.Controller
{
    public class MoveController
    {
        private readonly ICreature creature;
        private readonly DoubleInput dbClick;
        private readonly DashSkill dash;
        private ICreatureAttr GetCreatureAttr() => creature.GetCreatureAttr();

        private bool Movable => GetCreatureAttr().Movable;

        private bool CanNotControlled => GetCreatureAttr().CanNotControlled();

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
            dbClick = new DoubleInput(key,
                null, null, creature.GetRigidbody2D());
            dash = new DashSkill(creature, key,0.5f,
                () => SetState(MindState.Dash),
                () => SetState(MindState.Idle));
        }

        private void Move(float value)
        {
            if (value != 0)
            {
                creature.Move(true);
                float moveX = value * GetCreatureAttr().MoveSpeed;
                creature.GetRigidbody2D().SetMoveX(moveX);
                SetState(MindState.Move);
            }
            else
            {
                creature.Move(false);
                creature.GetRigidbody2D().SetMoveX(0);
                SetState(MindState.Idle);
            }
        }

        private void MoveCycle()
        {
            // 當按下按鍵->確認狀態->移動
            var dir = UnityEngine.Input.GetAxisRaw("Horizontal");
            if (dir != 0)
            {
                if (!Movable || CanNotControlled)
                {
                    Move(0);
                }

                if (!Grounded && !EnableAirControl)
                    return;
                Move(dir);
            }
            // 當鬆開按鍵且在地面上->停下
            else
            {
                if (!Grounded)
                    return;
                Move(0);
            }
        }

        public void Update()
        {
            if (dbClick.DoubleClick)
            {
                Dash();
            }

            if (!IsDashing)
            {
                MoveCycle();
            }
        }

        private void Dash()
        {
            var moveX = UnityEngine.Input.GetAxisRaw("Horizontal") *
                        GetCreatureAttr().DashForce;
            dash.Invoke(new Vector2(moveX, 0));
        }
        
    }
}