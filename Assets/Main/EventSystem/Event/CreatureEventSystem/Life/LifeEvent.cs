using Main.Entity.Creature;
using Main.EventSystem.Common;
using Main.EventSystem.Event.CreatureEventSystem.Common;
using Main.EventSystem.Event.CreatureEventSystem.Decorator;

namespace Main.EventSystem.Event.CreatureEventSystem.Life
{
    
    public class LifeEvent : AbstractCreatureEventC
    {
        public LifeEvent(Creature creature) : base(creature)
        {
            FinalEvent += () => CreatureInterface.GetAnimManager().Interrupt();// 加速復活和死亡的流程
            InitCreatureEventOrder(EnumCreatureEventTag.Life, EnumOrder.Life);
        }
        public void Killed()
        {
            CreatureEvent.AppendToThread(() =>
            {
                CreatureInterface.GetAnimManager().Killed(); // IsTag("Die") == true
                CreatureInterface.GetCreatureAttr().MindState = EnumMindState.Dead;
                CreatureInterface.GetCreatureAttr().Alive = false;
            });
        }

        public void Revival()
        {
            CreatureEvent.AppendToThread(() =>
            {
                CreatureInterface.GetAnimManager().Revival(); // IsTag("Die") == false
                CreatureInterface.GetCreatureAttr().MindState = EnumMindState.Idle;
                CreatureInterface.GetCreatureAttr().Alive = true;
            });
        }
        protected override void Enter()
        {
        }

        protected override void Exit()
        {
        }
    }
}