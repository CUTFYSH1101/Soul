using Main.Util;

namespace Main.Entity.Controller
{
    public class AttackAIState : IAIState
    {
        public override void Update()
        {
            aiStrategy.AttackUpdate();
        }
    }
}