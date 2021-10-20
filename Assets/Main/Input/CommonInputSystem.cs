using System;
using JetBrains.Annotations;
using Main.Entity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Input
{
    public class CommonInputSystem : IComponent
    {
        private readonly InputManager _inputManager = new();

        public CommonInputSystem() => Enter();

        private void Enter() =>
            _inputManager
                .AddEventListener(DataType.Event.Down, HotkeySet.QuitGame, Application.Quit)
                .AddEventListener(DataType.Event.Down, HotkeySet.Reset, () => SceneManager.LoadScene(SceneManager.GetActiveScene().name))
                .AddEventListener(DataType.Event.Down,HotkeySet.DebugMode,null);// todo creatureList

        public EnumComponentTag Tag => EnumComponentTag.InputSystem;
        public void Update() => InputManager.UpdateListener();

        #region InputManager
        public class InputManager
        {
            [CanBeNull] private static Action AllEventListener { get; set; }

            public InputManager AddEventListener(DataType.Event @event, string key, [NotNull] Action callback)
            {
                Action newEvent = @event switch
                {
                    DataType.Event.Up => () =>
                    {
                        if (Input.GetButtonUp(key)) callback.Invoke();
                    },
                    DataType.Event.Ing => () =>
                    {
                        if (Input.GetButton(key)) callback.Invoke();
                    },
                    DataType.Event.Down => () =>
                    {
                        if (Input.GetButtonDown(key)) callback.Invoke();
                    },
                    _ => throw new ArgumentOutOfRangeException(nameof(@event), @event, null)
                };
                AllEventListener += newEvent;
                return this;
            }

            public static void UpdateListener() => AllEventListener?.Invoke();
        }
        #endregion
    }
}