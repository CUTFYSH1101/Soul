using Test.Scene.Scripts.Game;
using Test.Scene.Scripts.Main.UI;
using UnityEngine;

namespace Test.Scene.Scripts
{
    public class GameLoop : MonoBehaviour
    {
        private SceneLoadingUI _ui;
        public Component test;
        public EntityData data = new(){attack_speed = 1};
        public GameCenter center;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            // _ui = new SceneLoadingUI("UI/PanelLoadingUI");
            
            center = new GameCenter();
            center.ChangeSceneScriptState(EnumMapTag.MapTest);
        }

        private void Update()
        {
            /*if (Input.anyKeyDown)
            {
                _ui.NewGameScene(UnityAsyncSceneHandler.LoadScene("map2"));
            }*/
            /*if (Input.GetKeyDown(KeyCode.S))
            {
                data.Save();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                data.Load();
            }*/
            center.Update();
        }
    }
}