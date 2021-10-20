using System;
using System.ComponentModel;
using Main.EventLib.Sub.CreatureEvent.Skill.Common;
using UnityEngine;

namespace Main.EventLib.Common
{
    [Serializable]
    public class MindData
    {
        // debuff
        [SerializeField] private DeBuffList deBuff = new();
        public EnumDebuff[] CurrentDeBuffs => deBuff.GetCurrentDeBuffs();
        public bool DuringDeBuff => deBuff.DuringDeBuff;

        public void AppendDeBuff(EnumDebuff newDebuff)
            => deBuff.Append(newDebuff);

        public void RemoveDeBuff(EnumDebuff debuff)
            => this.deBuff.Remove(debuff);

        // mindState
        [SerializeField] private MindStateList mindState = new();

        public EnumMindState MindState
        {
            get => mindState.Value;
            set => mindState.Value = value;
        }

        [Description("包括不能攻擊、不能移動，但可以被擊退、添加buff/debuff、加減血量")]
        public bool CanNotControlled => mindState.CanNotControlled() || DuringDeBuff;

        [Description("同上，但可以攻擊")] public bool CanNotMoving => mindState.CanNotMoving() || DuringDeBuff;

        public MindData(EnumMindState mindState = EnumMindState.Idle) =>
            this.mindState.Value = mindState;
    }

    /// 包裹MindState，判斷哪些狀態無法執行哪些行為
    [Serializable]
    public class MindStateList
    {
        public EnumMindState Value { get; set; }

        /// 允許設定初始值
        public MindStateList(EnumMindState value = default) => Value = value;

        /// <code>
        /// 1.各種"途中"不能控制
        ///     攻擊途中、格檔途中、衝刺途中
        /// 2.因為某些狀態而不能控制
        ///     死亡、QTE
        /// </code>
        public bool CanNotControlled()
        {
            switch (Value)
            {
                /*
                 * 死亡、負面狀態下、無法控制
                 * 跳躍、衝刺中途，視情況無法控制
                 */
                // case EnumMindState.Dead:
                case EnumMindState.UnderQteEvent:
                case EnumMindState.Attacking:
                case EnumMindState.Parry:
                case EnumMindState.Dashing:
                    return true;
                case EnumMindState.Idle:
                case EnumMindState.Aware:
                case EnumMindState.Moving:
                    return false;

                default:
                    Debug.LogError("超出範圍");
                    return false;
            }
        }

        /// 和CanNotControlled的差別是：
        ///  衝刺中和移動中，都可以進行攻擊；
        ///  但是衝刺中再按左右鍵，不能再移動
        public bool CanNotMoving() =>
            CanNotControlled() &&
            Value != EnumMindState.Dashing;
    }

    /*
    /// 角色環境狀態。SurroundingState的包裹器
    [Serializable]
    public class SurroundingStateItem : DefaultItem<SurroundingState>
    {
        /// 允許設定初始值
        public SurroundingStateItem(SurroundingState @default = default) : base(@default)
        {
        }
    }
    */

    [Serializable]
    public class SkillStateItem : DefaultItem<EnumSkillTag>
    {
        /// 允許設定初始值
        public SkillStateItem(EnumSkillTag @default = default) : base(@default)
        {
        }
    }
}