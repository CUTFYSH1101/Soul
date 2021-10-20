using System.ComponentModel;

namespace Main.EventLib.Main.EventSystem.EventOrderbySystem
{
    /// <summary>
    /// 執行順序，數值越小排越前面
    /// </summary>
    public enum EnumOrder
    {
        [Description("1st,死亡")]
        Life = 0,
        [Description("2st,進入負面狀態，目前所有的負面狀態都會使角色無法控制")]
        DeBuff = 1,
        [Description("2st,擊退效果")] 
        Knockback = 1,
        [Description("3st,主動+被動攻擊技能")] 
        Attack = 2,
        [Description("4st,衝刺，優先度高於Move")] 
        Dash = 3,
        [Description("5st,除非其他事件都執行完畢，否則不執行")] 
        Move = 4
    }
}