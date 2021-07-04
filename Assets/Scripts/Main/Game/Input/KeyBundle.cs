using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Main.Game.Input
{
    /// 客戶端
    public static class Input
    {
        public static bool GetButtonDown(string buttonName) => HotkeySet.KeyEventSet[buttonName].IsButton(Event.Down);
        public static bool GetButton(string buttonName) => HotkeySet.KeyEventSet[buttonName].IsButton(Event.Ing);
        public static bool GetButtonUp(string buttonName) => HotkeySet.KeyEventSet[buttonName].IsButton(Event.Up);
        public static float GetAxisRaw(string axisName) => HotkeySet.KeyEventSet[axisName].GetAxisRaw();
    }

    /// 客戶端
    public class KeyBundle
    {
        private readonly List<IKey> keys = new List<IKey>();

        public KeyBundle(IKey key) =>
            Append(key);

        public KeyBundle(IKey key1, IKey key2) =>
            Append(key1)
                .Append(key2);

        public KeyBundle(IKey key1, IKey key2, IKey key3, IKey key4) =>
            Append(key1)
                .Append(key2)
                .Append(key3)
                .Append(key4);

        /// 1P。1個鍵盤按鍵
        public KeyBundle(KeyCode key) =>
            Append(new ExtraKey(PorN.Positive, key));
        /// 1P。1個滑鼠
        public KeyBundle(int key) =>
            Append(new ExtraMouse(PorN.Positive, key));

        /// 2P。1鍵盤1滑鼠。JKL專屬，未來估計有GamePad加入。
        public KeyBundle(KeyCode key1, int key2) =>
            Append(new ExtraKey(PorN.Positive, key1))
                .Append(new ExtraMouse(PorN.Positive, key2));

        /// 2P+2N。鍵盤與滑鼠組合
        public KeyBundle(KeyCode key1, KeyCode key2, int key3, int key4) =>
            Append(new ExtraKey(PorN.Positive, key1))
                .Append(new ExtraKey(PorN.Negative, key2))
                .Append(new ExtraMouse(PorN.Positive, key3))
                .Append(new ExtraMouse(PorN.Negative, key4));

        /// 2P+2N。4個鍵盤按鍵
        public KeyBundle(KeyCode key1, KeyCode key2, KeyCode key3, KeyCode key4) =>
            Append(new ExtraKey(PorN.Positive, key1))
                .Append(new ExtraKey(PorN.Negative, key2))
                .Append(new ExtraKey(PorN.Positive, key3))
                .Append(new ExtraKey(PorN.Negative, key4));

        public KeyBundle Append(IKey key)
        {
            keys.Add(key);
            return this;
        }

        public bool IsButton(Event @event)
            => keys.Any(k => k.IsButton(@event));

        public int GetAxisRaw()
        {
            var anyP = keys.Any(k => k.PorN == PorN.Positive && k.IsButton(Event.Ing));
            var anyN = keys.Any(k => k.PorN == PorN.Negative && k.IsButton(Event.Ing));
            if (anyP)
                return 1;
            else if (anyN)
                return -1;
            else
                return 0;
        }
    }
}
/*void p()
{
    Key key = new Key(PorN.Positive, KeyCode.J);
    keys.Add(key);
}*/