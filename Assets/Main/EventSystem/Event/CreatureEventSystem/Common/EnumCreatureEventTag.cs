using System.ComponentModel;

namespace Main.EventSystem.Event.CreatureEventSystem.Common
{
    /// 辨識用
    public enum EnumCreatureEventTag
    {
        [Description("類似null")] None,
        NormalAttack,
        SpurAttack,
        JumpAttack,
        DiveAttack,
        Life,
        Knockback,
        DeBuff,
        Dash,
        Move,
        Jump,
    }
}