namespace Main.Entity
{
    // 不支援自動導航/自動戰鬥，一旦初始結束，則角色行為終身固定
    public abstract class IAIStrategy
    {
        public abstract void IdleUpdate();
        public abstract void AttackUpdate();
    }
}