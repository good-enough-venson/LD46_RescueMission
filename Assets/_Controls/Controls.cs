// GENERATED AUTOMATICALLY FROM 'Assets/_Scripts/GameControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameControls"",
    ""maps"": [
        {
            ""name"": ""InGame_Actions"",
            ""id"": ""364713d2-d2bb-436e-bb63-0ec200f2558f"",
            ""actions"": [
                {
                    ""name"": ""Thrusters"",
                    ""type"": ""Value"",
                    ""id"": ""70807ff8-9f1d-4936-bb3b-c17b75e03187"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""4ac1fad2-a470-43e1-b278-c52f5505406c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AimScreen"",
                    ""type"": ""Value"",
                    ""id"": ""39f7eb3d-0f2b-4a72-a24e-6b80d2b54d49"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0d0dba97-c0cb-4557-bffd-ba9628ffd509"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""Thrusters"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""edcb0ef2-0e26-4867-8650-cfa943a62780"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""68a928a5-4bd6-4f82-b6e9-0ca72cec2b48"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""AimScreen"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Mouse"",
            ""bindingGroup"": ""Mouse"",
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
        // InGame_Actions
        m_InGame_Actions = asset.FindActionMap("InGame_Actions", throwIfNotFound: true);
        m_InGame_Actions_Thrusters = m_InGame_Actions.FindAction("Thrusters", throwIfNotFound: true);
        m_InGame_Actions_Fire = m_InGame_Actions.FindAction("Fire", throwIfNotFound: true);
        m_InGame_Actions_AimScreen = m_InGame_Actions.FindAction("AimScreen", throwIfNotFound: true);
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

    // InGame_Actions
    private readonly InputActionMap m_InGame_Actions;
    private IInGame_ActionsActions m_InGame_ActionsActionsCallbackInterface;
    private readonly InputAction m_InGame_Actions_Thrusters;
    private readonly InputAction m_InGame_Actions_Fire;
    private readonly InputAction m_InGame_Actions_AimScreen;
    public struct InGame_ActionsActions
    {
        private @Controls m_Wrapper;
        public InGame_ActionsActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Thrusters => m_Wrapper.m_InGame_Actions_Thrusters;
        public InputAction @Fire => m_Wrapper.m_InGame_Actions_Fire;
        public InputAction @AimScreen => m_Wrapper.m_InGame_Actions_AimScreen;
        public InputActionMap Get() { return m_Wrapper.m_InGame_Actions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InGame_ActionsActions set) { return set.Get(); }
        public void SetCallbacks(IInGame_ActionsActions instance)
        {
            if (m_Wrapper.m_InGame_ActionsActionsCallbackInterface != null)
            {
                @Thrusters.started -= m_Wrapper.m_InGame_ActionsActionsCallbackInterface.OnThrusters;
                @Thrusters.performed -= m_Wrapper.m_InGame_ActionsActionsCallbackInterface.OnThrusters;
                @Thrusters.canceled -= m_Wrapper.m_InGame_ActionsActionsCallbackInterface.OnThrusters;
                @Fire.started -= m_Wrapper.m_InGame_ActionsActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_InGame_ActionsActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_InGame_ActionsActionsCallbackInterface.OnFire;
                @AimScreen.started -= m_Wrapper.m_InGame_ActionsActionsCallbackInterface.OnAimScreen;
                @AimScreen.performed -= m_Wrapper.m_InGame_ActionsActionsCallbackInterface.OnAimScreen;
                @AimScreen.canceled -= m_Wrapper.m_InGame_ActionsActionsCallbackInterface.OnAimScreen;
            }
            m_Wrapper.m_InGame_ActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Thrusters.started += instance.OnThrusters;
                @Thrusters.performed += instance.OnThrusters;
                @Thrusters.canceled += instance.OnThrusters;
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
                @AimScreen.started += instance.OnAimScreen;
                @AimScreen.performed += instance.OnAimScreen;
                @AimScreen.canceled += instance.OnAimScreen;
            }
        }
    }
    public InGame_ActionsActions @InGame_Actions => new InGame_ActionsActions(this);
    private int m_MouseSchemeIndex = -1;
    public InputControlScheme MouseScheme
    {
        get
        {
            if (m_MouseSchemeIndex == -1) m_MouseSchemeIndex = asset.FindControlSchemeIndex("Mouse");
            return asset.controlSchemes[m_MouseSchemeIndex];
        }
    }
    public interface IInGame_ActionsActions
    {
        void OnThrusters(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnAimScreen(InputAction.CallbackContext context);
    }
}
