using System;
using Main.Entity.Creature;
using Main.EventSystem.Event.CreatureEventSystem.Common;
using Main.EventSystem.Event.CreatureEventSystem.Decorator;

namespace Main.EventSystem.Event.CreatureEventSystem.StateEvent
{
    /// <summary>
    /// 控制creature, debuff狀態添加和移除
    /// </summary>
    public class DeBuff : AbstractCreatureEventC
    {
        public float Duration
        {
            get => EventAttr.MaxDuration;
            set => EventAttr.MaxDuration = value;
        }

        public DeBuff(AbstractCreature creature, Action preWork, Action postWork, float duration) :
            base(creature,0, duration) // 沒有cd
        {
            CauseEnter = new FuncCause(() => creature.CreatureAttr.Alive);
            PreWork = preWork;
            PostWork = postWork;
            InitCreatureEventOrder(EnumCreatureEventTag.None,EnumOrder.DeBuff);
        }

        public new void Invoke() => base.Invoke();

        protected override void Enter()
        {
        }

        protected override void Exit()
        {
        }
    }
}