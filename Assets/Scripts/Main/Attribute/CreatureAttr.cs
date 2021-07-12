using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Main.Common;
using Main.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Main.Attribute
{
    [Serializable]
    public class CreatureAttr
    {
        // ======
        // 四、Other field
        // * 與一二必須無耦合關係
        // ======
        [Space(5), SerializeField, NotNull] private MindStateItem mindState = new MindStateItem();

        [Space(5)] [SerializeField, NotNull]
        private SurroundingStateItem surroundingState = new SurroundingStateItem(SurroundingState.Grounded);

        // [Space(5)] [SerializeField, NotNull] private SkillStateItem skillState = new SkillStateItem(SkillName.None);
        [SerializeField] private SkillName skillState;

        // ======
        // 一、
        // * Debug
        // * 命名規範：xxx
        // ======
        [SerializeField] private string name = "";

        // [SerializeField] private bool alive = true;
        [FormerlySerializedAs("coeffMovable")]
        [FormerlySerializedAs("bodyMovable")]
        [FormerlySerializedAs("movable")]
        [SerializeField]
        private bool movableCoeff = true;

        [FormerlySerializedAs("coeffAttackable")] [FormerlySerializedAs("attackable")] [SerializeField]
        private bool attackableCoeff = true;

        [SerializeField] private bool enableAirControl = false;
        [SerializeField] private bool grounded = false;
        [SerializeField] private float jumpForce = 1800;
        [SerializeField] private float moveSpeed = 5;
        [SerializeField] private float dashForce = 40;
        [SerializeField] private float diveForce = 60;
        [SerializeField] private float recoilForce = 40;

        // ======
        // 二、
        // * 相依於一
        // * 如果有宣告同名+Item物件，代表可以設定它的初始值和控制值，反之則為一般field
        // * 命名規範：xxxItem
        // ======
        // [Space(5)] private BoolItem aliveItem;
        private BoolItem movableItem;

        private BoolItem attackableItem;
        // private BoolItem enableAirControlItem;
        // private BoolItem groundedItem;

        public string Name
        {
            get => name;
            set => name = value;
        }

        public string MaxHP { get; private set; }
        public string NowHP { get; private set; }

        // ======
        // 三、
        // * 外部呼叫
        // ======
        public bool Alive
        {
            get => MindState != MindState.Dead;
            set => MindState = value ? default : MindState.Dead;
        }

        // isTag->mindState->Alive
        /// 固定數值，身體能力。eg:植物、boss
        public bool MovableCoeff => movableCoeff;

        /// 動態，因為狀態而有所調整，條件更苛刻
        [Description("是否允許下一次移動")]
        public bool MovableDyn
        {
            get => Alive && !CanNotMoving() && movableCoeff;
            // 詳見註釋一
            set
            {
                movableItem.SetValue(value);
                movableCoeff = movableItem.GetValue();
            }
        }

        public bool AttackableCoeff => attackableCoeff;

        /// 注意state模式請勿提前設定mindState==Attack，會導致CanNotControlled()觸發
        [Description("是否允許下一次攻擊")]
        public bool AttackableDyn
        {
            get => Alive && attackableItem.GetValue() && !CanNotControlled() && attackableCoeff;
            set
            {
                attackableItem.SetValue(value);
                attackableCoeff = attackableItem.GetValue();
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
            set
            {
                grounded = value;
                surroundingState.SetValue(!value,
                    SurroundingState.Air); // 接地SurroundingState。當不在地面上時為Air，反之為Grounded（預設值）
            }
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

        public float DashForce
        {
            get => dashForce;
            set => dashForce = value;
        }

        public float DiveForce
        {
            get => diveForce;
            set => diveForce = value;
        }

        public float RecoilForce
        {
            get => recoilForce;
            set => recoilForce = value;
        }

        public MindState MindState
        {
            get => mindState.GetValue();
            set => mindState.SetValue(value);
        }

        public SurroundingState SurroundingState
        {
            get => surroundingState.GetValue();
            set => surroundingState.SetValue(value);
        }

        public SkillName SkillName
        {
            /*get => skillState.GetValue();
            set => skillState.SetValue(value);*/
            get => skillState;
            set => skillState = value;
        }

        public CreatureAttr(
            bool alive = true,
            bool movableCoeff = true, bool attackableCoeff = true,
            bool enableAirControl = false, bool grounded = false,
            float moveSpeed = 5, float jumpForce = 1800,
            float dashForce = 20, float diveForce = 50,
            float recoilForce = 40)
        {
            this.movableCoeff = movableCoeff;
            this.attackableCoeff = attackableCoeff;
            this.enableAirControl = enableAirControl;
            this.grounded = grounded;
            this.moveSpeed = moveSpeed;
            this.jumpForce = jumpForce;
            this.dashForce = dashForce;
            this.diveForce = diveForce;
            this.recoilForce = recoilForce; // 攻擊后座力
            Init();
            // this.alive = alive;
            if (!alive)
                mindState.SetValue(MindState.Dead);
        }

        [Description("包括不能攻擊、不能移動，但可以被擊退、添加buff/debuff、加減血量")]
        public bool CanNotControlled() => mindState.CanNotControlled();

        [Description("同上，但可以攻擊")]
        public bool CanNotMoving() => mindState.CanNotMoving();


        // OnValidate SetBehavior();
        public void Init()
        {
            /*this.aliveItem = new BoolItem() {@default = alive};
            this.movableItem = new BoolItem() {@default = movableCoeff};
            this.attackableItem = new BoolItem() {@default = attackableCoeff};
            this.enableAirControlItem = new BoolItem() {@default = enableAirControl};
            this.groundedItem = new BoolItem() {@default = grounded};*/
            /*
             * {0}.setValue({1}.getValue);
             * {0} = Inspector's value
             * {1} = @default
             */
            this.attackableItem = new BoolItem(attackableCoeff);
            this.movableItem = new BoolItem(movableCoeff);
            /*foreach (var info in this.GetFieldInfos<bool>())
            {
                var infoName = info.Name;
                if (infoName.Contains("Coeff"))
                    infoName = $"{infoName.Substring(0, infoName.Length - 5)}Item";
                Debug.Log(infoName);
                this
                    .GetFieldInfo(infoName)
                    ?.SetValue(this, new BoolItem((bool) info.GetValue(this)));
            }*/
        }

        public override string ToString()
        {
            var text = this.GetType().Name;
            text +=
                Join<bool>(temp =>
                    $"\n{temp.Name}: {temp.GetValue(this)}" +
                    (this.GetFieldInfo($"{temp.Name}Item").IsEmpty()
                        ? null
                        : $"\ndefault: {((BoolItem) this.GetFieldInfo($"{temp.Name}Item").GetValue(this)).GetDefault()}"
                    ));
            text += "\n";
            text +=
                Join<float>(temp =>
                    $"\n{temp.Name}: {temp.GetValue(this)}");
            return text;
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
    }

    /// 包裹器。可以自定義預設值（default），並在一般情況下設定value為default。bool的包裹器。
    [Serializable]
    public class BoolItem : DefaultItem<bool>
    {
        public BoolItem(bool @default) : base(@default)
        {
        }

        /// 使數值在newValue或@default之間切換
        public new void SetValue(bool newValue) => base.SetValue(true, newValue);
    }
}

/*
註釋一：
注意不論getter或setter，都會通過預設值設定，因此在getter中使用的是大寫開頭"MovableDyn"
簡化程式：
public bool MovableDyn
{
    get
    {
        MovableDyn = Alive && !CanNotControlled();
        return movableItem.GetValue();
    }
    set
    {
        movableItem.SetValue(value);
        movableCoeff = movableItem.GetValue();
    }
}
原式：
public bool MovableDyn
{
    get
    {
        if (!Alive || CanNotControlled())
        {
            movableCoeff = false;
        }
        else
        {
            if (movableItem.@default)
            {
                movableCoeff = true;
            }
            else
            {
                movableCoeff = false;
            }
        }
        return movableCoeff;
    }
    set
    {
        if (movableItem.@default)
        {
            movableCoeff = value;
        }
        else
        {
            movableCoeff = false;
        }
    }
}
改良：
public bool MovableDyn
{
    get
    {
        movableItem.value = Alive && !CanNotControlled() && movableItem.@default;
        movableCoeff = movableItem.value;
        return movableCoeff;
    }
    set
    {
        movableItem.value = movableItem.@default && value;
        movableCoeff = movableItem.value;
    }
}
*/