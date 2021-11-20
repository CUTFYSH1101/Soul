using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Sandbox.Scene.Scripts.Main.GUI
{
    public interface ILoadingVFX
    {
        public void NewGameScene(AsyncOperation async);

        public IEnumerator LoadingUpdateEnumerator(AsyncOperation async);

        public void LoadingUpdate(AsyncOperation async);
    }
}