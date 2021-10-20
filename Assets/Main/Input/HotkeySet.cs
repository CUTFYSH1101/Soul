using System.Collections.Generic;
using UnityEngine;
using static Main.Input.EnumJoyStick;

namespace Main.Input
{
    /// 客戶端
    public static class HotkeySet
    {
        public static string
            DebugMode = "DebugMode", // F3
            QuitGame = "QuitGame", // esc
            Reset = "Reset", // R
            Horizontal = "Horizontal", // A, D
            Vertical = "Vertical", // W, S
            Jump = "Jump", // space
            Trigger = "Trigger", // W
            Control = "Control", // U
            Fire1 = "Fire1", // J, mouse 0
            Fire2 = "Fire2", // K, mouse 1
            Fire3 = "Fire3", // L, mouse 2
            Qte1 = "QTE1", // Q
            Qte2 = "QTE2", // E
            Qte3 = "QTE3"; // T


        public static readonly Dictionary<string, KeyBundle> KeyEventSet = new()
        {
            {Horizontal, new KeyBundle(KeyCode.D, KeyCode.A, KeyCode.RightArrow, KeyCode.LeftArrow).Add(LeftStick)},
            {Vertical, new KeyBundle(KeyCode.W, KeyCode.S, KeyCode.UpArrow, KeyCode.DownArrow)},
            {Jump, new KeyBundle(KeyCode.Space).Add(ButtonNorth)},
            {Trigger, new KeyBundle(KeyCode.W).Add(ButtonNorth)},
            {Control, new KeyBundle(KeyCode.U)},
            {Fire1, new KeyBundle(KeyCode.J, 0).Add(ButtonWest)},
            {Fire2, new KeyBundle(KeyCode.K, 2).Add(ButtonSouth)},
            {Fire3, new KeyBundle(KeyCode.L, 1).Add(ButtonEast)},
            {Qte1, new KeyBundle(KeyCode.Q).Add(ButtonWest)},
            {Qte2, new KeyBundle(KeyCode.E).Add(ButtonSouth)},
            {Qte3, new KeyBundle(KeyCode.T).Add(ButtonEast)},
            {QuitGame, new KeyBundle(KeyCode.Escape)},
            {Reset, new KeyBundle(KeyCode.R)},
            {DebugMode, new KeyBundle(KeyCode.F3)}
        };
    }
}