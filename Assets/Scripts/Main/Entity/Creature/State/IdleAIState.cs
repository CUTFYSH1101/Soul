namespace Main.Entity
{
    public class IdleAIState : IAIState
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public override void Update()
        {
            aiStrategy.IdleUpdate();
        }
    }
}