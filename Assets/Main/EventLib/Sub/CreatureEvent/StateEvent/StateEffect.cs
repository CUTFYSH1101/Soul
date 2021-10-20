using System;
using System.Diagnostics.CodeAnalysis;
using Main.EventLib.Main.EventSystem.Main;
using Main.Entity.Creature;
using Main.EventLib.Main.EventSystem;
using Main.EventLib.Main.EventSystem.EventOrderbySystem;
using Main.EventLib.Main.EventSystem.Main.Interface;
using Main.EventLib.Main.EventSystem.Main.Sub.Interface;
using Main.EventLib.Sub.CreatureEvent.Skill;

namespace Main.EventLib.Sub.CreatureEvent.StateEvent
{
    /// <summary>
    /// 控制creature, debuff狀態添加和移除
    /// </summary>
    [SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
    public class StateEffect : AbsEventObject, IEvent2, IWorkOnCreature
    {
        public StateEffect(Creature creature, Action preWork, Action postWork, float duration)
        {
            Director = this.Build(creature, EnumOrder.DeBuff, EnumCreatureEventTag.None);
            EventAttr = new EventAttr(maxDuration: duration);
            
            FilterIn = () => creature.CreatureAttr.Alive;
            PreWork = preWork;
            PostWork = postWork;
        }
        public float Duration
        {
            get => EventAttr.MaxDuration;
            set => EventAttr.MaxDuration = value;
        }
        public void Execute() => Director.CreateEvent();
        public void Enter()
        {
        }

        public void Exit()
        {
        }

        public Func<bool> FilterIn { get; }
        public Func<bool> ToInterrupt { get; }
        public CreatureInterface CreatureInterface { get; set; }
    }
    /*public class DeBuff : AbstractCreatureEventC
    {
        public float Duration
        {
            get => EventAttr.MaxDuration;
            set => EventAttr.MaxDuration = value;
        }

        public DeBuff(Creature creature, Action preWork, Action postWork, float duration) :
            base(creature,0, duration) // 沒有cd
        {
            CauseEnter = () => creature.CreatureAttr.Alive);
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
    }*/
}