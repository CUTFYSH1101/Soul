using System;
using Main.EventLib.Sub.CreatureEvent.Skill;

namespace Main.EventLib.Main.EventSystem.Main.Interface
{
    /// PreWork->Action1->Action2....->FinalEvent->PostWork
    public interface IEvent : IInterruptible
    {
        public bool Finished => State == EnumState.Free;
        public EnumState State { get; set; }
        public Action PreWork { get; set; }
        public Action PostWork { get; set; }
        public Action FinalAct { get; set; }
        public EventAttr EventAttr { get; set; }
        // 同時設定SkillAttr，保持資料統一
        public void SetCd(float cd)
        {
            if (EventAttr == null) throw new Exception("EventAttr尚未實例化");
            if (this is ISkill skill) skill.SkillAttr.CdTime = cd;
            EventAttr.CdTime = cd;
            // Debug.Log("SkillAttr上的技能CD，已同步成功");
        }

        public void SetDuration(float duration)
        {
            if (EventAttr == null) throw new Exception("EventAttr尚未實例化");
            if (this is ISkill skill) skill.SkillAttr.Duration = duration;
            EventAttr.MaxDuration = duration;
        }
    }

    public static class EventExtensions
    {
        public static void SetCd(this IEvent e, float cd) => e.SetCd(cd);

        public static void SetDuration(this IEvent e, float duration) => e.SetDuration(duration);
    }
}