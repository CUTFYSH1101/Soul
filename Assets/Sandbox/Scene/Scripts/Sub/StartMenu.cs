using Test.Scene.Scripts.Game;
using Test.Scene.Scripts.Main.MenuBtn;
using Test.Scene.Scripts.Main.SceneStateScript;
using UnityEngine;
using static Test.Scene.Scripts.Main.MenuBtn.EnumButtonTag;

namespace Test.Scene.Scripts.Sub
{
    public class StartMenu : SceneStateScript
    {
        public override void Enter()
        {
            BtnContinue.AddOnClickListener(() =>
            {
                Debug.Log("載入先前進度");
                Center.SceneHandler.ChangeScene(EnumMapTag.Victory);
                Center.MainCharacterData.Load(); // dont destroy this
                Center.ChangeSceneScriptState(EnumMapTag.Victory);
            });
            BtnNewGame.AddOnClickListener(() =>
            {
                Debug.Log("重新開始");
                Center.SceneHandler.ChangeScene(EnumMapTag.Victory);
                Center.MainCharacterData.Save(); // 覆蓋舊檔案
                Center.ChangeSceneScriptState(EnumMapTag.Victory);
            });
            BtnExit.AddOnClickListener(() =>
            {
                Application.Quit();
                Debug.Log("退出");
            });
        }
        public override void Exit()
        {
            BtnContinue.RemoveAllOnClickListeners();
            BtnNewGame.RemoveAllOnClickListeners();
            BtnExit.RemoveAllOnClickListeners();
        }
    }
}