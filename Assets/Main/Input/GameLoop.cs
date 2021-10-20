using System;
using System.Collections.Generic;
using Main.Util;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Enum = System.Enum;
using UnityInput = UnityEngine.Input;

// todo
namespace Main.Input
{
    public class GameLoop : MonoBehaviour
    {
        public static IEnumerable<KeyCode> GetCurrentKeys()
        {
            if (UnityInput.anyKeyDown)
            {
                var _keyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));
                for (int i = 0; i < _keyCodes.Length; i++)
                    if (UnityInput.GetKey(_keyCodes[i]))
                        yield return _keyCodes[i];
            }
        }

        private static void DetectKeyCodeWhichDuringPress()
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                if (UnityInput.GetKey(keyCode))
                    Debug.Log("KeyCode down: " + keyCode);
        }

        private PlayerControls _controls;

        private void Awake()
        {
            _controls = new PlayerControls();
            // _controls.Gamepad.Fire1.performed += context => Debug.Log(context);
        }

        private void OnEnable()
        {
            _controls.Enable();
            _controls.Gamepad.Enable();
        }

        private static bool IsPressedDown(ButtonControl gamepad) => gamepad.wasPressedThisFrame;
        private static bool IsPressed(ButtonControl gamepad) => gamepad.isPressed;
        private static bool IsReleased(ButtonControl gamepad) => gamepad.wasReleasedThisFrame;

        /// keydown
        public static bool IsPressedDown(Func<Gamepad, ButtonControl> button)
        {
            var gamepad = Gamepad.current;
            return gamepad != null && button(gamepad).wasPressedThisFrame;
        }

        /// key
        public static bool IsPressed(Func<Gamepad, ButtonControl> button)
        {
            var gamepad = Gamepad.current;
            return gamepad != null && button(gamepad).isPressed;
        }

        /// keyup
        public static bool IsReleased(Func<Gamepad, ButtonControl> button)
        {
            var gamepad = Gamepad.current;
            return gamepad != null && button(gamepad).wasReleasedThisFrame;
        }

        /// dpad方向鍵
        public static Vector2 GetDPadAxisRaw()
        {
            var control = Gamepad.current?.dpad;
            return control?.ReadValue() ?? Vector2.zero;
        }

        /// 左方向鍵
        public static Vector2 GetStickLeftAxisRaw()
        {
            var control = Gamepad.current?.leftStick;
            return control?.ReadValue() ?? Vector2.zero;
        }

        /// 右方向鍵
        public static Vector2 GetStickRightAxisRaw()
        {
            var control = Gamepad.current?.rightStick;
            return control?.ReadValue() ?? Vector2.zero;
        }

        private int count;

        private void Update()
        {
            // EnumJoyStick.leftStick.GetAxisRaw().LogLine();
            Input.GetAxisRaw(HotkeySet.Horizontal).LogLine();
            /*Log2(KeyCode.JoystickButton0); // 南
            Log2(KeyCode.JoystickButton1); // 東
            Log2(KeyCode.JoystickButton2); // 西
            Log2(KeyCode.JoystickButton3); // 北
            Log2(KeyCode.JoystickButton4); // left shoulder
            Log2(KeyCode.JoystickButton5); // right shoulder
            Log2(KeyCode.JoystickButton6); // select
            Log2(KeyCode.JoystickButton7); // start
            Log2(KeyCode.JoystickButton8); // left stick
            Log2(KeyCode.JoystickButton9); // right stick
            Log2(KeyCode.JoystickButton10);
            Log2(KeyCode.JoystickButton11);
            Log2(KeyCode.JoystickButton12);
            Log2(KeyCode.JoystickButton13);
            Log2(KeyCode.JoystickButton14);
            Log2(KeyCode.JoystickButton15);
            Log2(KeyCode.JoystickButton16);
            Log2(KeyCode.JoystickButton17);
            Log2(KeyCode.JoystickButton18);
            Log2(KeyCode.JoystickButton19);
            Log2(KeyCode.K);*/

            // Gamepad.current.buttonWest.ReadValue().LogLine();


            /*IsPressedDown(gamepad => gamepad.buttonNorth); // x
            IsPressedDown(gamepad => gamepad.buttonWest); // x
            IsPressedDown(gamepad => gamepad.buttonSouth); // a
            IsPressedDown(gamepad => gamepad.buttonEast); // b
            IsPressedDown(gamepad => gamepad.selectButton); // icon like window
            IsPressedDown(gamepad => gamepad.startButton);
            IsPressed(gamepad => gamepad.buttonWest);
            IsPressed(gamepad => gamepad.triangleButton);

            Log(gamepad => gamepad.rightShoulder);
            Log(gamepad => gamepad.rightTrigger);
            Log(gamepad => gamepad.rightStickButton);
            Log(gamepad => gamepad.startButton);
            Log(gamepad => gamepad.selectButton);
            Log(gamepad => gamepad.triangleButton);
            $"左方向鍵:{GetStickLeftAxisRaw()}".LogLine();
            $"右方向鍵:{GetStickRightAxisRaw()}".LogLine();
            $"dpad: {GetDPadAxisRaw()}".LogLine();*/
            /*$"rightShoulder:{IsPressed(gamepad => gamepad.rightShoulder)}".LogLine();
            $"rightTrigger:{IsPressed(gamepad => gamepad.rightTrigger)}".LogLine();
            $"rightStickButton:{IsPressed(gamepad => gamepad.rightStickButton)}".LogLine();
            $"start:{IsPressed(gamepad => gamepad.startButton)}".LogLine();
            $"select:{IsPressed(gamepad => gamepad.selectButton)}".LogLine();
            $"triangle:{IsPressed(gamepad => gamepad.triangleButton)}".LogLine();
            $"左方向鍵:{GetStickLeftAxisRaw()}".LogLine();
            $"右方向鍵:{GetStickRightAxisRaw()}".LogLine();
            $"dpad: {GetDPadAxisRaw()}".LogLine();*/
        }

        private void Log2(KeyCode keyCode)
        {
            if (UnityEngine.Input.GetKey(keyCode)) $"{keyCode.ToString()}".LogLine();
        }

        private static void Log(Func<Gamepad, ButtonControl> button)
        {
            var gamepad = Gamepad.current;
            if (IsPressed(button)) $"{button(gamepad).name}".LogLine();
        }
        /*
        private void Update()
        {
            var keyboard = Keyboard.current;
            var mouse = Mouse.current;
            var gamepad = Gamepad.current;
            var keyControl = Keyboard.current.jKey;
            var buttonControl = Mouse.current.middleButton;
            var buttonControl2 = Gamepad.current.buttonWest;
            var dpadControl = Gamepad.current.dpad;
            var buttonControl3 = Gamepad.current.dpad.down;
            if (keyboard.wasUpdatedThisFrame) Debug.Log("有按鍵被點擊或放開時");
            if (keyboard.jKey.wasReleasedThisFrame) Debug.Log("jKey.keyup");
            if (keyboard.jKey.wasPressedThisFrame) Debug.Log("jKey.keydown");
            if (keyboard.jKey.isPressed) Debug.Log("jKey.key");
        }
        */
        /*private void Update()
        {
            var keyboard = Keyboard.current;
            var mouse = Mouse.current;
            var gamepad = Gamepad.current;
            var keyControl = Keyboard.current.jKey;
            var buttonControl = Mouse.current.middleButton;
            var buttonControl2 = Gamepad.current.buttonWest;
            var dpadControl = Gamepad.current.dpad;
            var buttonControl3 = Gamepad.current.dpad.down;
            
            if (keyboard.wasUpdatedThisFrame) Debug.Log("wasUpdatedThisFrame");
            if (keyboard.jKey.wasReleasedThisFrame) Debug.Log("jKey.wasReleasedThisFrame");
            if (keyboard.jKey.wasPressedThisFrame) Debug.Log("jKey.wasPressedThisFrame");
            if (keyboard.jKey.isPressed) Debug.Log("jKey.isPressed");
            var gamepadDpad = gamepad.dpad;
            var dpadLeft = gamepadDpad.left;
            if (gamepadDpad.IsPressed()) Debug.Log("IsPressed");
            if (gamepadDpad.IsActuated()) Debug.Log("IsActuated");
            if (gamepadDpad.ReadValue() != Vector2.zero) Debug.Log(gamepadDpad.ReadValue());
            if (dpadLeft.invert) Debug.Log("invert");
            if (dpadLeft.noisy) Debug.Log("noisy");
            if (dpadLeft.normalize) Debug.Log("normalize");
            if (dpadLeft.scale) Debug.Log("scale");
            if (dpadLeft.synthetic) Debug.Log("synthetic");
            if (dpadLeft.ReadValue() != 0) Debug.Log(dpadLeft.ReadValue());
            if (gamepad.buttonWest.wasReleasedThisFrame) Debug.Log("buttonWest.wasReleasedThisFrame");
            if (gamepad.buttonWest.wasPressedThisFrame) Debug.Log("buttonWest.wasPressedThisFrame");
            if (gamepad.buttonWest.isPressed) Debug.Log("buttonWest.isPressed");
        }*/
    }
}

/*using System;
using System.Collections.Generic;
using UnityEngine;
using UnityInput = UnityEngine.Input;
namespace Main.Input
{
    public class GameLoop : MonoBehaviour
    {
        private void Update()
        {
            if (UnityInput.anyKeyDown) 
                Debug.Log(UnityInput.inputString);

            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                if (UnityInput.GetKey(keyCode))
                    Debug.Log("KeyCode down: " + keyCode);
            /*
            LogKeyCodeWhilePress(KeyCode.Joystick1Button0);
            LogKeyCodeWhilePress(KeyCode.Joystick1Button1);
            LogKeyCodeWhilePress(KeyCode.Joystick1Button2);
            LogKeyCodeWhilePress(KeyCode.Joystick1Button3);
            LogKeyCodeWhilePress(KeyCode.Joystick1Button4);
            LogKeyCodeWhilePress(KeyCode.Joystick1Button5);
            LogKeyCodeWhilePress(KeyCode.Joystick1Button6);
            LogKeyCodeWhilePress(KeyCode.Joystick1Button7);
            LogKeyCodeWhilePress(KeyCode.Joystick1Button8);
            LogKeyCodeWhilePress(KeyCode.Joystick1Button9);
            LogKeyCodeWhilePress(KeyCode.Joystick1Button10);
            LogKeyCodeWhilePress(KeyCode.Joystick1Button11);
            LogKeyCodeWhilePress(KeyCode.Joystick1Button12);
            LogKeyCodeWhilePress(KeyCode.Joystick1Button13);
            LogKeyCodeWhilePress(KeyCode.Joystick1Button14);
            LogKeyCodeWhilePress(KeyCode.Joystick1Button15);
            LogKeyCodeWhilePress(KeyCode.Joystick1Button16);
            LogKeyCodeWhilePress(KeyCode.Joystick1Button17);
            LogKeyCodeWhilePress(KeyCode.Joystick1Button18);
            LogKeyCodeWhilePress(KeyCode.Joystick1Button19);
        #1#
        }

        /*
        private void OnGUI()
        {
            if (Event.current.isKey && Event.current.type == EventType.KeyDown)
            {
                Debug.Log(Event.current.keyCode);
            }
        }
        #1#
        public static IEnumerable<KeyCode> GetCurrentKeys()
        {
            if (UnityInput.anyKeyDown)
            {
                var _keyCodes =(KeyCode[]) Enum.GetValues(typeof(KeyCode));
                for (int i = 0; i < _keyCodes.Length; i++)
                    if (UnityInput.GetKey(_keyCodes[i]))
                        yield return _keyCodes[i];
            }
        }
        private static void DetectKeyCodeWhichDuringPress()
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                if (UnityInput.GetKey(keyCode))
                    Debug.Log("KeyCode down: " + keyCode);
        }

        private static void LogKeyCodeWhilePress(KeyCode keyCode)
        {
            if (UnityInput.GetKeyDown(keyCode))
                Debug.Log(keyCode.ToString());
        }
    }
}*/