using System;
using JetBrains.Annotations;
using Main.Entity.Creature;
using Main.EventLib.Main.EventSystem.Main.Interface;
using Main.EventLib.Sub.CreatureEvent;

namespace Main.EventLib.Main.EventSystem.EventOrderbySystem
{
    public class Element : IElement
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
                if (value) _openPermissionCmd();
                else _interruptAndClosePermissionCmd();
            }
        }

        [NotNull] private Action _interruptAndClosePermissionCmd, _openPermissionCmd;
        public Action InvokeEvent { get; private set; }
        public Func<bool> Finished { get; private set; }
        private readonly Creature _creature;

        public void AppendToThread(Action delegateEvent)
        {
            InvokeEvent += delegateEvent;
            _creature.ThreadSystem.AppendEventElement(this);
        }

        // ReSharper disable once NotNullMemberIsNotInitialized
        public Element(Creature creature) => _creature = creature;
        
        public void InitEvent(EnumCreatureEventTag tag, EnumOrder order, IEvent @event)
        {
            Tag = tag;
            Order = order;
            _interruptAndClosePermissionCmd = () => @event.IsOpen = false;
            _openPermissionCmd = () => @event.IsOpen = true;
            Finished = () => @event.Finished;
        }
        #region InitEvent Method2
        /*public void InitEvent(EnumCreatureEventTag tag, EnumOrder order, Action interruptCmd, Action allowEnter, Func<bool> finished)
        {
            Tag = tag;
            Order = order;
            _interruptCmd = interruptCmd;
            _allowEnter = allowEnter;
            Finished = finished;
        }
        public void InitEvent(EnumCreatureEventTag tag, EnumOrder order, IEvent @event) =>
            InitEvent(tag, order, () =>  @event.Switch = false, () =>  @event.Switch = true,
                () => @event.Finished);*/
        #endregion
    }
}