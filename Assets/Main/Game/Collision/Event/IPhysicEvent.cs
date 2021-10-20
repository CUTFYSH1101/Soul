using UnityEngine;

namespace Main.Game.Collision.Event
{
    /// <summary>
    /// 為了節省因getComponent而浪費的效能(重複獲取obj.collider2D.size)。
    /// 由manager改為使用
    /// 實例化的Event事件去處理
    /// (舊:manager->新:Event事件)
    /// </summary>
    public interface IPhysicEvent
    {
        bool IsTrigger { get; }
        Collider2D[] Others { get; }
    }
}