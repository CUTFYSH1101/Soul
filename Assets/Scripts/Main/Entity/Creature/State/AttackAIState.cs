namespace Main.Entity
{
    public class AttackAIState : IAIState
    {
        public override void Update()
        {
            aiStrategy.AttackUpdate();
        }
    }
}