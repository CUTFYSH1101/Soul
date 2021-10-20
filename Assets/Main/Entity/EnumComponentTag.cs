using System.ComponentModel;

namespace Main.Entity
{
    public enum EnumComponentTag
    {
        None,
        [Description("角色轉向控制。會自行決定是否要轉向")] FlipChecker,
        [Description("地面確認控制。會自行決定是否要執行跳躍動畫")] GroundChecker,

        [Description("角色事件系統。依照事件的優先序執行事件，並中斷優先序較低的")]
        CreatureThreadSystem,
        [Description("角色狀態機")] CreatureState,
        [Description("角色控制器")] PlayerController,
        [Description("角色策略")] CreatureStrategy,
        [Description("輸入系統")] InputSystem,
        [Description("戰鬥系統")] BattleSpoilerSystem,
        [Description("物理碰撞自動更新系統")] PhysicsCollisionSystem,
        [Description("角色系統。控制角色的生成、給予介面讓BattleSystem能夠獲得角色資料")] CreatureSystem,
        [Description("場景管理系統")] SceneManagement,
        [Description("多執行緒")] Multithreading,
        [Description("血條")] Blood,
    }
}