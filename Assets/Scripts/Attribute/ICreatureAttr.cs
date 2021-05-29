using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Extension.Common;
using JetBrains.Annotations;
using Main.Entity;
using Main.Util;
using UnityEngine;
using Main.Entity.Controller;
using static System.Reflection.BindingFlags;

namespace Main.Entity.Attr
{
    [Serializable]
    public class ICreatureAttr
    {
        // ======
        // 四、Other field
        // * 與一二必須無耦合關係
        // ======
        [Space(5), SerializeField, NotNull] private MindStateItem mindState = new MindStateItem();

        [Space(5)] [SerializeField, NotNull]
        private SurroundingStateItem surroundingState = new SurroundingStateItem(SurroundingState.Grounded);

        // ======
        // 一、
        // * Debug
        // * 命名規範：xxx
        // ======
        [SerializeField] private string name = "";        
        [SerializeField] private bool alive = true;
        [SerializeField] private bool movable = true;
        [SerializeField] private bool attackable = true;
        [SerializeField] private bool enableAirControl = false;
        [SerializeField] private bool grounded = false;
        [SerializeField] private float jumpForce = 1800;
        [SerializeField] private float moveSpeed = 5;
        [SerializeField] private float sprintForce = 800;

        // ======
        // 二、
        // * 相依於一
        // * 如果有宣告同名+Item物件，代表可以設定它的初始值和控制值，反之則為一般field
        // * 命名規範：xxxItem
        // ======
        [Space(5)] private BoolItem aliveItem;
        private BoolItem movableItem;

        private BoolItem attackableItem;
        // private BoolItem enableAirControlItem;
        // private BoolItem groundedItem;

        public string GetName() => name;

        // ======
        // 三、
        // * 外部呼叫
        // ======
        public bool Alive
        {
            get => aliveItem.GetValue();
            set
            {
                aliveItem.SetValue(value);
                alive = aliveItem.GetValue();
                if (!aliveItem.GetValue()) mindState.SetValue(MindState.Dead);
                // mindState = aliveItem.GetValue() ? mindState : MindState.Dead;
            }
        }

        public bool Movable
        {
            get
            {
                Movable = Alive && !CanNotControlled(); // 詳見註釋一
                return movableItem.GetValue();
            }
            set
            {
                movableItem.SetValue(value);
                movable = movableItem.GetValue();
            }
        }

        public bool Attackable
        {
            get => attackableItem.GetValue();
            set
            {
                attackableItem.SetValue(value);
                attackable = attackableItem.GetValue();
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
                surroundingState.SetValue(!value, SurroundingState.Air);// 接地SurroundingState。當不在地面上時為Air，反之為Grounded（預設值）
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

        public float SprintForce
        {
            get => sprintForce;
            set => sprintForce = value;
        }
        
        public MindState MindState
        {
            get => mindState.GetValue();
            set => mindState.SetValue(value);
        }
        [NotNull]
        public SurroundingState SurroundingState
        {
            get => surroundingState.GetValue();
            set => surroundingState.SetValue(value);
        }

        public bool CanNotControlled() => mindState.CanNotControlled();

        public ICreatureAttr(
            bool alive = true, bool movable = true, bool attackable = true,
            bool enableAirControl = false, bool grounded = false,
            float jumpForce = 600, float moveSpeed = 5)
        {
            this.alive = alive;
            this.movable = movable;
            this.attackable = attackable;
            this.enableAirControl = enableAirControl;
            this.grounded = grounded;
            this.jumpForce = jumpForce;
            this.moveSpeed = moveSpeed;
            Init();
        }


        // OnValidate Init();
        public void Init()
        {
            /*this.aliveItem = new BoolItem() {@default = alive};
            this.movableItem = new BoolItem() {@default = movable};
            this.attackableItem = new BoolItem() {@default = attackable};
            this.enableAirControlItem = new BoolItem() {@default = enableAirControl};
            this.groundedItem = new BoolItem() {@default = grounded};*/
            /*
             * {0}.setValue({1}.getValue);
             * {0} = Inspector's value
             * {1} = @default
             */
            foreach (var info in this.GetFieldInfos<bool>())
            {
                this
                    .GetFieldInfo($"{info.Name}Item")
                    ?.SetValue(this, new BoolItem((bool) info.GetValue(this)));
            }
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
        public void SetValue(bool newValue) => base.SetValue(newValue, newValue);
    }
}

/*
註釋一：
注意不論getter或setter，都會通過預設值設定，因此在getter中使用的是大寫開頭"Movable"
簡化程式：
public bool Movable
{
    get
    {
        Movable = Alive && !CanNotControlled();
        return movableItem.GetValue();
    }
    set
    {
        movableItem.SetValue(value);
        movable = movableItem.GetValue();
    }
}
原式：
public bool Movable
{
    get
    {
        if (!Alive || CanNotControlled())
        {
            movable = false;
        }
        else
        {
            if (movableItem.@default)
            {
                movable = true;
            }
            else
            {
                movable = false;
            }
        }
        return movable;
    }
    set
    {
        if (movableItem.@default)
        {
            movable = value;
        }
        else
        {
            movable = false;
        }
    }
}
改良：
public bool Movable
{
    get
    {
        movableItem.value = Alive && !CanNotControlled() && movableItem.@default;
        movable = movableItem.value;
        return movable;
    }
    set
    {
        movableItem.value = movableItem.@default && value;
        movable = movableItem.value;
    }
}
*/