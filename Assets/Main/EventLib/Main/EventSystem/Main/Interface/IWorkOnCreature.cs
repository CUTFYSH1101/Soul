using JetBrains.Annotations;
using Main.Entity.Creature;
using Main.EventLib.Sub.CreatureEvent.Skill;

namespace Main.EventLib.Main.EventSystem.Main.Interface
{
    public interface IWorkOnCreature
    {
        [NotNull] public CreatureInterface CreatureInterface { get; set; }
        public void Init(Creature creature) => CreatureInterface = new CreatureInterface(creature);
    }
}