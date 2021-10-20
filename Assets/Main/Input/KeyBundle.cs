using System.Collections.Generic;
using System.Linq;
using Main.Input.DataType;
using UnityEngine;

namespace Main.Input
{
    /// 客戶端
    public class KeyBundle
    {
        private readonly List<IKey> _keys = new();
        public KeyBundle(IKey key) =>
            Add(key);

        public KeyBundle(IKey key1, IKey key2) =>
            Add(key1)
                .Add(key2);

        public KeyBundle(IKey key1, IKey key2, IKey key3, IKey key4) =>
            Add(key1)
                .Add(key2)
                .Add(key3)
                .Add(key4);

        /// 1P。1個鍵盤按鍵
        public KeyBundle(KeyCode key) =>
            this.Add(key);
        // Add(new ExtraKey(PorN.Positive, key));

        /// 1P。1個滑鼠
        public KeyBundle(int key) =>
            this.Add(key);
        // Add(new ExtraMouse(PorN.Positive, key));

        /// 2P。1鍵盤1滑鼠。JKL專屬，未來估計有GamePad加入。
        public KeyBundle(KeyCode key1, int key2) =>
            this.Add(key1).Add(key2);

        /// 2P+2N。鍵盤與滑鼠組合
        public KeyBundle(KeyCode key1, KeyCode key2, int key3, int key4) =>
            this.Add(key1, key2)
                .Add(key3, key4);

        /// 2P+2N。4個鍵盤按鍵
        public KeyBundle(KeyCode key1, KeyCode key2, KeyCode key3, KeyCode key4) =>
            this.Add(key1, key2)
                .Add(key3, key4);

        public KeyBundle Add(IKey key)
        {
            _keys.Add(key);
            return this;
        }

        public bool IsButton(DataType.Event @event)
            => _keys.Any(k => k.IsButton(@event));
        /// <remarks>
        /// 這個方法近似unity的GetAxisRaw。
        /// 
        /// </remarks>
        public int GetAxisRaw()
        {
            bool anyP = _keys.Any(k => k.GetAxisRaw() > 0);
            bool anyN = _keys.Any(k => k.GetAxisRaw() < 0);
            if (anyP)
                return 1;
            else if (anyN)
                return -1;
            else
                return 0;
            // return Math.Sign(_keys.Sum(k => k.GetAxisRaw()));
        }

        public int GetAxisRawDown()
        {
            bool anyP = _keys.Any(k => k.GetAxisRawDown() > 0);
            bool anyN = _keys.Any(k => k.GetAxisRawDown() < 0);
            if (anyP)
                return 1;
            else if (anyN)
                return -1;
            else
                return 0;
            // return Math.Sign(_keys.Sum(k => k.GetAxisRawDown()));
            // var anyP = _keys.Any(k => k.PorN == PorN.Positive && k.IsButton(Event.Down));
            // var anyN = _keys.Any(k => k.PorN == PorN.Negative && k.IsButton(Event.Down));
        }
    }
}