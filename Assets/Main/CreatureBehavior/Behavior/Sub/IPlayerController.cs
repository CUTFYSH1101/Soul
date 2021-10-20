using Main.Entity;

namespace Main.CreatureBehavior.Behavior.Sub
{
    public interface IPlayerController : IComponent
    {
        bool EnableAttack { get; set; }
    }
}