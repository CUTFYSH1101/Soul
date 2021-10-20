using System;
using System.ComponentModel;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityInput = UnityEngine.Input;
using static Main.Input.EnumJoyStick;

namespace Main.Input
{
    public enum EnumJoyStick
    {
        [Description("選擇鍵")] SelectButton,
        [Description("開始鍵")] StartButton,
        ButtonWest,
        ButtonSouth,
        ButtonEast,
        ButtonNorth,
        [Description("前上")] LeftShoulder,
        [Description("前上")] RightShoulder,
        [Description("前下")] LeftTrigger,
        [Description("前下")] RightTrigger,
        [Description("左方向鍵")] LeftStick,
        [Description("右方向鍵")] RightStick,
        [Description("方向鍵")] Dpad,
    }

    public enum Direction
    {
        Horizontal,
        Vertical
    }

    public static class UnityJoyStickMethod
    {
        [CanBeNull]
        private static ButtonControl Get(this EnumJoyStick joyStick)
        {
            var gamepad = Gamepad.current;
            if (gamepad == null) return null;
            return joyStick switch
            {
                SelectButton => gamepad.selectButton,
                StartButton => gamepad.startButton,
                ButtonWest => gamepad.buttonWest,
                ButtonSouth => gamepad.buttonSouth,
                ButtonEast => gamepad.buttonEast,
                ButtonNorth => gamepad.buttonNorth,
                LeftShoulder => gamepad.leftShoulder,
                RightShoulder => gamepad.rightShoulder,
                LeftTrigger => gamepad.leftTrigger,
                RightTrigger => gamepad.rightTrigger,
                _ => throw new ArgumentOutOfRangeException(nameof(joyStick), joyStick, null)
            };
        }

        private static Vector2 GetDirection(this EnumJoyStick joyStick)
        {
            var gamepad = Gamepad.current;
            if (gamepad != null)
                return joyStick switch
                {
                    LeftStick => gamepad.leftStick.ReadValue(),
                    RightStick => gamepad.rightStick.ReadValue(),
                    Dpad => gamepad.dpad.ReadValue(),
                    _ => throw new ArgumentOutOfRangeException(nameof(joyStick), joyStick, null)
                };
            return Vector2.zero;
        }

        public static int GetAxisRaw(this EnumJoyStick joyStick, Direction direction = Direction.Horizontal)
        {
            if (joyStick is not (LeftStick or RightStick or Dpad)) direction = Direction.Horizontal;
            switch (joyStick)
            {
                case SelectButton:
                case StartButton:
                case ButtonWest:
                case ButtonSouth:
                case ButtonEast:
                case ButtonNorth:
                case LeftShoulder:
                case RightShoulder:
                case LeftTrigger:
                case RightTrigger:
                    return Math.Sign(joyStick.Get()?.ReadValue() ?? 0);
                case LeftStick:
                case RightStick:
                case Dpad:
                    var vec2 = joyStick.GetDirection();
                    return direction == Direction.Horizontal ? Math.Sign(vec2.x) : Math.Sign(vec2.y);
                default:
                    throw new ArgumentOutOfRangeException(nameof(joyStick), joyStick, null);
            }
        }

        public static bool GetKeyDown(this EnumJoyStick joyStick)
        {
            if (joyStick is not (LeftStick or RightStick or Dpad))
                return joyStick.Get()?.wasPressedThisFrame ?? false;
            if (joyStick is not Dpad)
                return UnityInput.GetKeyDown(KeyCode.JoystickButton8) || // left stick
                       UnityInput.GetKeyDown(KeyCode.JoystickButton9); // right stick
            return false;
        }

        public static bool GetKey(this EnumJoyStick joyStick)
        {
            if (joyStick is not (LeftStick or RightStick or Dpad))
                return joyStick.Get()?.isPressed ?? false;
            if (joyStick is LeftStick or RightStick)
                return UnityInput.GetKey(KeyCode.JoystickButton8) || // left stick
                       UnityInput.GetKey(KeyCode.JoystickButton9); // right stick

            /*if (joyStick is leftStick or rightStick)
                return UnityInput.GetKey(KeyCode.JoystickButton8) || // left stick
                       UnityInput.GetKey(KeyCode.JoystickButton9) || // right stick
                       joyStick.GetDirection() != Vector2.zero; // todo*/
            return false;
        }

        public static bool GetKeyUp(this EnumJoyStick joyStick)
        {
            if (joyStick is not (LeftStick or RightStick or Dpad))
                return joyStick.Get()?.wasReleasedThisFrame ?? false;
            if (joyStick is not Dpad)
                return UnityInput.GetKeyUp(KeyCode.JoystickButton8) || // left stick
                       UnityInput.GetKeyUp(KeyCode.JoystickButton9); // right stick
            return false;
        }

        /*/// keydown
        public static bool IsPressedDown(Func<Gamepad, ButtonControl> button)
        {
            var gamepad = Gamepad.current;
            return gamepad != null && button(gamepad).wasPressedThisFrame;
        }

        public static bool IsPressedDown(this EnumJoyStick map) =>
            Gamepad.current != null && map.Get().wasPressedThisFrame;

        /// key
        public static bool IsPressed(Func<Gamepad, ButtonControl> button)
        {
            var gamepad = Gamepad.current;
            return gamepad != null && button(gamepad).isPressed;
        }

        public static bool IsPressed(this EnumJoyStick map) =>
            Gamepad.current != null && map.Get().isPressed;

        /// keyup
        public static bool IsReleased(Func<Gamepad, ButtonControl> button)
        {
            var gamepad = Gamepad.current;
            return gamepad != null && button(gamepad).wasReleasedThisFrame;
        }

        public static bool IsReleased(this EnumJoyStick map) =>
            Gamepad.current != null && map.Get().wasReleasedThisFrame;

        public static Vector2 GetDirection(this EnumJoyStickArrow joyStick)
        {
            var gamepad = Gamepad.current;
            if (gamepad != null)
                return joyStick switch
                {
                    leftStick => gamepad.leftStick.ReadValue(),
                    rightStick => gamepad.rightStick.ReadValue(),
                    dpad => gamepad.dpad.ReadValue(),
                    _ => throw new ArgumentOutOfRangeException(nameof(joyStick), joyStick, null)
                };
            return Vector2.zero;
        }
        public static Vector2 GetAxisRaw(this EnumJoyStickArrow joyStick)
        {
            var vec2 = joyStick.GetDirection();
            return new Vector2(Math.Sign(vec2.x), Math.Sign(vec2.y));
        }
        public static int GetSingleDirectionAxisRaw(this EnumJoyStickArrow joyStick, bool isHorizontal = true)
        {
            var vec2 = joyStick.GetDirection();
            return isHorizontal ? Math.Sign(vec2.x) : Math.Sign(vec2.y);
        }*/
    }
}
/*using System;
using UnityEngine;
using UnityInputSystem;
using UnityInputSystem.Controls;
using static Main.Input.EnumJoyStickArrow;
using static Main.Input.EnumJoyStick;

namespace Main.Input
{
    public enum EnumJoyStick
    {
        selectButton,
        startButton,
        buttonWest,
        buttonSouth,
        buttonEast,
        buttonNorth,
    }

    public enum EnumJoyStickArrow
    {
        leftStick,
        rightStick,
        dpad,
    }

    public static class UnityJoyStickMethod
    {
        /// keydown
        public static bool IsPressedDown(Func<Gamepad, ButtonControl> button)
        {
            var gamepad = Gamepad.current;
            return gamepad != null && button(gamepad).wasPressedThisFrame;
        }

        public static bool IsPressedDown(this EnumJoyStick map) =>
            Gamepad.current != null && map.Get().wasPressedThisFrame;

        /// key
        public static bool IsPressed(Func<Gamepad, ButtonControl> button)
        {
            var gamepad = Gamepad.current;
            return gamepad != null && button(gamepad).isPressed;
        }

        public static bool IsPressed(this EnumJoyStick map) =>
            Gamepad.current != null && map.Get().isPressed;

        /// keyup
        public static bool IsReleased(Func<Gamepad, ButtonControl> button)
        {
            var gamepad = Gamepad.current;
            return gamepad != null && button(gamepad).wasReleasedThisFrame;
        }

        public static bool IsReleased(this EnumJoyStick map) =>
            Gamepad.current != null && map.Get().wasReleasedThisFrame;

        public static Vector2 GetDirection(this EnumJoyStickArrow joyStick)
        {
            var gamepad = Gamepad.current;
            if (gamepad != null)
                return joyStick switch
                {
                    leftStick => gamepad.leftStick.ReadValue(),
                    rightStick => gamepad.rightStick.ReadValue(),
                    dpad => gamepad.dpad.ReadValue(),
                    _ => throw new ArgumentOutOfRangeException(nameof(joyStick), joyStick, null)
                };
            return Vector2.zero;
        }
        public static Vector2 GetAxisRaw(this EnumJoyStickArrow joyStick)
        {
            var vec2 = joyStick.GetDirection();
            return new Vector2(Math.Sign(vec2.x), Math.Sign(vec2.y));
        }
        public static int GetSingleDirectionAxisRaw(this EnumJoyStickArrow joyStick, bool isHorizontal = true)
        {
            var vec2 = joyStick.GetDirection();
            return isHorizontal ? Math.Sign(vec2.x) : Math.Sign(vec2.y);
        }
        public static ButtonControl Get(this EnumJoyStick map)
        {
            var gamepad = Gamepad.current;
            return map switch
            {
                selectButton => gamepad.selectButton,
                startButton => gamepad.startButton,
                buttonWest => gamepad.buttonWest,
                buttonSouth => gamepad.buttonSouth,
                buttonEast => gamepad.buttonEast,
                buttonNorth => gamepad.buttonNorth,
                _ => throw new ArgumentOutOfRangeException(nameof(map), map, null)
            };
        }
    }
}*/
/*
StickControl  // new Vector2()
DpadControl   // new Vector2()
ButtonControl // 0, 1
*/