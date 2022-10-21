//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/InputSystem/PlayerControls.inputactions
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
            ""name"": ""Movement"",
            ""id"": ""e04b497b-ee17-4a1a-85f2-950c46c5e94b"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""970fd6d6-5dca-444b-aa95-616e4d63dd2e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""1971ef7a-6528-4839-a24b-af7a8ba1c096"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""58537b1b-6d86-43c6-98a6-bb549b825a34"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": ""Controller"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""ec3c14cf-6b33-4cfd-989a-63760afd34c9"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""15bffe61-a573-4af5-bb4a-21ececcff596"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""40e266dc-f83f-44cc-9a85-71427fe7bb89"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""1dbf3347-9a00-4745-ad2c-e52f2a21fdb5"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b7fa1e41-481a-44f1-8f25-c3c4af871421"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""9689990e-ea66-4fbb-a249-6ca860ad1c12"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4712eb39-e4a8-4a3f-acb5-5fae5400b98d"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""SummonClone"",
            ""id"": ""bbc6c3f2-d607-4eb2-99fd-0cecae19451c"",
            ""actions"": [
                {
                    ""name"": ""SummonAClone"",
                    ""type"": ""Button"",
                    ""id"": ""b47245b7-7c97-4efa-824b-15b350482538"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ExitClone"",
                    ""type"": ""Button"",
                    ""id"": ""51bf1880-6f7c-45f3-95ff-795a4aa0975a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SwitchPlaces"",
                    ""type"": ""Button"",
                    ""id"": ""1a910fba-8948-4d7a-bf70-ac9b8d82eb93"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a491595f-d6d6-4175-8c0a-9d8ed7293972"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""SummonAClone"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e26a3248-314e-43fa-9b6e-44ec44441afb"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""SummonAClone"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5afeecf5-7b4f-4c16-acc4-f73d943f9d3a"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ExitClone"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5f09fbe0-ab2d-4d0a-9494-a1f97996bb56"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ExitClone"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e6418500-4128-41a3-9de9-929c42a69c1a"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""SwitchPlaces"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f8603aad-e67b-4a7e-883c-d96e6324ccd1"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""SwitchPlaces"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PauseMenu"",
            ""id"": ""0e5b5e9c-05c9-4d6f-9163-184dbc39e3a7"",
            ""actions"": [
                {
                    ""name"": ""PauseGame"",
                    ""type"": ""Button"",
                    ""id"": ""32c4207e-b0f6-46a1-b93e-07cfd3a23ce5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e3f81ea7-b382-4c97-a67c-bdaea04b41ca"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""PauseGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""50a888dd-334f-4da4-a3be-725d790d7e6c"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""PauseGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Grapple"",
            ""id"": ""7d6cb877-56dc-4baa-bb17-7bb236aa586a"",
            ""actions"": [
                {
                    ""name"": ""ShootHook"",
                    ""type"": ""Button"",
                    ""id"": ""d8fe9231-f463-4da0-afac-847d15a63dd3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CancelHook"",
                    ""type"": ""Button"",
                    ""id"": ""5d6c3782-5563-4d9a-921c-f027647a9caf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ExtendGrapple"",
                    ""type"": ""Button"",
                    ""id"": ""1fac779a-09f2-42f6-9d54-6c32a882dafc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d13403ba-52e8-42d9-93f1-467b72b4a8e2"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ShootHook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6c87cb85-8e1c-4d5c-864b-617008bbcd7b"",
                    ""path"": ""<XInputController>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ShootHook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c91f6ab7-e094-429b-9764-12a55c42223d"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""CancelHook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fc941a43-8eec-40e3-95d7-d2441862c689"",
                    ""path"": ""<XInputController>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""CancelHook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""87ccda8c-9d5e-4036-bcf4-01b27a5d090e"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ExtendGrapple"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5dc5281a-7ae4-4ad1-870d-7ba7993735bd"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ExtendGrapple"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Push"",
            ""id"": ""cab184f3-6cc4-4c29-9dbb-045a557e2b46"",
            ""actions"": [
                {
                    ""name"": ""Push"",
                    ""type"": ""Button"",
                    ""id"": ""a0913a19-c5f3-48a5-bb17-1e76467c5eaf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0d161618-a1fc-4297-87da-a4e1e2697c51"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Push"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Controller"",
            ""bindingGroup"": ""Controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Movement
        m_Movement = asset.FindActionMap("Movement", throwIfNotFound: true);
        m_Movement_Move = m_Movement.FindAction("Move", throwIfNotFound: true);
        m_Movement_Jump = m_Movement.FindAction("Jump", throwIfNotFound: true);
        // SummonClone
        m_SummonClone = asset.FindActionMap("SummonClone", throwIfNotFound: true);
        m_SummonClone_SummonAClone = m_SummonClone.FindAction("SummonAClone", throwIfNotFound: true);
        m_SummonClone_ExitClone = m_SummonClone.FindAction("ExitClone", throwIfNotFound: true);
        m_SummonClone_SwitchPlaces = m_SummonClone.FindAction("SwitchPlaces", throwIfNotFound: true);
        // PauseMenu
        m_PauseMenu = asset.FindActionMap("PauseMenu", throwIfNotFound: true);
        m_PauseMenu_PauseGame = m_PauseMenu.FindAction("PauseGame", throwIfNotFound: true);
        // Grapple
        m_Grapple = asset.FindActionMap("Grapple", throwIfNotFound: true);
        m_Grapple_ShootHook = m_Grapple.FindAction("ShootHook", throwIfNotFound: true);
        m_Grapple_CancelHook = m_Grapple.FindAction("CancelHook", throwIfNotFound: true);
        m_Grapple_ExtendGrapple = m_Grapple.FindAction("ExtendGrapple", throwIfNotFound: true);
        // Push
        m_Push = asset.FindActionMap("Push", throwIfNotFound: true);
        m_Push_Push = m_Push.FindAction("Push", throwIfNotFound: true);
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

    // Movement
    private readonly InputActionMap m_Movement;
    private IMovementActions m_MovementActionsCallbackInterface;
    private readonly InputAction m_Movement_Move;
    private readonly InputAction m_Movement_Jump;
    public struct MovementActions
    {
        private @PlayerControls m_Wrapper;
        public MovementActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Movement_Move;
        public InputAction @Jump => m_Wrapper.m_Movement_Jump;
        public InputActionMap Get() { return m_Wrapper.m_Movement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
        public void SetCallbacks(IMovementActions instance)
        {
            if (m_Wrapper.m_MovementActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnJump;
            }
            m_Wrapper.m_MovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
            }
        }
    }
    public MovementActions @Movement => new MovementActions(this);

    // SummonClone
    private readonly InputActionMap m_SummonClone;
    private ISummonCloneActions m_SummonCloneActionsCallbackInterface;
    private readonly InputAction m_SummonClone_SummonAClone;
    private readonly InputAction m_SummonClone_ExitClone;
    private readonly InputAction m_SummonClone_SwitchPlaces;
    public struct SummonCloneActions
    {
        private @PlayerControls m_Wrapper;
        public SummonCloneActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @SummonAClone => m_Wrapper.m_SummonClone_SummonAClone;
        public InputAction @ExitClone => m_Wrapper.m_SummonClone_ExitClone;
        public InputAction @SwitchPlaces => m_Wrapper.m_SummonClone_SwitchPlaces;
        public InputActionMap Get() { return m_Wrapper.m_SummonClone; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SummonCloneActions set) { return set.Get(); }
        public void SetCallbacks(ISummonCloneActions instance)
        {
            if (m_Wrapper.m_SummonCloneActionsCallbackInterface != null)
            {
                @SummonAClone.started -= m_Wrapper.m_SummonCloneActionsCallbackInterface.OnSummonAClone;
                @SummonAClone.performed -= m_Wrapper.m_SummonCloneActionsCallbackInterface.OnSummonAClone;
                @SummonAClone.canceled -= m_Wrapper.m_SummonCloneActionsCallbackInterface.OnSummonAClone;
                @ExitClone.started -= m_Wrapper.m_SummonCloneActionsCallbackInterface.OnExitClone;
                @ExitClone.performed -= m_Wrapper.m_SummonCloneActionsCallbackInterface.OnExitClone;
                @ExitClone.canceled -= m_Wrapper.m_SummonCloneActionsCallbackInterface.OnExitClone;
                @SwitchPlaces.started -= m_Wrapper.m_SummonCloneActionsCallbackInterface.OnSwitchPlaces;
                @SwitchPlaces.performed -= m_Wrapper.m_SummonCloneActionsCallbackInterface.OnSwitchPlaces;
                @SwitchPlaces.canceled -= m_Wrapper.m_SummonCloneActionsCallbackInterface.OnSwitchPlaces;
            }
            m_Wrapper.m_SummonCloneActionsCallbackInterface = instance;
            if (instance != null)
            {
                @SummonAClone.started += instance.OnSummonAClone;
                @SummonAClone.performed += instance.OnSummonAClone;
                @SummonAClone.canceled += instance.OnSummonAClone;
                @ExitClone.started += instance.OnExitClone;
                @ExitClone.performed += instance.OnExitClone;
                @ExitClone.canceled += instance.OnExitClone;
                @SwitchPlaces.started += instance.OnSwitchPlaces;
                @SwitchPlaces.performed += instance.OnSwitchPlaces;
                @SwitchPlaces.canceled += instance.OnSwitchPlaces;
            }
        }
    }
    public SummonCloneActions @SummonClone => new SummonCloneActions(this);

    // PauseMenu
    private readonly InputActionMap m_PauseMenu;
    private IPauseMenuActions m_PauseMenuActionsCallbackInterface;
    private readonly InputAction m_PauseMenu_PauseGame;
    public struct PauseMenuActions
    {
        private @PlayerControls m_Wrapper;
        public PauseMenuActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @PauseGame => m_Wrapper.m_PauseMenu_PauseGame;
        public InputActionMap Get() { return m_Wrapper.m_PauseMenu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PauseMenuActions set) { return set.Get(); }
        public void SetCallbacks(IPauseMenuActions instance)
        {
            if (m_Wrapper.m_PauseMenuActionsCallbackInterface != null)
            {
                @PauseGame.started -= m_Wrapper.m_PauseMenuActionsCallbackInterface.OnPauseGame;
                @PauseGame.performed -= m_Wrapper.m_PauseMenuActionsCallbackInterface.OnPauseGame;
                @PauseGame.canceled -= m_Wrapper.m_PauseMenuActionsCallbackInterface.OnPauseGame;
            }
            m_Wrapper.m_PauseMenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                @PauseGame.started += instance.OnPauseGame;
                @PauseGame.performed += instance.OnPauseGame;
                @PauseGame.canceled += instance.OnPauseGame;
            }
        }
    }
    public PauseMenuActions @PauseMenu => new PauseMenuActions(this);

    // Grapple
    private readonly InputActionMap m_Grapple;
    private IGrappleActions m_GrappleActionsCallbackInterface;
    private readonly InputAction m_Grapple_ShootHook;
    private readonly InputAction m_Grapple_CancelHook;
    private readonly InputAction m_Grapple_ExtendGrapple;
    public struct GrappleActions
    {
        private @PlayerControls m_Wrapper;
        public GrappleActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @ShootHook => m_Wrapper.m_Grapple_ShootHook;
        public InputAction @CancelHook => m_Wrapper.m_Grapple_CancelHook;
        public InputAction @ExtendGrapple => m_Wrapper.m_Grapple_ExtendGrapple;
        public InputActionMap Get() { return m_Wrapper.m_Grapple; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GrappleActions set) { return set.Get(); }
        public void SetCallbacks(IGrappleActions instance)
        {
            if (m_Wrapper.m_GrappleActionsCallbackInterface != null)
            {
                @ShootHook.started -= m_Wrapper.m_GrappleActionsCallbackInterface.OnShootHook;
                @ShootHook.performed -= m_Wrapper.m_GrappleActionsCallbackInterface.OnShootHook;
                @ShootHook.canceled -= m_Wrapper.m_GrappleActionsCallbackInterface.OnShootHook;
                @CancelHook.started -= m_Wrapper.m_GrappleActionsCallbackInterface.OnCancelHook;
                @CancelHook.performed -= m_Wrapper.m_GrappleActionsCallbackInterface.OnCancelHook;
                @CancelHook.canceled -= m_Wrapper.m_GrappleActionsCallbackInterface.OnCancelHook;
                @ExtendGrapple.started -= m_Wrapper.m_GrappleActionsCallbackInterface.OnExtendGrapple;
                @ExtendGrapple.performed -= m_Wrapper.m_GrappleActionsCallbackInterface.OnExtendGrapple;
                @ExtendGrapple.canceled -= m_Wrapper.m_GrappleActionsCallbackInterface.OnExtendGrapple;
            }
            m_Wrapper.m_GrappleActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ShootHook.started += instance.OnShootHook;
                @ShootHook.performed += instance.OnShootHook;
                @ShootHook.canceled += instance.OnShootHook;
                @CancelHook.started += instance.OnCancelHook;
                @CancelHook.performed += instance.OnCancelHook;
                @CancelHook.canceled += instance.OnCancelHook;
                @ExtendGrapple.started += instance.OnExtendGrapple;
                @ExtendGrapple.performed += instance.OnExtendGrapple;
                @ExtendGrapple.canceled += instance.OnExtendGrapple;
            }
        }
    }
    public GrappleActions @Grapple => new GrappleActions(this);

    // Push
    private readonly InputActionMap m_Push;
    private IPushActions m_PushActionsCallbackInterface;
    private readonly InputAction m_Push_Push;
    public struct PushActions
    {
        private @PlayerControls m_Wrapper;
        public PushActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Push => m_Wrapper.m_Push_Push;
        public InputActionMap Get() { return m_Wrapper.m_Push; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PushActions set) { return set.Get(); }
        public void SetCallbacks(IPushActions instance)
        {
            if (m_Wrapper.m_PushActionsCallbackInterface != null)
            {
                @Push.started -= m_Wrapper.m_PushActionsCallbackInterface.OnPush;
                @Push.performed -= m_Wrapper.m_PushActionsCallbackInterface.OnPush;
                @Push.canceled -= m_Wrapper.m_PushActionsCallbackInterface.OnPush;
            }
            m_Wrapper.m_PushActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Push.started += instance.OnPush;
                @Push.performed += instance.OnPush;
                @Push.canceled += instance.OnPush;
            }
        }
    }
    public PushActions @Push => new PushActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_ControllerSchemeIndex = -1;
    public InputControlScheme ControllerScheme
    {
        get
        {
            if (m_ControllerSchemeIndex == -1) m_ControllerSchemeIndex = asset.FindControlSchemeIndex("Controller");
            return asset.controlSchemes[m_ControllerSchemeIndex];
        }
    }
    public interface IMovementActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
    }
    public interface ISummonCloneActions
    {
        void OnSummonAClone(InputAction.CallbackContext context);
        void OnExitClone(InputAction.CallbackContext context);
        void OnSwitchPlaces(InputAction.CallbackContext context);
    }
    public interface IPauseMenuActions
    {
        void OnPauseGame(InputAction.CallbackContext context);
    }
    public interface IGrappleActions
    {
        void OnShootHook(InputAction.CallbackContext context);
        void OnCancelHook(InputAction.CallbackContext context);
        void OnExtendGrapple(InputAction.CallbackContext context);
    }
    public interface IPushActions
    {
        void OnPush(InputAction.CallbackContext context);
    }
}