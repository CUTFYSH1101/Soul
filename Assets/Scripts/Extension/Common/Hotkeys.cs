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
            QTE3 = "QTE3", // T
            QuitGame = "QuitGame", // esc
            Reset = "Reset"; // R

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
            {QuitGame, new KeyValue(KeyCode.Escape)},
            {Reset, new KeyValue(KeyCode.R)}
        };

        public static KeyValue GetValue(string key) => KeyCodes[key];
        public static KeyCode GetPositive(string key) => KeyCodes[key].positive;
        public static KeyCode GetNegative(string key) => KeyCodes[key].negative;
        public static KeyCode GetAltPositive(string key) => KeyCodes[key].altPositive;
        public static KeyCode GetAltNegative(string key) => KeyCodes[key].altNegative;
    }

    public class KeyValue
    {
        public readonly KeyCode positive, negative;
        public readonly KeyCode altPositive, altNegative;

        public KeyCode Positive => positive;

        public KeyCode Negative => negative.IsEmpty() ? positive : negative;

        public KeyCode AltPositive => altPositive.IsEmpty() ? positive : altPositive;

        public KeyCode AltNegative => altNegative.IsEmpty() ? positive : altNegative;

        public KeyValue(KeyCode positive)
        {
            this.positive = positive;
        }

        public KeyValue(KeyCode positive, KeyCode negative)
        {
            this.positive = positive;
            this.negative = negative;
        }

        public KeyValue(KeyCode positive, KeyCode negative, KeyCode altPositive, KeyCode altNegative)
        {
            this.positive = positive;
            this.negative = negative;
            this.altPositive = altPositive;
            this.altNegative = altNegative;
        }
    }

    public static class Input
    {
        public static bool GetButtonDown(string buttonName)
        {
            return UnityInput.GetKeyDown(Hotkeys.GetPositive(buttonName)) ||
                   UnityInput.GetKeyDown(Hotkeys.GetNegative(buttonName)) ||
                   UnityInput.GetKeyDown(Hotkeys.GetAltPositive(buttonName)) ||
                   UnityInput.GetKeyDown(Hotkeys.GetAltNegative(buttonName));
        }

        public static bool GetButton(string buttonName)
        {
            return UnityInput.GetKey(Hotkeys.GetPositive(buttonName)) ||
                   UnityInput.GetKey(Hotkeys.GetNegative(buttonName)) ||
                   UnityInput.GetKey(Hotkeys.GetAltPositive(buttonName)) ||
                   UnityInput.GetKey(Hotkeys.GetAltNegative(buttonName));
        }

        public static bool GetButtonUp(string buttonName)
        {
            return UnityInput.GetKeyUp(Hotkeys.GetPositive(buttonName)) ||
                   UnityInput.GetKeyUp(Hotkeys.GetNegative(buttonName)) ||
                   UnityInput.GetKeyUp(Hotkeys.GetAltPositive(buttonName)) ||
                   UnityInput.GetKeyUp(Hotkeys.GetAltNegative(buttonName));
        }

        public static float GetAxisRaw(string axisName)
        {
            if (UnityInput.GetKey(Hotkeys.GetPositive(axisName)) ||
                UnityInput.GetKey(Hotkeys.GetAltPositive(axisName)))
                return 1;
            else if (UnityInput.GetKey(Hotkeys.GetNegative(axisName)) ||
                     UnityInput.GetKey(Hotkeys.GetAltNegative(axisName)))
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