using Test.Scene.Scripts.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Test.Scene.Scripts.Main.Game
{
    public static class UnityAsyncSceneManager
    {
        public static AsyncOperation LoadNextLevelScene()
        {
            var async = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            async.allowSceneActivation = false; //此處將允許自動場景載入禁用，防止到90%時自動跳轉到新場景。
            return async;
        }

        public static AsyncOperation LoadScene(string sceneName)
        {
            var async = SceneManager.LoadSceneAsync(sceneName);
            async.allowSceneActivation = false; //此處將允許自動場景載入禁用，防止到90%時自動跳轉到新場景。
            return async;
        }
        public static AsyncOperation LoadScene(EnumMapTag map)
        {
            var async = SceneManager.LoadSceneAsync(map.ReflectSceneName());
            async.allowSceneActivation = false; //此處將允許自動場景載入禁用，防止到90%時自動跳轉到新場景。
            return async;
        }
    }
}