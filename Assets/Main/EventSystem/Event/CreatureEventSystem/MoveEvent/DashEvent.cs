using Main.Entity.Creature;
using Main.EventSystem.Common;
using Main.EventSystem.Event.Attribute;
using Main.EventSystem.Event.CreatureEventSystem.Common;
using Main.EventSystem.Event.CreatureEventSystem.Decorator;
using UnityEngine;
using static Main.AnimAndAudioSystem.Audios.Scripts.DictionaryAudioPlayer.Key;


namespace Main.EventSystem.Event.CreatureEventSystem.MoveEvent
{
    public class DashEvent : AbstractCreatureEventA
    {
        // 有冷卻時間和持續時間條件
        private bool _inited;

        private bool NotMove()
        {
            if (!_inited) return false; // 避免一開始就使條件成立
            return !CreatureInterface.GetRigidbody2D().IsMoving;
        }

        private Vector2 _force;

        private void SetForce(Vector2 value)
        {
            _force = value;
            _inited = true;
        }

        // dbClick -> onEnter
        // CauseDuration.IsTimeUp || math.abs rigidbody.velocity.x <= 0.1f -> onExit
        public DashEvent(Creature creature,
            float cdTime = 0.8f, float duration = 0.15f) :
            base(creature, new EventAttr(cdTime, duration))
        {
            CauseEnter = new FuncCause(() =>
                CreatureInterface.GetCreatureAttr().MovableDyn && _inited); // 防呆。事件執行順序：SetForce->Enter
            CauseExit = new FuncCause(NotMove);

            PreWork += () => creature.SetMindState(EnumMindState.Dash);
            PostWork += () => creature.SetMindState(EnumMindState.Idle);

            FinalEvent += () =>
            {
                // 在空中會繼續向前移動一段距離，在地面則直接停下
                CreatureInterface.GetRigidbody2D().ActiveX *= CreatureInterface.Grounded ? 0 : 0.3f;
                CreatureInterface.GetRigidbody2D().UseGravity(true);
                CreatureInterface.GetAnimManager().Dash(false);
            };
            InitCreatureEventOrder(EnumCreatureEventTag.Dash, EnumOrder.Dash);
        }

        public void Invoke(Vector2 dir)
        {
            if (State != EnumState.Free) return;
            SetForce(dir.normalized * CreatureInterface.GetCreatureAttr().DiveForce);
            base.Invoke();
        }

        protected override void Enter()
        {
            _inited = false;
            // 開啟動畫
            CreatureInterface.Play(Dash);
            CreatureInterface.GetAnimManager().Dash(true);
            CreatureInterface.GetRigidbody2D().UseGravity(false);
            CreatureInterface.GetRigidbody2D().ResetAll(); // 使Y軸不變
            CreatureInterface.AddForce_OnActive(_force, ForceMode2D.Impulse);
        }

        protected override void Update()
        {
            CreatureInterface.GetRigidbody2D().instance.velocity =
                new Vector2(CreatureInterface.GetRigidbody2D().Velocity.x, 0); // 強制霸體
        }

        protected override void Exit()
        {
            // 在空中會繼續向前移動一段距離，在地面則直接停下
            // 避免錯誤更改，在diveAttack時 todo
            CreatureInterface.GetRigidbody2D().ActiveX *= CreatureInterface.Grounded ? 0 : 0.3f;
            // 關閉
            CreatureInterface.GetRigidbody2D().UseGravity(true);
            CreatureInterface.GetAnimManager().Dash(false);
        }
    }
}