using System;
using System.Diagnostics.CodeAnalysis;
using Main.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Test.Scene.Scripts.Main.MenuBtn
{
    // 與unity畫面中的按鈕做連結
    public static class MenuBtn
    {
        public static readonly Transform Canvas = UnityTool.GetComponentByPath<Transform>("UI/PanelMenu");
        public static void AddOnClickListener(this EnumButtonTag tag, [NotNull] Action e)
        {
            if (Canvas == null) return;
            var button = GetButton(Canvas, tag);
            
            if (button == null) return;
            button.onClick.AddListener(e.Invoke);
        }
        public static void RemoveAllOnClickListeners(this EnumButtonTag tag)
        {
            if (Canvas == null) return;
            var button = GetButton(Canvas, tag);
            
            if (button == null) return;
            button.onClick.RemoveAllListeners();
        }
        private static Button GetButton(Component parent, EnumButtonTag tag) =>
            tag switch
            {
                EnumButtonTag.BtnContinue => parent.GetFirstComponentInChildren<Button>("BtnContinue"),
                EnumButtonTag.BtnNewGame => parent.GetFirstComponentInChildren<Button>("BtnNewGame"),
                EnumButtonTag.BtnExit => parent.GetFirstComponentInChildren<Button>("BtnExit"),
                _ => throw new ArgumentOutOfRangeException(nameof(tag), tag, null)
            };
    }
}