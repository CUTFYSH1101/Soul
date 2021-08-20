using System.Collections.Generic;
using UnityEngine;

namespace Main.Input
{
    /// 客戶端
    public static class HotkeySet
    {
        public static string
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
            Qte3 = "QTE3", // T
            QuitGame = "QuitGame", // esc
            Reset = "Reset"; // R

        public static readonly Dictionary<string, KeyBundle> KeyEventSet = new Dictionary<string, KeyBundle>()
        {
            {Horizontal, new KeyBundle(KeyCode.D, KeyCode.A, KeyCode.RightArrow, KeyCode.LeftArrow)},
            {Vertical, new KeyBundle(KeyCode.W, KeyCode.S, KeyCode.UpArrow, KeyCode.DownArrow)},
            {Jump, new KeyBundle(KeyCode.Space)},
            {Trigger, new KeyBundle(KeyCode.W)},
            {Control, new KeyBundle(KeyCode.U)},
            {Fire1, new KeyBundle(KeyCode.J, 0)},
            {Fire2, new KeyBundle(KeyCode.K, 1)},
            {Fire3, new KeyBundle(KeyCode.L, 2)},
            {Qte1, new KeyBundle(KeyCode.Q)},
            {Qte2, new KeyBundle(KeyCode.E)},
            {Qte3, new KeyBundle(KeyCode.T)},
            {QuitGame, new KeyBundle(KeyCode.Escape)},
            {Reset, new KeyBundle(KeyCode.R)}
        };
    }
}