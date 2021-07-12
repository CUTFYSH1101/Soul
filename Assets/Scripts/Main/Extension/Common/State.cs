using System;
using System.ComponentModel;
using UnityEngine;

namespace Main.Common
{
    public enum QTEState
    {
        Enter,
        Processing,
        Exit
    }

    /// 角色心智狀態
    public enum MindState
    {
        [Description("閒置")] Idle,
        [Description("戰鬥狀態")] Aware,
        [Description("移動")] Move,
        [Description("攻擊")] Attack,
        [Description("僵直，持續0.5秒")] Stiff,
        [Description("暈眩，眼冒金星，持續1秒")] Dizzy,
        [Description("昏迷，持續3秒")] Stun,
        [Description("衝刺")] Dash,
        [Description("格檔")] Parry,
        [Description("死亡")] Dead,
    }

    /// 角色環境狀態。
    public enum SurroundingState
    {
        Air,
        Grounded
    }

    /// 角色心智狀態。MindState的包裹器
    [Serializable]
    public class MindStateItem : DefaultItem<MindState>
    {
        /// 允許設定初始值
        public MindStateItem(MindState @default = default) : base(@default)
        {
        }

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
                case MindState.Attack:
                    return true;
                case MindState.Stiff:
                    return true;
                case MindState.Dizzy:
                    return true;
                case MindState.Stun:
                    return true;
                case MindState.Parry:
                    return true;
                case MindState.Dead:
                    return true;
                case MindState.Dash:
                    return true;
                // case MindState.Air:
                //     if (!EnableAirControl)
                //         return true;
                // break;
                case MindState.Idle:
                    break;
                case MindState.Aware:
                    break;
                case MindState.Move:
                    break;
                default:
                    Debug.LogError("超出範圍");
                    return false;
            }

            return false;
        }

        /// <summary>
        /// 不能接受dash、move等指令
        /// </summary>
        /// <returns></returns>
        public bool CanNotMoving()
        {
            return CanNotControlled() && value != MindState.Dash;
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
    public class SkillStateItem : DefaultItem<SkillName>
    {
        /// 允許設定初始值
        public SkillStateItem(SkillName @default = default) : base(@default)
        {
        }
    }
}