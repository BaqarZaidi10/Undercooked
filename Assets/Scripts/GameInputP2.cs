using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputP2 : MonoBehaviour 
{
    private const string Player2_PREFS_BINDINGS = "InputBindings";

    public static GameInputP2 Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;

    public enum Binding {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternate,
        Pause,
        Gamepad_Interact,
        Gamepad_InteractAlternate,
        Gamepad_Pause
    }

    private Player2InputActions Player2InputActions;

    private void Awake() 
    {
        Instance = this;

        Player2InputActions = new Player2InputActions();

        if (PlayerPrefs.HasKey(Player2_PREFS_BINDINGS)) 
        {
            Player2InputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(Player2_PREFS_BINDINGS));
        }

        Player2InputActions.Player2.Enable();

        Player2InputActions.Player2.Interact.performed += Interact_performed;
        Player2InputActions.Player2.InteractAlternate.performed += InteractAlternate_performed;
        Player2InputActions.Player2.Pause.performed += Pause_performed;
    }

    private void OnDestroy() 
    {
        Player2InputActions.Player2.Interact.performed -= Interact_performed;
        Player2InputActions.Player2.InteractAlternate.performed -= InteractAlternate_performed;
        Player2InputActions.Player2.Pause.performed -= Pause_performed;

        Player2InputActions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) 
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) 
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) 
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() 
    {
        Vector2 inputVector = Player2InputActions.Player2.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

    public string GetBindingText(Binding binding) 
    {
        switch (binding) {
            default:
            case Binding.Move_Up:
                return Player2InputActions.Player2.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return Player2InputActions.Player2.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return Player2InputActions.Player2.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return Player2InputActions.Player2.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return Player2InputActions.Player2.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return Player2InputActions.Player2.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return Player2InputActions.Player2.Pause.bindings[0].ToDisplayString();
            case Binding.Gamepad_Interact:
                return Player2InputActions.Player2.Interact.bindings[1].ToDisplayString();
            case Binding.Gamepad_InteractAlternate:
                return Player2InputActions.Player2.InteractAlternate.bindings[1].ToDisplayString();
            case Binding.Gamepad_Pause:
                return Player2InputActions.Player2.Pause.bindings[1].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound) 
    {
        Player2InputActions.Player2.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding) {
            default:
            case Binding.Move_Up:
                inputAction = Player2InputActions.Player2.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = Player2InputActions.Player2.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = Player2InputActions.Player2.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = Player2InputActions.Player2.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = Player2InputActions.Player2.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputAction = Player2InputActions.Player2.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = Player2InputActions.Player2.Pause;
                bindingIndex = 0;
                break;
            case Binding.Gamepad_Interact:
                inputAction = Player2InputActions.Player2.Interact;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_InteractAlternate:
                inputAction = Player2InputActions.Player2.InteractAlternate;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_Pause:
                inputAction = Player2InputActions.Player2.Pause;
                bindingIndex = 1;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback => {
                callback.Dispose();
                Player2InputActions.Player2.Enable();
                onActionRebound();

                PlayerPrefs.SetString(Player2_PREFS_BINDINGS, Player2InputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            })
            .Start();
    }

}