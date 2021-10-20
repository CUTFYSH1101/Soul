using Test.Scene.Scripts.Main.Game;
using Test.Scene.Scripts.Main.UI;

namespace Test.Scene.Scripts.Game
{
    /// 在遊戲引擎中變更場景的工具
    public class SceneHandler
    {
        private readonly SceneLoadingUI _ui; // 場景變更系統

        public SceneHandler(string hierarchyPath = "UI/PanelLoadingUI") => _ui = new SceneLoadingUI(hierarchyPath);
        /// 在遊戲引擎中切換顯示場景、並顯示過場動畫
        public void ChangeScene(EnumMapTag newMap) => _ui.NewGameScene(UnityAsyncSceneManager.LoadScene(newMap));
    }
}