using System;
using UnityEngine;

namespace Main.EventLib.Common
{
    /*
    /// <code>
    /// 包裹器。可以自定義預設值（default），並在一般情況下設定value為default。泛型的包裹器。
    /// 注意：unity inspector不接受除了ListT以外的泛型類別顯示，故無法顯示
    /// </code>
    */
    /// <summary>
    /// 包含兩個數值
    /// default
    /// value
    /// 設定value時必須注意絕不可違反default
    /// 例如
    /// T : bool
    /// default = false
    /// value = true
    /// 則getValue = false
    /// default = true
    /// value = false
    /// 則getValue = false
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class DefaultItem<T>
    {
        [SerializeField] protected T value;
        [SerializeField] protected T @default;

        /// 設定值
        public void SetValue(T newValue) => value = newValue;

        /// 設定值。當為真，設定為新數值，否則Reset
        /// <code>
        /// if true, make "value" equals "newValue",
        /// else, make it equals default.(Reset function)
        /// </code>
        public void SetValue(bool controlled, T newValue) => value = controlled ? newValue : @default;

        /// 重新設定default（初始值）
        public void SetDefault(T value) => @default = value;

        /// 還原為初始值
        public void Reset() => value = @default;

        public T GetValue() => value;
        public T GetDefault() => @default;

        protected DefaultItem(T @default)
        {
            this.@default = @default;
            value = @default;
        }
    }
    [Serializable]
    public abstract class CoeffItem<T>
    {
        [SerializeField] protected T value;
        [SerializeField] protected T coeff;

        protected CoeffItem(T coeff)
        {
            this.coeff = coeff;
            this.value = coeff;
        }

        public abstract T Value { get; set; }
        public T Coeff
        {
            get => coeff;
            set => coeff = value;
        }
    }
}