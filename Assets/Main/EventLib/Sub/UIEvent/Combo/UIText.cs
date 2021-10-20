using System;
using Main.Game;
using TMPro;
using UnityEngine;

namespace Main.EventLib.Sub.UIEvent.Combo
{
    public class UIText
    {
        private readonly TMP_Text _compText;

        public float Alpha
        {
            get => _compText.color.a;
            set
            {
                var color = _compText.color;
                color.a = value;
                _compText.color = color;
            }
        }

        public string Text
        {
            get => _compText.text;
            set => _compText.text = value;
        }

        public float FontSize
        {
            get => _compText.fontSize;
            set => _compText.fontSize = value;
        }

        public UIText(string hierarchyPath)
        {
            _compText = UnityTool.GetComponentByPath<TMP_Text>(hierarchyPath);
            if (_compText == null) _compText = UnityTool.GetComponentByPath<Transform>(hierarchyPath).GetComponentInChildren<TMP_Text>();
            if (_compText == null) throw new Exception("場景不含有該物件，卻試圖想要獲取它！");
        }

        public UIText(TMP_Text compText) => _compText = compText;
    }
}