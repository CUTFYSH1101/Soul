namespace Main.CreatureAI
{
    public interface ICreatureStrategy
    {
        void IdleEnter();
        void IdleUpdate();
        void AwareEnter();
        void AwareUpdate();
        void AttackEnter();
        void AttackUpdate();
        void ChaseEnter();
        void ChaseUpdate();

        public CreatureState CurrentState { get; }
        public void ChangeState(EnumCreatureStateTag newState);
    }
}