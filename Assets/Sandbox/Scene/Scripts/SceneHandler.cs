using Sandbox.Scene.Scripts.Main.GUI;
using Test.Scene.Scripts.Main.Game;

namespace Test.Scene.Scripts.Game
{
    /// 在遊戲引擎中變更場景的工具
    public class SceneHandler
    {
        private readonly LoadingVFX _vfx; // 場景變更系統

        public SceneHandler(string hierarchyPath = "UI/PanelLoadingUI") => _vfx = new LoadingVFX(hierarchyPath);
        /// 在遊戲引擎中切換顯示場景、並顯示過場動畫
        public void ChangeScene(EnumMapTag newMap) => _vfx.NewGameScene(UnityAsyncSceneManager.LoadScene(newMap));
    }
}