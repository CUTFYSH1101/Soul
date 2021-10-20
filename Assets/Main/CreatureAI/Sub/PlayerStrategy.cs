using Main.CreatureBehavior.Behavior.Sub;
using Main.Entity;

namespace Main.CreatureAI.Sub
{
    public class PlayerStrategy : ICreatureStrategy, IComponent
    {
        private readonly IPlayerController _playerController;

        public PlayerStrategy(IPlayerController playerController)
        {
            _playerController = playerController;
            ChangeState(EnumCreatureStateTag.Aware); // init state
        }

        public void IdleEnter()
        {
            _playerController.EnableAttack = false;
        }

        public void IdleUpdate()
        {
            // on trigger: toAwareState
        }

        public void AwareEnter()
        {
            _playerController.EnableAttack = true;
        }

        public void AwareUpdate()
        {
            // if on trigger: toIdleState
        }

        public void AttackEnter()
        {
        }

        public void AttackUpdate()
        {
        }

        public void ChaseEnter()
        {
        }

        public void ChaseUpdate()
        {
        }

        public EnumComponentTag Tag => EnumComponentTag.CreatureStrategy;

        public CreatureState CurrentState { get; private set; }

        // creature.update => (IComponent) strategy.update (外露方法) => state.update => strategy.xxxUpdate
        public void Update() => CurrentState?.Update();

        // creature.update => (IComponent) strategy.update (外露方法) => state.update => strategy.xxxUpdate =>
        // if onTrigger => ChangeState(newState)
        public void ChangeState(EnumCreatureStateTag newState)
        {
            CurrentState = new CreatureState(newState, this);
            CurrentState.Enter();
        }
    }
}