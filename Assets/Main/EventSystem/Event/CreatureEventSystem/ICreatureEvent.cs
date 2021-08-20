using System;
using Main.EventSystem.Event.CreatureEventSystem.Common;

namespace Main.EventSystem.Event.CreatureEventSystem
{
    public interface ICreatureEvent
    {
        /// 供辨識用，判斷同一種招式不可重入
        EnumCreatureEventTag Tag { get; }
        EnumOrder Order { get; }
        /// false代表中斷函式，true代表允許再次執行(需要呼叫InvokeEvent())
        bool Switch { get; set; }
        Action InvokeEvent { get; }
        Func<bool> Finished { get; }
    }
}