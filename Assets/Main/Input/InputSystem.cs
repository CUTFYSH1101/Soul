using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Input
{
    public class InputSystem
    {
        private readonly InputManager _inputManager = new InputManager();

        public void Awake() =>
            _inputManager
                .AddEventListener(Event.Down, HotkeySet.QuitGame, Application.Quit)
                .AddEventListener(Event.Down, HotkeySet.Reset, () => SceneManager.LoadScene(SceneManager.GetActiveScene().name));

        public void Update() => InputManager.UpdateListener();
    }
}