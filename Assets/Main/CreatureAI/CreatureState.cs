using System;
using JetBrains.Annotations;
using Main.Entity;
using Main.Entity.Creature;

namespace Main.CreatureAI
{
    public class CreatureState : IComponent
    {
        private readonly (Action enter, Action update) _actions;
        public EnumComponentTag Tag => EnumComponentTag.CreatureState;
        public EnumCreatureStateTag State { get; }
        public Creature Target { get; set; } = null;

        /// changeState
        public CreatureState(EnumCreatureStateTag state, [NotNull] ICreatureStrategy strategy)
        {
            State = state;
            _actions = Convert(state, strategy);
        }

        private static (Action enter, Action update) Convert(EnumCreatureStateTag state,
            [NotNull] ICreatureStrategy strategy) =>
            state switch
            {
                EnumCreatureStateTag.Idle => (strategy.IdleEnter, strategy.IdleUpdate),
                EnumCreatureStateTag.Aware => (strategy.AwareEnter, strategy.AwareUpdate),
                EnumCreatureStateTag.Attack => (strategy.AttackEnter, strategy.AttackUpdate),
                EnumCreatureStateTag.Chase => (strategy.ChaseEnter, strategy.ChaseUpdate),
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };

        public void Enter() => _actions.enter?.Invoke();

        public void Update() => _actions.update?.Invoke();
    }
}