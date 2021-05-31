using System.Collections.Generic;
using Main.Util;
using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace Main.Common
{
    public static class Hotkeys
    {
        public static string
            Horizontal = "Horizontal", // A, D
            Vertical = "Vertical", // W, S
            Jump = "Jump", // space
            Trigger = "Trigger", // W
            Control = "Control", // U
            Fire1 = "Fire1", // J
            Fire2 = "Fire2", // K
            Fire3 = "Fire3", // L
            QTE1 = "QTE1", // Q
            QTE2 = "QTE2", // E
            QTE3 = "QTE3"; // T

        public static Dictionary<string, KeyValue> KeyCodes = new Dictionary<string, KeyValue>()
        {
            {Horizontal, new KeyValue(KeyCode.D, KeyCode.A, KeyCode.RightArrow, KeyCode.LeftArrow)},
            {Vertical, new KeyValue(KeyCode.W, KeyCode.S, KeyCode.UpArrow, KeyCode.DownArrow)},
            {Jump, new KeyValue(KeyCode.Space)},
            {Trigger, new KeyValue(KeyCode.W)},
            {Control, new KeyValue(KeyCode.U)},
            {Fire1, new KeyValue(KeyCode.J)},
            {Fire2, new KeyValue(KeyCode.K)},
            {Fire3, new KeyValue(KeyCode.L)},
            {QTE1, new KeyValue(KeyCode.Q)},
            {QTE2, new KeyValue(KeyCode.E)},
            {QTE3, new KeyValue(KeyCode.T)},
        };

        public static KeyValue GetValue(string key) => KeyCodes[key];
        public static KeyCode GetPositive(string key) => KeyCodes[key].Positive;
        public static KeyCode GetNegative(string key) => KeyCodes[key].Negative;
        public static KeyCode GetAltPositive(string key) => KeyCodes[key].AltPositive;
        public static KeyCode GetAltNegative(string key) => KeyCodes[key].AltNegative;
    }

    public class KeyValue
    {
        public readonly KeyCode Positive, Negative;
        public readonly KeyCode AltPositive, AltNegative;

        public KeyValue(KeyCode positive)
        {
            Positive = positive;
        }

        public KeyValue(KeyCode positive, KeyCode negative)
        {
            Positive = positive;
            Negative = negative;
        }

        public KeyValue(KeyCode positive, KeyCode negative, KeyCode altPositive, KeyCode altNegative)
        {
            Positive = positive;
            Negative = negative;
            AltPositive = altPositive;
            AltNegative = altNegative;
        }
    }

    public static class Input
    {
        public static bool GetButtonDown(string buttonName) => UnityInput.GetKeyDown(Hotkeys.GetPositive(buttonName));

        public static bool GetButton(string buttonName) => UnityInput.GetKey(Hotkeys.GetPositive(buttonName));

        public static bool GetButtonUp(string buttonName) => UnityInput.GetKeyUp(Hotkeys.GetPositive(buttonName));

        public static float GetAxisRaw(string axisName)
        {
            if (UnityInput.GetKey(Hotkeys.GetPositive(axisName)))
                return 1;
            else if (UnityInput.GetKey(Hotkeys.GetNegative(axisName)))
                return -1;
            else
                return 0;
        }

        private const double Gravity = 3;
        private static double axis;

        private delegate double Less(ref double num, double middle = 0);

        private static Less LessDown => (ref double num, double middle) => num.Less(Gravity * Time.deltaTime, middle);
    }
}