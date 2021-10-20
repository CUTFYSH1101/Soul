using System;
using Main.EventLib.Main.EventSystem.Main;
using Main.Entity.Creature;
using Main.EventLib.Main.EventSystem;
using Main.EventLib.Main.EventSystem.EventOrderbySystem;
using Main.EventLib.Main.EventSystem.Main.EventBuilder;
using Main.EventLib.Main.EventSystem.Main.Interface;
using Main.EventLib.Main.EventSystem.Main.Sub.Interface;
using Main.EventLib.Sub.CreatureEvent.Skill;
using UnityEngine;

namespace Main.EventLib.Sub.CreatureEvent.Life
{
    public class LifeEvent : IEvent2, IWorkOnCreature
    {
        public bool IsOpen { get; set; }
        public EnumState State { get; set; }
        public Action PreWork { get; set; }
        public Action PostWork { get; set; }
        public Action FinalAct { get; set; }
        public EventAttr EventAttr { get; set; }
        private IEventDirector _builder;
        private bool _isKilled;
        public LifeEvent(Creature creature)
        {
            _builder = this.Build(creature, EnumOrder.Life, EnumCreatureEventTag.Life);
            EventAttr = new EventAttr(0, 2);
            FinalAct += () => CreatureInterface.GetAnim().Interrupt(); // 加速復活和死亡的流程
        }
        public void Killed()
        {
            _isKilled = true;
            _builder.CreateEvent();
            /*Element.AppendToThread(() =>
            {
                CreatureInterfaceForSkill.GetAnimManager().Killed(); // IsTag("Die") == true
                // CreatureInterface.GetCreatureAttr().MindState = EnumMindState.Dead;
                CreatureInterfaceForSkill.GetCreatureAttr().Alive = false;
            });*/
        }

        public void Revival()
        {
            _isKilled = false;
            /*Element.AppendToThread(() =>
            {
                CreatureInterfaceForSkill.GetAnimManager().Revival(); // IsTag("Die") == false
                // CreatureInterface.GetCreatureAttr().MindState = EnumMindState.Idle;
                CreatureInterfaceForSkill.GetCreatureAttr().Alive = true;
            });*/
        }
        public void Enter()
        {
            Debug.Log("VAR");
            if (_isKilled)
            {
                CreatureInterface.GetAttr().Alive = false;
                CreatureInterface.GetAnim().Interrupt();
                CreatureInterface.GetAnim().Killed(); // IsTag("Die") == true
                // CreatureInterface.MindState = EnumMindState.Dead;
            }
        }

        public void Exit()
        {
        }

        public Func<bool> FilterIn { get; }
        public Func<bool> ToInterrupt { get; }
        public CreatureInterface CreatureInterface { get; set; }
    }
    /*public class LifeEvent : AbstractCreatureEventC
    {
        public LifeEvent(Creature creature) : base(creature)
        {
            FinalEvent += () => CreatureInterfaceForSkill.GetAnimManager().Interrupt();// 加速復活和死亡的流程
            InitCreatureEventOrder(EnumCreatureEventTag.Life, EnumOrder.Life);
        }
        public void Killed()
        {
            Element.AppendToThread(() =>
            {
                CreatureInterfaceForSkill.GetAnimManager().Killed(); // IsTag("Die") == true
                // CreatureInterface.GetCreatureAttr().MindState = EnumMindState.Dead;
                CreatureInterfaceForSkill.GetCreatureAttr().Alive = false;
            });
        }

        public void Revival()
        {
            Element.AppendToThread(() =>
            {
                CreatureInterfaceForSkill.GetAnimManager().Revival(); // IsTag("Die") == false
                // CreatureInterface.GetCreatureAttr().MindState = EnumMindState.Idle;
                CreatureInterfaceForSkill.GetCreatureAttr().Alive = true;
            });
        }
        protected override void Enter()
        {
        }

        protected override void Exit()
        {
        }
    }*/
}