using Main.EventSystem.Event.CreatureEventSystem.Skill.Attribute;

namespace Main.EventSystem.Event.CreatureEventSystem.Skill
{
    /// 包含Skill的功能，支持更多cause, if-else支持
    public interface ISkill
    {
        /// 注意可能為空值
        SkillAttr SkillAttr { get; }

        CreatureInterface CreatureInterface { get; }

        /*
        protected ISkill(float cdTime = 0, float duration = 10) : base(new EventAttr(cdTime, duration))
        {
            // var cameraShaker = UnityRes.GetCameraShaker();
            // onEnter += cameraShaker.Invoke;
        }
    */
    }
}
/*
 * invoke skill時
 * 呼叫cameraShaking
 * 呼叫特定類啟用畫面icon cd時間
*/