using System;
using Main.EventLib.Main.EventSystem.Main.EventBuilder;

namespace Main.EventLib.Main.EventSystem.Main.Interface
{
    public abstract class AbsEventObject : IEvent
    {
        private bool _isOpen = true;// *記得預設要打開，否則您可能很納悶為何永遠觸發不了事件

        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                _isOpen = value;
                if (value == false) Director?.InterruptAndClose();
            }
        }

        public EnumState State { get; set; } = EnumState.Free;
        public Action PreWork { get; set; }
        public Action PostWork { get; set; }
        public virtual Action FinalAct { get; set; }
        public EventAttr EventAttr { get; set; } = new();
        /// 需要實例化 [NotNull]
        public IEventDirector Director { get; set; }
    }
}