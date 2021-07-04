namespace Main.Entity
{
    public class PlayerStrategy : IAIStrategy
    {
        private readonly AbstractCreatureAI abstractCreatureAI;
        private readonly PlayerController controller;

        public PlayerStrategy(AbstractCreatureAI abstractCreatureAI)
        {
            this.abstractCreatureAI = abstractCreatureAI;
            controller = new PlayerController(abstractCreatureAI.GetCreature());
            (abstractCreatureAI as PlayerAI)?.SetController(controller);
        }

        public override void IdleUpdate()
        {
            controller.Update();
        }

        public override void AttackUpdate()
        {
        }
    }
}