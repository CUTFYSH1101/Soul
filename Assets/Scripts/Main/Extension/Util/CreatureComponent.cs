namespace Main.Extension.Util
{
    /*// 代理模式
    public class CreatureComponent : Creature
    {
        private readonly Creature subject;
        public Creature GetTarget() => subject.GetCreatureAI().GetAIState().GetTarget();
        public IAIState GetAIState() => subject.GetCreatureAI().GetAIState();
        public ICreatureAttr GetCreatureAttr() => subject.GetCreatureAttr();
        public ICreatureAI GetCreatureAI() => subject.GetCreatureAI();
        public Rigidbody2D GetRigidbody2D() => subject.GetRigidbody2D();
        public CreatureAnimManager GetAnimManager() => subject.GetAnimator();

        protected internal CreatureComponent(ICreatureAttr creatureAttr, Transform transform) : base(creatureAttr, transform)
        {
        }
    }*/
}