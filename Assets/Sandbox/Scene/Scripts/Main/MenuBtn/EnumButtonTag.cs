using System.ComponentModel;

namespace Test.Scene.Scripts.Main.MenuBtn
{
    public enum EnumButtonTag
    {
        // Start,
        [Description("繼續")] BtnContinue,
        [Description("重新開始")] BtnNewGame,
        [Description("退出")] BtnExit
    }
}