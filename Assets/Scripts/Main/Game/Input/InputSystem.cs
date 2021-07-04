using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Game.Input
{
    public class InputSystem
    {
        private readonly InputManager inputManager = new InputManager();

        public void Awake() =>
            inputManager
                .AddEventListener(Event.Down, HotkeySet.QuitGame, Application.Quit)
                .AddEventListener(Event.Down, HotkeySet.Reset, () => SceneManager.LoadScene(SceneManager.GetActiveScene().name));

        public void Update() => InputManager.UpdateListener();
    }
}