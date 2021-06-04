using System;
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
        Idle, // 閒置
        Aware, // 戰鬥狀態
        Move, // 移動
        Attack, // 攻擊
        Knockback, // 被擊退
        Stun, // 暈眩
        Dash, // 衝刺
        Parry, // 格檔
        Dead, // 死亡
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
        /// 回傳角色是否"不"可控制
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
                case MindState.Knockback:
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
}