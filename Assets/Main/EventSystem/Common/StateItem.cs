using System;
using Main.EventSystem.Event.CreatureEventSystem.Skill.Common;
using UnityEngine;

namespace Main.EventSystem.Common
{
    /// 角色心智狀態。MindState的包裹器
    [Serializable]
    public class MindStateItem : DefaultItem<EnumMindState>
    {
        /// 允許設定初始值
        public MindStateItem(EnumMindState @default = default) : base(@default)
        {
        }

        private bool IsAttacking() => value == EnumMindState.Attack;

        /// 回傳角色是否"不"可控制。不能接受attack、move、hit等幾乎所有指令
        /// <code>
        /// 死亡、負面狀態下、無法控制
        /// 跳躍、衝刺中途，視情況無法控制
        /// </code>
        public bool CanNotControlled()
        {
            switch (value)
            {
                /*
                 * 死亡、負面狀態下、無法控制
                 * 跳躍、衝刺中途，視情況無法控制
                 */
                case EnumMindState.Attack:
                case EnumMindState.Parry:
                case EnumMindState.Dead:
                case EnumMindState.Dash:
                case EnumMindState.UnderQteEvent:
                    return true;
                case EnumMindState.Idle:
                case EnumMindState.Aware:
                case EnumMindState.Move:
                    return false;

                default:
                    Debug.LogError("超出範圍");
                    return false;
            }
        }

        /// <summary>
        /// 不能接受dash、move等指令
        /// </summary>
        /// <returns></returns>
        public bool CanNotMoving()
        {
            return CanNotControlled() && value != EnumMindState.Dash;
        }
    }

    /// 角色環境狀態。SurroundingState的包裹器
    [Serializable]
    public class SurroundingStateItem : DefaultItem<SurroundingState>
    {
        /// 允許設定初始值
        public SurroundingStateItem(SurroundingState @default = default) : base(@default)
        {
        }
    }

    [Serializable]
    public class SkillStateItem : DefaultItem<EnumSkillTag>
    {
        /// 允許設定初始值
        public SkillStateItem(EnumSkillTag @default = default) : base(@default)
        {
        }
    }
}