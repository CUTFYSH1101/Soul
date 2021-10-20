using System;
using UnityEngine;

namespace Test
{
    internal class GUILog
    {
        private readonly (GUIStyle style, Rect rect, Action<string> log) _gui;
        // OnAwake, OnStart, 記得不要在區域空間!
        public GUILog()
        {
            _gui.style = new GUIStyle { fontSize = 14, normal = { textColor = new Color(0.07f,0.4f,0.39f) } };
            _gui.rect = new Rect(Screen.width * 0.1f, Screen.height * 0.1f, Screen.width * 0.25f, Screen.width * 0.25f);
            _gui.log += message => GUI.Label(_gui.rect, message, _gui.style);
        }
        // OnGUI
        public void ShowTextOnScene(string message) => _gui.log(message);
    }
}