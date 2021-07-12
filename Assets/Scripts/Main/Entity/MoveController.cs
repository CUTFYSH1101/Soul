using System;
using JetBrains.Annotations;
using Main.Attribute;
using Main.Common;
using Main.Event;
using Input = Main.Game.Input.Input;
using Main.Game.Input;

namespace Main.Entity
{
    public class MoveController
    {
        [NotNull] private Action<float> move;
        [NotNull] private Action<float> dash;
        private readonly AbstractCreature creature;
        private readonly DBClick dbClick;
        private CreatureAttr GetCreatureAttr() => creature.GetCreatureAttr();
        private bool CanNotMoving => !GetCreatureAttr().MovableDyn;
        private bool EnableAirControl => GetCreatureAttr().EnableAirControl;
        private bool Grounded => GetCreatureAttr().Grounded;
        private bool IsDashing => GetCreatureAttr().MindState == MindState.Dash;

        public MoveController(AbstractCreature creature, string key)
        {
            this.creature = creature;
            dbClick = new DBClick(key, .3f);
        }

        public MoveController(AbstractCreature creature, string key,
            [NotNull] Action<float> move, [NotNull] Action<float> dash)
        {
            this.creature = creature;
            dbClick = new DBClick(key, .3f);
            this.move = move;
            this.dash = dash;
        }

        public void Init([NotNull] BaseBehavior baseBehavior)
        {
            move = baseBehavior.Move;
            dash = baseBehavior.Dash;
        }

        private void MoveCycle()
        {
            // 當按下按鍵->確認狀態->移動
            var dir = Input.GetAxisRaw(HotkeySet.Horizontal);
            if (dir != 0)
            {
                if ((Grounded || EnableAirControl) && !CanNotMoving)
                    move(dir);
                // 一旦受到攻擊，馬上停止移動。在空中不會停止位移
                else if (CanNotMoving)
                    move(0);

            }
            // 當鬆開按鍵且在地面上->停下
            else
            {
                if (Grounded) move(0);
            }
        }

        public void Update()
        {
            if (!@switch || CanNotMoving) return;

            if (dbClick.Cause()) Dash();
            // dbClick.Reset();

            if (!IsDashing) MoveCycle();
        }

        private void Dash() => dash(Input.GetAxisRaw(HotkeySet.Horizontal));

        // 強制開關
        /// <code>
        /// if(!GetCreatureAttr().MovableDyn || spurAttack.Invoke())
        ///     Switch(false)
        /// </code>
        private bool @switch = true;

        public void Switch(bool value)
        {
            @switch = value;
            if (!@switch) move(0);
        }
    }
}
// idle->key->not double key->move
// idle->key->double key->dash
// state: enableMove, enableDash