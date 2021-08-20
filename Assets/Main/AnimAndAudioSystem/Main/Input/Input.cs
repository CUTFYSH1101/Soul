namespace Main.AnimAndAudioSystem.Main.Input
{
    /// 客戶端
    public static class Input
    {
        public static bool GetButtonDown(string buttonName) => HotkeySet.KeyEventSet[buttonName].IsButton(Event.Down);
        public static bool GetButton(string buttonName) => HotkeySet.KeyEventSet[buttonName].IsButton(Event.Ing);
        public static bool GetButtonUp(string buttonName) => HotkeySet.KeyEventSet[buttonName].IsButton(Event.Up);
        public static int GetAxisRaw(string axisName) => HotkeySet.KeyEventSet[axisName].GetAxisRaw();
        public static int GetAxisRawDown(string axisName) => HotkeySet.KeyEventSet[axisName].GetAxisRawDown();
    }
}