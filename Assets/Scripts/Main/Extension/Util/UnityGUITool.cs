using System;
using UnityEngine;
using UnityEngine.UI;

namespace Main.Util
{
    public static class UnityGUITool
    {
        public static void AddOnClickListener(this Button source, Action action)
        {
            source.onClick.AddListener(() => action());
        }

        public static void AddOnClickListener(this Component container, string buttonName, Action action)
        {
            container.GetFirstComponent<Button>(buttonName).AddOnClickListener(action);
        }
    }
}