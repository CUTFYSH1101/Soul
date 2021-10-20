//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.1.0
//     from Assets/Main/Input/PlayerControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Gamepad"",
            ""id"": ""7e0ff979-42e8-4d74-9bda-72615b58c76a"",
            ""actions"": [
                {
                    ""name"": ""StickLeft"",
                    ""type"": ""Button"",
                    ""id"": ""8048745f-aba4-4880-b14d-a3f5dc47b004"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DPad"",
                    ""type"": ""Button"",
                    ""id"": ""c09c6db9-56a7-4b54-ba5e-f6c6ee78061d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""StickRight"",
                    ""type"": ""Button"",
                    ""id"": ""ea559058-095b-4289-b561-7ec5108b11e4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoulder"",
                    ""type"": ""Button"",
                    ""id"": ""600bbf24-83eb-4383-ad17-ccc18e741e18"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Trigger"",
                    ""type"": ""Button"",
                    ""id"": ""3fd66154-69a4-42a3-bfa0-b9ec24b07d7e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""9e6a971f-0803-491a-9eae-51bc4cac7a43"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Start"",
                    ""type"": ""Button"",
                    ""id"": ""b06171b4-6888-4a69-a72d-1a78ca538c81"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""628820b8-676d-4086-bee1-def9790a582d"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StickLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Horizontal"",
                    ""id"": ""cb097829-3ac4-4e3a-baaa-3c6f79e70940"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DPad"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""f76d0ba8-7bab-4837-8b0f-dc1c55c31e29"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DPad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""853baee2-bd63-4632-9d21-26d229e7683b"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DPad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Vertical"",
                    ""id"": ""13900ef9-a288-4ccb-ab7d-243348caa6a2"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DPad"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""f4690476-a14e-4297-88e0-29b33541e951"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DPad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""abb08148-27d0-4135-b33f-f58c3e69ac0e"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DPad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1e369815-f1d9-4e1e-90f1-6add90b7aa3e"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StickRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""6fb5d58d-db0e-4a73-bbc4-c4e53195a3d7"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoulder"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""3d490094-866f-415a-a88c-a92b6edacffa"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoulder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""6e920795-c9ad-47ab-8809-92efdabba78d"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoulder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""302d0dcb-ac44-4483-8a0d-4a99aab385da"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Trigger"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""c11792f9-2f10-41c7-b95f-d7b0c8ad0e49"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Trigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""18177598-3fe0-4674-a792-cc548076fe1b"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Trigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""556b575a-9cda-430a-8491-382643743afd"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""30007a94-ba29-4056-8d5f-bf3350ad8a6d"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Gamepad control scheme"",
            ""bindingGroup"": ""Gamepad control scheme"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Keyboard control scheme"",
            ""bindingGroup"": ""Keyboard control scheme"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Mouse control scheme"",
            ""bindingGroup"": ""Mouse control scheme"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Gamepad
        m_Gamepad = asset.FindActionMap("Gamepad", throwIfNotFound: true);
        m_Gamepad_StickLeft = m_Gamepad.FindAction("StickLeft", throwIfNotFound: true);
        m_Gamepad_DPad = m_Gamepad.FindAction("DPad", throwIfNotFound: true);
        m_Gamepad_StickRight = m_Gamepad.FindAction("StickRight", throwIfNotFound: true);
        m_Gamepad_Shoulder = m_Gamepad.FindAction("Shoulder", throwIfNotFound: true);
        m_Gamepad_Trigger = m_Gamepad.FindAction("Trigger", throwIfNotFound: true);
        m_Gamepad_Select = m_Gamepad.FindAction("Select", throwIfNotFound: true);
        m_Gamepad_Start = m_Gamepad.FindAction("Start", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Gamepad
    private readonly InputActionMap m_Gamepad;
    private IGamepadActions m_GamepadActionsCallbackInterface;
    private readonly InputAction m_Gamepad_StickLeft;
    private readonly InputAction m_Gamepad_DPad;
    private readonly InputAction m_Gamepad_StickRight;
    private readonly InputAction m_Gamepad_Shoulder;
    private readonly InputAction m_Gamepad_Trigger;
    private readonly InputAction m_Gamepad_Select;
    private readonly InputAction m_Gamepad_Start;
    public struct GamepadActions
    {
        private @PlayerControls m_Wrapper;
        public GamepadActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @StickLeft => m_Wrapper.m_Gamepad_StickLeft;
        public InputAction @DPad => m_Wrapper.m_Gamepad_DPad;
        public InputAction @StickRight => m_Wrapper.m_Gamepad_StickRight;
        public InputAction @Shoulder => m_Wrapper.m_Gamepad_Shoulder;
        public InputAction @Trigger => m_Wrapper.m_Gamepad_Trigger;
        public InputAction @Select => m_Wrapper.m_Gamepad_Select;
        public InputAction @Start => m_Wrapper.m_Gamepad_Start;
        public InputActionMap Get() { return m_Wrapper.m_Gamepad; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GamepadActions set) { return set.Get(); }
        public void SetCallbacks(IGamepadActions instance)
        {
            if (m_Wrapper.m_GamepadActionsCallbackInterface != null)
            {
                @StickLeft.started -= m_Wrapper.m_GamepadActionsCallbackInterface.OnStickLeft;
                @StickLeft.performed -= m_Wrapper.m_GamepadActionsCallbackInterface.OnStickLeft;
                @StickLeft.canceled -= m_Wrapper.m_GamepadActionsCallbackInterface.OnStickLeft;
                @DPad.started -= m_Wrapper.m_GamepadActionsCallbackInterface.OnDPad;
                @DPad.performed -= m_Wrapper.m_GamepadActionsCallbackInterface.OnDPad;
                @DPad.canceled -= m_Wrapper.m_GamepadActionsCallbackInterface.OnDPad;
                @StickRight.started -= m_Wrapper.m_GamepadActionsCallbackInterface.OnStickRight;
                @StickRight.performed -= m_Wrapper.m_GamepadActionsCallbackInterface.OnStickRight;
                @StickRight.canceled -= m_Wrapper.m_GamepadActionsCallbackInterface.OnStickRight;
                @Shoulder.started -= m_Wrapper.m_GamepadActionsCallbackInterface.OnShoulder;
                @Shoulder.performed -= m_Wrapper.m_GamepadActionsCallbackInterface.OnShoulder;
                @Shoulder.canceled -= m_Wrapper.m_GamepadActionsCallbackInterface.OnShoulder;
                @Trigger.started -= m_Wrapper.m_GamepadActionsCallbackInterface.OnTrigger;
                @Trigger.performed -= m_Wrapper.m_GamepadActionsCallbackInterface.OnTrigger;
                @Trigger.canceled -= m_Wrapper.m_GamepadActionsCallbackInterface.OnTrigger;
                @Select.started -= m_Wrapper.m_GamepadActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_GamepadActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_GamepadActionsCallbackInterface.OnSelect;
                @Start.started -= m_Wrapper.m_GamepadActionsCallbackInterface.OnStart;
                @Start.performed -= m_Wrapper.m_GamepadActionsCallbackInterface.OnStart;
                @Start.canceled -= m_Wrapper.m_GamepadActionsCallbackInterface.OnStart;
            }
            m_Wrapper.m_GamepadActionsCallbackInterface = instance;
            if (instance != null)
            {
                @StickLeft.started += instance.OnStickLeft;
                @StickLeft.performed += instance.OnStickLeft;
                @StickLeft.canceled += instance.OnStickLeft;
                @DPad.started += instance.OnDPad;
                @DPad.performed += instance.OnDPad;
                @DPad.canceled += instance.OnDPad;
                @StickRight.started += instance.OnStickRight;
                @StickRight.performed += instance.OnStickRight;
                @StickRight.canceled += instance.OnStickRight;
                @Shoulder.started += instance.OnShoulder;
                @Shoulder.performed += instance.OnShoulder;
                @Shoulder.canceled += instance.OnShoulder;
                @Trigger.started += instance.OnTrigger;
                @Trigger.performed += instance.OnTrigger;
                @Trigger.canceled += instance.OnTrigger;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Start.started += instance.OnStart;
                @Start.performed += instance.OnStart;
                @Start.canceled += instance.OnStart;
            }
        }
    }
    public GamepadActions @Gamepad => new GamepadActions(this);
    private int m_GamepadcontrolschemeSchemeIndex = -1;
    public InputControlScheme GamepadcontrolschemeScheme
    {
        get
        {
            if (m_GamepadcontrolschemeSchemeIndex == -1) m_GamepadcontrolschemeSchemeIndex = asset.FindControlSchemeIndex("Gamepad control scheme");
            return asset.controlSchemes[m_GamepadcontrolschemeSchemeIndex];
        }
    }
    private int m_KeyboardcontrolschemeSchemeIndex = -1;
    public InputControlScheme KeyboardcontrolschemeScheme
    {
        get
        {
            if (m_KeyboardcontrolschemeSchemeIndex == -1) m_KeyboardcontrolschemeSchemeIndex = asset.FindControlSchemeIndex("Keyboard control scheme");
            return asset.controlSchemes[m_KeyboardcontrolschemeSchemeIndex];
        }
    }
    private int m_MousecontrolschemeSchemeIndex = -1;
    public InputControlScheme MousecontrolschemeScheme
    {
        get
        {
            if (m_MousecontrolschemeSchemeIndex == -1) m_MousecontrolschemeSchemeIndex = asset.FindControlSchemeIndex("Mouse control scheme");
            return asset.controlSchemes[m_MousecontrolschemeSchemeIndex];
        }
    }
    public interface IGamepadActions
    {
        void OnStickLeft(InputAction.CallbackContext context);
        void OnDPad(InputAction.CallbackContext context);
        void OnStickRight(InputAction.CallbackContext context);
        void OnShoulder(InputAction.CallbackContext context);
        void OnTrigger(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnStart(InputAction.CallbackContext context);
    }
}
