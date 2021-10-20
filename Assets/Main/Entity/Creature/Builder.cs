namespace Main.Entity.Creature
{
    public class Director
    {
        private readonly Creature _result;

        public Director(Builder builder)
        {
            _result = builder.Creature;
            // 預設值
            builder.Init(); // 初始化、角色屬性
            builder.SetName(); // 設定角色名稱
            builder.SetBattleSystem(); // 設定陣營與戰鬥系統
            builder.SetAISystem(); // 是否啟用AI系統，如果是，則設定AIState初始狀態
        }

        public Creature GetResult() =>
            _result;
    }

    public abstract class Builder
    {
        public Creature Creature { get; protected set; } // switch-case給予

        public bool Inited { get; protected set; }

        public abstract void Init();

        // public abstract void SetName();
        public virtual void SetName() => Creature.CreatureAttr.Name = Creature.Transform.name; // 設定成遊戲物件的名稱

        /// 是否啟用AI系統，如果是，則設定AIState初始狀態。
        /// 採用AI系統時，角色會自動切換狀態。
        public abstract void SetAISystem();

        /// 設定陣營與戰鬥系統。
        /// 設定攻擊對象。
        public abstract void SetBattleSystem();
    }
}