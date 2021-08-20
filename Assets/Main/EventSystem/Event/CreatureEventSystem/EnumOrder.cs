using UnityEngine;

namespace Main.EventSystem.Event.CreatureEventSystem
{
    /// <summary>
    /// 執行順序，數值越小排越前面
    /// </summary>
    public enum EnumOrder
    {
        [Tooltip("1st,死亡")]
        Life = 0,
        [Tooltip("2st,進入負面狀態，目前所有的負面狀態都會使角色無法控制")]
        DeBuff = 1,
        [Tooltip("2st,擊退效果")] 
        Knockback = 1,
        [Tooltip("3st,主動+被動攻擊技能")] 
        Attack = 2,
        [Tooltip("4st,衝刺，優先度高於Move")] 
        Dash = 3,
        [Tooltip("5st,除非其他事件都執行完畢，否則不執行")] 
        Move = 4
    }
}