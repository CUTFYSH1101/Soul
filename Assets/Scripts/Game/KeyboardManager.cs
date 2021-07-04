using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using static InputManager.Event;
using static Main.Common.Hotkeys;
using Input = UnityEngine.Input;

public class KeyboardManager : MonoBehaviour
{
    private readonly InputManager inputManager = new InputManager();

    private void Awake() =>
        inputManager
            .AddEventListener(onKeyDown, QuitGame, Application.Quit)
            .AddEventListener(onKeyDown, Reset, () => SceneManager.LoadScene(SceneManager.GetActiveScene().name));

    private void Update() => InputManager.Update();
}

public class InputManager
{
    public enum Event
    {
        onKey,
        onKeyDown,
        onKeyUp
    }

    private static Action update;

    public InputManager AddEventListener(Event @event, string key, [NotNull] Action callback)
    {
        Action newEvent = null;
        switch (@event)
        {
            case onKey:
                newEvent = () =>
                {
                    if (Main.Common.Input.GetButton(key)) callback.Invoke();
                };
                break;
            case onKeyDown:
                newEvent = () =>
                {
                    if (Main.Common.Input.GetButtonDown(key)) callback.Invoke();
                };
                break;
            case onKeyUp:
                newEvent = () =>
                {
                    if (Main.Common.Input.GetButtonUp(key)) callback.Invoke();
                };
                break;
            default:
                Debug.LogError("超出範圍");
                break;
        }

        update += newEvent;
        return this;
    }

    public static void Update() => update?.Invoke();
}