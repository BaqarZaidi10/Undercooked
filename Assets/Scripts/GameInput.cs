using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    // Singleton instance
    public static GameInput Instance { get; private set; }

    // Player preferences key for input bindings
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    // Event for primary interact action
    public event EventHandler<OnInteractActionEventArgs> OnInteractAction;
    public class OnInteractActionEventArgs : EventArgs
    {
        public InputAction.CallbackContext action;
    }

    // Event for alternate interact action
    public event EventHandler<OnInteractAlternateActionEventArgs> OnInteractAlternateAction;
    public class OnInteractAlternateActionEventArgs : EventArgs
    {
        public InputAction.CallbackContext action;
    }

    // Event for pause action
    public event EventHandler OnPauseAction;

    // Event for binding rebind
    public event EventHandler OnBindingRebind;

    // Enumeration of possible input bindings
    public enum Binding
    {
        // Keyboard WASD bindings
        Move_Up_WASD,
        Move_Down_WASD,
        Move_Left_WASD,
        Move_Right_WASD,
        Interact_WASD,
        Interact_Alt_WASD,
        // Keyboard Arrow bindings
        Move_Up_Arrows,
        Move_Down_Arrows,
        Move_Left_Arrows,
        Move_Right_Arrows,
        Interact_Arrows,
        Interact_Alt_Arrows,
        Pause,
        Pause_Alt_Arrows,
        // Gamepad bindings
        Gamepad_Interact,
        Gamepad_Interact_Alternate,
        Gamepad_Pause
    }

    // Enumeration of control schemes
    public enum ControlSchemes
    {
        Keyboard_WASD,
        Keyboard_Arrows,
        Gamepad
    }

    // Default player input actions
    private static PlayerInputActions defaultPlayerInputActions;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        Instance = this;

        defaultPlayerInputActions = new PlayerInputActions();

        // Load saved binding preferences
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            defaultPlayerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }
    }

    // Initialize player input actions
    public void InitializePlayerInputActions(PlayerInputActions playerInputActions)
    {
        LoadPlayerPrefs(playerInputActions);
        SubscribeToInputActions(playerInputActions);
    }

    // Load player preferences for input actions
    private void LoadPlayerPrefs(PlayerInputActions playerInputActions)
    {
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }
    }

    // Subscribe to input actions events
    private void SubscribeToInputActions(PlayerInputActions playerInputActions)
    {
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_Performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_Performed;
        playerInputActions.Player.Pause.performed += Pause_Performed;
    }

    // Destroy player input actions and unsubscribe from events
    public void DestroyPlayerInputActions(PlayerInputActions playerInputActions)
    {
        UnsubscribeToInputActions(playerInputActions);
    }

    // Unsubscribe from input actions events
    private void UnsubscribeToInputActions(PlayerInputActions playerInputActions)
    {
        playerInputActions.Player.Interact.performed -= Interact_Performed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_Performed;
        playerInputActions.Player.Pause.performed -= Pause_Performed;

        playerInputActions.Dispose();
    }

    // Event handler for pause action
    private void Pause_Performed(InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    // Event handler for alternate interact action
    private void InteractAlternate_Performed(InputAction.CallbackContext action)
    {
        OnInteractAlternateAction?.Invoke(this, new OnInteractAlternateActionEventArgs
        {
            action = action
        });
    }

    // Event handler for primary interact action
    private void Interact_Performed(UnityEngine.InputSystem.InputAction.CallbackContext action)
    {
        OnInteractAction?.Invoke(this, new OnInteractActionEventArgs
        {
            action = action
        });
    }

    // Get normalized movement vector from player input actions
    public Vector2 GetMovementVectorNormalized(PlayerInputActions playerInputActions)
    {
        Vector2 inputVector2 = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector2 = inputVector2.normalized;

        return inputVector2;
    }

    // Check if the given action belongs to the player input actions
    public bool isActionMine(InputAction.CallbackContext obj, PlayerInputActions playerInputActions)
    {
        return (playerInputActions.Contains(obj.action));
    }

    // Get the default player input actions
    public PlayerInputActions GetDefaultPlayerInputActions()
    {
        return defaultPlayerInputActions;
    }

    #region Binding

    // Get display string for a specific binding
    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Interact_WASD:
                return (defaultPlayerInputActions.Player.Interact.bindings[0].ToDisplayString());
            case Binding.Interact_Alt_WASD:
                return (defaultPlayerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString());
            case Binding.Pause:
                return (defaultPlayerInputActions.Player.Pause.bindings[0].ToDisplayString());
            case Binding.Pause_Alt_Arrows:
                return (defaultPlayerInputActions.Player.Pause.bindings[2].ToDisplayString());
            case Binding.Move_Up_WASD:
                return (defaultPlayerInputActions.Player.Move.bindings[1].ToDisplayString());
            case Binding.Move_Down_WASD:
                return (defaultPlayerInputActions.Player.Move.bindings[2].ToDisplayString());
            case Binding.Move_Left_WASD:
                return (defaultPlayerInputActions.Player.Move.bindings[3].ToDisplayString());
            case Binding.Move_Right_WASD:
                return (defaultPlayerInputActions.Player.Move.bindings[4].ToDisplayString());
            case Binding.Interact_Arrows:
                return (defaultPlayerInputActions.Player.Interact.bindings[1].ToDisplayString());
            case Binding.Interact_Alt_Arrows:
                return (defaultPlayerInputActions.Player.InteractAlternate.bindings[1].ToDisplayString());
            case Binding.Move_Up_Arrows:
                return (defaultPlayerInputActions.Player.Move.bindings[6].ToDisplayString());
            case Binding.Move_Down_Arrows:
                return defaultPlayerInputActions.Player.Move.bindings[7].ToDisplayString();
            case Binding.Move_Left_Arrows:
                return defaultPlayerInputActions.Player.Move.bindings[8].ToDisplayString();
            case Binding.Move_Right_Arrows:
                return defaultPlayerInputActions.Player.Move.bindings[9].ToDisplayString();
            case Binding.Gamepad_Interact:
                return defaultPlayerInputActions.Player.Interact.bindings[2].ToDisplayString();
            case Binding.Gamepad_Interact_Alternate:
                return defaultPlayerInputActions.Player.InteractAlternate.bindings[2].ToDisplayString();
            case Binding.Gamepad_Pause:
                return defaultPlayerInputActions.Player.Pause.bindings[1].ToDisplayString();
        }
    }

    // Rebind a specific binding
    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        // Disable player input
        defaultPlayerInputActions.Player.Disable();

        // Get input action and binding index for the specified binding
        InputAction inputAction;
        int bindingIndex;
        switch (binding)
        {
            default:
            case Binding.Move_Up_WASD:
                inputAction = defaultPlayerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down_WASD:
                inputAction = defaultPlayerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left_WASD:
                inputAction = defaultPlayerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right_WASD:
                inputAction = defaultPlayerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact_WASD:
                inputAction = defaultPlayerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.Interact_Alt_WASD:
                inputAction = defaultPlayerInputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = defaultPlayerInputActions.Player.Pause;
                bindingIndex = 0;
                break;
            case Binding.Pause_Alt_Arrows:
                inputAction = defaultPlayerInputActions.Player.Pause;
                bindingIndex = 2;
                break;
            case Binding.Move_Up_Arrows:
                inputAction = defaultPlayerInputActions.Player.Move;
                bindingIndex = 6;
                break;
            case Binding.Move_Down_Arrows:
                inputAction = defaultPlayerInputActions.Player.Move;
                bindingIndex = 7;
                break;
            case Binding.Move_Left_Arrows:
                inputAction = defaultPlayerInputActions.Player.Move;
                bindingIndex = 8;
                break;
            case Binding.Move_Right_Arrows:
                inputAction = defaultPlayerInputActions.Player.Move;
                bindingIndex = 9;
                break;
            case Binding.Interact_Arrows:
                inputAction = defaultPlayerInputActions.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.Interact_Alt_Arrows:
                inputAction = defaultPlayerInputActions.Player.InteractAlternate;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_Interact:
                inputAction = defaultPlayerInputActions.Player.Interact;
                bindingIndex = 2;
                break;
            case Binding.Gamepad_Interact_Alternate:
                inputAction = defaultPlayerInputActions.Player.InteractAlternate;
                bindingIndex = 2;
                break;
            case Binding.Gamepad_Pause:
                inputAction = defaultPlayerInputActions.Player.Pause;
                bindingIndex = 1;
                break;
        }

        // Start interactive rebinding
        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                // Dispose of the callback
                callback.Dispose();

                // Enable default player input
                defaultPlayerInputActions.Player.Enable();

                // Invoke the rebound action
                onActionRebound();

                // Save the new binding overrides to player preferences
                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, defaultPlayerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                // Reload bindings on rebind
                ReloadBindingsOnRebind();

                // Invoke the binding rebind event
                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            })
            .Start();
    }

    // Reload bindings on rebind for all control schemes
    private void ReloadBindingsOnRebind()
    {
        LoadPlayerPrefs(defaultPlayerInputActions);

        foreach (ControlSchemeParameters controlSchemeParameters in GameControlsManager.Instance.GetAllControlSchemeParameters())
        {
            if (controlSchemeParameters.playerInputActions != null)
            {
                LoadPlayerPrefs(controlSchemeParameters.playerInputActions);
            }
        }
    }

    #endregion
}
