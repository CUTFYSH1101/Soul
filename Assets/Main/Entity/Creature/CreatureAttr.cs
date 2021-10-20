using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Main.Util;
using Main.EventLib.Common;
using Main.EventLib.Sub.CreatureEvent.Skill.Common;
using UnityEngine;

namespace Main.Entity.Creature
{
    [Serializable]
    public class CreatureAttr
    {
        // ======
        // 四、Other field
        // * 與一二必須無耦合關係
        // ======
        [Space(5), SerializeField, NotNull] private MindData mindData = new();

        [SerializeField] private EnumSkillTag skillState = EnumSkillTag.None;

        // ======
        // 一、
        // * Debug
        // * 命名規範：xxx
        // ======
        [SerializeField, Description("角色名稱")] private string name = "";
        [SerializeField, Description("是否存活")] private bool alive = true;
        [SerializeField, Description("是否在地面上")] private bool grounded = false;
        [SerializeField, Description("本身具有移動的能力")] private bool movableCoeff;
        [SerializeField, Description("本身具有攻擊的能力")] private bool attackableCoeff;

        [SerializeField, Description("擊退無效的能力，怎麼打都不會擊飛")]
        private bool preventKnockbackCoeff;

        [SerializeField, Description("受擊無效的能力，怎麼打都不會死")]
        private bool preventHitCoeff;

        [SerializeField, Description("滯空是否可以也可以跳躍的能力")]
        private bool enableAirControl = false;

        [SerializeField, Description("跳躍力")] private float jumpForce;
        [SerializeField, Description("移動速度")] private float moveSpeed;
        [SerializeField, Description("衝刺力")] private float dashForce;
        [SerializeField, Description("俯衝力")] private float diveForce;
        [SerializeField, Description("攻擊后座力")] private float recoilForce;

        [SerializeField, Description("攻擊速度。值越大，動畫播放越快，cd越短。為n倍原速")]
        private float attackSpeed = 1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moveSpeed">移動速度</param>
        /// <param name="jumpForce">跳躍力</param>
        /// <param name="dashForce">衝刺力</param>
        /// <param name="diveForce">俯衝力</param>
        /// <param name="recoilForce">攻擊后座力</param>
        /// <param name="attackSpeed">攻擊速度</param>
        /// <returns></returns>
        public CreatureAttr SetSpeedAndForce(
            float moveSpeed = 5, float jumpForce = 30,
            float dashForce = 20, float diveForce = 60,
            float recoilForce = 40, float attackSpeed = 1)
        {
            this.moveSpeed = moveSpeed;
            this.jumpForce = jumpForce;
            this.dashForce = dashForce;
            this.diveForce = diveForce;
            this.recoilForce = recoilForce; // 攻擊后座力
            this.attackSpeed = attackSpeed; // 攻速
            return this;
        }

        public CreatureAttr InitAbilityCoeff(
            bool alive = true,
            bool attackableCoeff = true, bool movable = true,
            bool preventKnockbackCoeff = false, bool preventHitCoeff = false)
        {
            _aliveItem = new BoolItem(alive);
            _enableAttackItem = new BoolItem(attackableCoeff);
            _enableMoveItem = new BoolItem(movable);
            _preventKnockbackItem = new BoolItem(preventKnockbackCoeff);
            _preventHitItem = new BoolItem(preventHitCoeff);
            return this;
        }

        // ======
        // 二、
        // * 相依於一
        // * 如果有宣告同名+Item物件，代表可以設定它的初始值和控制值，反之則為一般field
        // * 命名規範：xxxItem
        // ======
        private BoolItem _aliveItem;
        private BoolItem _preventKnockbackItem;
        private BoolItem _preventHitItem;
        private BoolItem _enableMoveItem;
        private BoolItem _enableAttackItem;

        private bool SetItem(ref BoolItem item2, bool value)
        {
            item2.Value = value; // 實際數值會根據初始值coeff是否為false而有不同
            return item2.Value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public string MaxHp { get; private set; }
        public string NowHp { get; private set; }

        // ======
        // 三-2、
        // * 內部更新
        // ======
        // isTag->mindState->Alive
        /// 固定數值，身體能力。eg:植物、boss
        public bool MovableCoeff => _enableMoveItem.Coeff;

        /// 動態，因為狀態而有所調整，條件更苛刻
        [Description("是否允許下一次移動")]
        public bool EnableMoveDyn
        {
            get => Alive && !CanNotMoving && _enableMoveItem.Value; // 活著+狀態參數+初始值
            // 詳見註釋一
            set => movableCoeff = SetItem(ref _enableMoveItem, value); // 更新顯示
        }

        public bool AttackableCoeff => _enableAttackItem.Coeff;

        /// 注意state模式請勿提前設定mindState==Attack，會導致CanNotControlled()觸發
        [Description("是否允許下一次攻擊")]
        public bool EnableAttackDyn
        {
            get => Alive && !CanNotControlled && _enableAttackItem.Value;
            set => attackableCoeff = SetItem(ref _enableAttackItem, value);
        }

        // ======
        // 三、
        // * 外部呼叫
        // ======
        public bool Alive
        {
            get => _aliveItem.Value;
            set
            {
                _aliveItem = new BoolItem(value);
                alive = _aliveItem.Value;
            }
        }

        public bool EnableAirControl
        {
            get => enableAirControl;
            set => enableAirControl = value;
        }

        public bool Grounded
        {
            get => grounded;
            set => grounded = value;
        }

        public float JumpForce
        {
            get => jumpForce;
            set => jumpForce = value;
        }

        public float MoveSpeed
        {
            get => moveSpeed;
            set => moveSpeed = value;
        }

        public float DashForce => dashForce;
        public float DiveForce => diveForce;
        public float RecoilForce => recoilForce;

        public float AttackSpeed
        {
            get => attackSpeed;
            set => attackSpeed = value;
            // 配點
        }

        public EnumMindState MindState
        {
            get => mindData.MindState;
            set => mindData.MindState = value;
        }

        /// default:None
        public EnumSkillTag CurrentSkill
        {
            get => skillState;
            set => skillState = value;
        }

        public void AppendDeBuff(EnumDebuff newDebuff)
            => mindData.AppendDeBuff(newDebuff);

        public void RemoveDeBuff(EnumDebuff debuff)
            => mindData.RemoveDeBuff(debuff);

        public EnumDebuff[] CurrentDeBuffs => mindData.CurrentDeBuffs;

        [Description("包括不能攻擊、不能移動，但可以被擊退、添加buff/debuff、加減血量")]
        public bool CanNotControlled => mindData.CanNotControlled || UnderQte;

        [Description("同上，但可以攻擊")] public bool CanNotMoving => mindData.CanNotMoving || UnderQte;

        public bool DuringDeBuff => mindData.DuringDeBuff;
        
        public bool UnderQte { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alive"></param>
        /// <param name="moveSpeed"></param>
        /// <param name="jumpForce"></param>
        /// <param name="dashForce"></param>
        /// <param name="diveForce"></param>
        /// <param name="recoilForce"></param>
        /// <param name="attackableCoeff">本身是否具有攻擊的能力</param>
        /// <param name="attackSpeed"></param>
        /// <param name="enableAirControl">允許空中跳躍</param>
        /// <param name="movableCoeff">本身是否具有移動的能力</param>
        /// <param name="preventKnockbackCoeff">常駐免疫擊退</param>
        /// <param name="preventHitCoeff">常駐免疫攻擊</param>
        public CreatureAttr(
            bool alive = true,
            float moveSpeed = 5, float jumpForce = 30, float dashForce = 20, float diveForce = 60,
            float recoilForce = 40,
            bool attackableCoeff = true, float attackSpeed = 1,
            bool enableAirControl = false,
            bool movableCoeff = true, bool preventKnockbackCoeff = false, bool preventHitCoeff = false)
        {
            EnableAirControl = enableAirControl;
            SetSpeedAndForce(moveSpeed, jumpForce, dashForce, diveForce, recoilForce, attackSpeed);
            InitAbilityCoeff(alive, attackableCoeff, movableCoeff, preventKnockbackCoeff, preventHitCoeff);
        }

        /// 簡單回傳指定類型指定內容的join+select字串
        private string Join<T>(Func<FieldInfo, string> selector)
        {
            return string.Join("\n", Select<T>(selector));
        }

        private string[] Select<T>(Func<FieldInfo, string> selector)
        {
            return this.GetFieldInfos<T>()
                .Select(selector)
                .ToArray();
        }

        public override string ToString() =>
            $"{GetType().Name} {Name}：\n" +
            $"{MindState.ToString()}\n" +
            $"{CurrentSkill.ToString()}\n" +
            $"grounded: {(Grounded ? "Yes" : "No")}\n" +
            $"CanNotControl: {CanNotControlled}\n" +
            $"debuff: {CurrentDeBuffs.EnumArrayToString()}\n";
    }

    public class BoolItem: CoeffItem<bool>
    {
        public BoolItem(bool coeff) : base(coeff)
        {
        }

        public override bool Value
        {
            get => value;
            set
            {
                if (coeff)
                    this.value = value;
            }
        }
    }
}
/*
表示是否本身具有該種能力…應用場景例如植物、魔王等
 attackable 
 movable
 knockbackAble
 coeff
 const
表示在臨時情況下開關能力…應用場景例如debug模式、衝刺
 enableAttack
 enableMove
 enableKnockback
 dynamic
 */