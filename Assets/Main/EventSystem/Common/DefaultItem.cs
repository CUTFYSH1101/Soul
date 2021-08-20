using System;
using UnityEngine;

namespace Main.EventSystem.Common
{
    /// <code>
    /// 包裹器。可以自定義預設值（default），並在一般情況下設定value為default。泛型的包裹器。
    /// 注意：unity inspector不接受除了ListT以外的泛型類別顯示，故無法顯示
    /// </code>
    [Serializable]
    public abstract class DefaultItem<T>
    {
        [SerializeField] protected T value;
        protected T Default;

        /// 設定值
        public void SetValue(T newValue) => value = newValue;
        /// 設定值。當為真，設定為新數值，否則Reset
        /// <code>
        /// if true, make "value" equals "newValue",
        /// else, make it equals default.(Reset function)
        /// </code>
        public void SetValue(bool controlled, T newValue) => value = controlled ? newValue : Default;

        /// 重新設定default（初始值）
        public void SetDefault(T value) => Default = value;
        /// 還原為初始值
        public void Reset() => value = Default;

        public T GetValue() => value;
        public T GetDefault() => Default;

        protected DefaultItem(T @default)
        {
            Default = @default;
            value = @default;
        }
    }
}