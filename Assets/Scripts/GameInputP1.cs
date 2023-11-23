using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputP1 : MonoBehaviour 
{
    private const string Player1_PREFS_BINDINGS = "InputBindings";

    public static GameInputP1 Instance { get; private set; }

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

    private Player1InputActions Player1InputActions;

    private void Awake() 
    {
        Instance = this;

        Player1InputActions = new Player1InputActions();

        if (PlayerPrefs.HasKey(Player1_PREFS_BINDINGS)) 
        {
            Player1InputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(Player1_PREFS_BINDINGS));
        }

        Player1InputActions.Player.Enable();

        Player1InputActions.Player.Interact.performed += Interact_performed;
        Player1InputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        Player1InputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy() 
    {
        Player1InputActions.Player.Interact.performed -= Interact_performed;
        Player1InputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        Player1InputActions.Player.Pause.performed -= Pause_performed;

        Player1InputActions.Dispose();
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
        Vector2 inputVector = Player1InputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

    public string GetBindingText(Binding binding) 
    {
        switch (binding) {
            default:
            case Binding.Move_Up:
                return Player1InputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return Player1InputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return Player1InputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return Player1InputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return Player1InputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return Player1InputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return Player1InputActions.Player.Pause.bindings[0].ToDisplayString();
            case Binding.Gamepad_Interact:
                return Player1InputActions.Player.Interact.bindings[1].ToDisplayString();
            case Binding.Gamepad_InteractAlternate:
                return Player1InputActions.Player.InteractAlternate.bindings[1].ToDisplayString();
            case Binding.Gamepad_Pause:
                return Player1InputActions.Player.Pause.bindings[1].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound) 
    {
        Player1InputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding) {
            default:
            case Binding.Move_Up:
                inputAction = Player1InputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = Player1InputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = Player1InputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = Player1InputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = Player1InputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputAction = Player1InputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = Player1InputActions.Player.Pause;
                bindingIndex = 0;
                break;
            case Binding.Gamepad_Interact:
                inputAction = Player1InputActions.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_InteractAlternate:
                inputAction = Player1InputActions.Player.InteractAlternate;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_Pause:
                inputAction = Player1InputActions.Player.Pause;
                bindingIndex = 1;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback => {
                callback.Dispose();
                Player1InputActions.Player.Enable();
                onActionRebound();

                PlayerPrefs.SetString(Player1_PREFS_BINDINGS, Player1InputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            })
            .Start();
    }

}