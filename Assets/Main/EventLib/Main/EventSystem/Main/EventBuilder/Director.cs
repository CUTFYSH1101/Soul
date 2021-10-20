using System;
using Main.EventLib.Main.EventSystem.Main.Interface;
using Main.EventLib.Main.EventSystem.Main.Sub.Builder;
using Main.EventLib.Main.EventSystem.Main.Sub.Interface;
using Main.EventLib.Sub.CreatureEvent.Skill;
using Main.EventLib.Sub.CreatureEvent.Skill.Attribute;

namespace Main.EventLib.Main.EventSystem.Main.EventBuilder
{
    public class Director : IEventDirector
    {
        public AbsBuilder Builder { get; }

        public Director(IEvent @event)
        {
            Builder = @event switch
            {
                IEvent1 e => new Event1Builder(e),
                IEvent2 e => new Event2Builder(e),
                IEvent3 e => new Event3Builder(e),
                IEvent4 e => new Event4Builder(e),
                _ => throw new ArgumentOutOfRangeException(nameof(@event))
            };

            Builder.Event.EventAttr ??= new EventAttr();
            /*if (Event is IWorkOnCreature workOnCreature)
                workOnCreature.CreatureInterfaceForSkill = new CreatureInterfaceForSkill(creature);// 幫忙初始化creature介面*/
            if (@event is ISkill { SkillAttr: null } skill)
                skill.SkillAttr = new SkillAttr();
            if (@event is AbsEventObject absEventObject) absEventObject.Director = this;
        }

        public void CreateEvent()
        {
            if (!Builder.PreCreateCheck()) return;
            Builder.CreateEvent();
        }

        public void InterruptAndClose() => Builder.InterruptAndClose();
    }
}