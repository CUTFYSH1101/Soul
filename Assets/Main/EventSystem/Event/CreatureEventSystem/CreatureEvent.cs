using System;
using JetBrains.Annotations;
using Main.Entity.Creature;
using Main.EventSystem.Event.CreatureEventSystem.Common;

namespace Main.EventSystem.Event.CreatureEventSystem
{
    public class CreatureEvent : ICreatureEvent
    {
        public EnumCreatureEventTag Tag { get;private set; }
        public EnumOrder Order { get; private set; }

        // 事件的開關允許條件
        private bool _switch;

        public bool Switch
        {
            get => _switch;
            set
            {
                _switch = value;
                if (value) _allowEnter();
                else _interruptCmd();
            }
        }

        [NotNull] private Action _interruptCmd, _allowEnter;
        public Action InvokeEvent { get; private set; }
        public Func<bool> Finished { get; private set; }
        private readonly AbstractCreature _creature;

        public void AppendToThread(Action delegateEvent)
        {
            InvokeEvent += delegateEvent;
            _creature.ThreadSystem.AppendThread(this);
        }

        // ReSharper disable once NotNullMemberIsNotInitialized
        public CreatureEvent(AbstractCreature creature) => _creature = creature;

        public void InitEvent(EnumCreatureEventTag tag, EnumOrder order, Action interruptCmd, Action allowEnter, Func<bool> finished)
        {
            Tag = tag;
            Order = order;
            _interruptCmd = interruptCmd;
            _allowEnter = allowEnter;
            Finished = finished;
        }
        public void InitEvent(EnumCreatureEventTag tag, EnumOrder order, AbstractEvent @event) =>
            InitEvent(tag, order, () =>  @event.Switch = false, () =>  @event.Switch = true,
                () => @event.Finished);
    }
}