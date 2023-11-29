using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    // References to TextMeshProUGUI elements for different key bindings
    [SerializeField] private TextMeshProUGUI keyMoveUpText;
    [SerializeField] private TextMeshProUGUI keyMoveDownText;
    [SerializeField] private TextMeshProUGUI keyMoveLeftText;
    [SerializeField] private TextMeshProUGUI keyMoveRightText;
    [SerializeField] private TextMeshProUGUI keyInteractText;
    [SerializeField] private TextMeshProUGUI keyInteractAlternateText;
    [SerializeField] private TextMeshProUGUI keyPauseText;
    [SerializeField] private TextMeshProUGUI keyGamepadInteractText;
    [SerializeField] private TextMeshProUGUI keyGamepadInteractAlternateText;
    [SerializeField] private TextMeshProUGUI keyGamepadPauseText;

    private void Start()
    {
        // Subscribe to events when the game starts
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        GameManager_.Instance.OnStateChanged += GameManager_OnStateChanged;

        // Update UI visuals based on current key bindings
        UpdateVisual();

        // Show the UI initially
        Show();
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        // Hide UI when the game is in countdown state
        if (GameManager_.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void GameInput_OnBindingRebind(object sender, EventArgs e)
    {
        // Update UI visuals when key bindings are rebound
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        // Update UI elements with the current key bindings
        keyMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up_WASD);
        keyMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down_WASD);
        keyMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left_WASD);
        keyMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right_WASD);
        keyInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact_WASD);
        keyInteractAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact_Alt_WASD);
        keyPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        keyGamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
        keyGamepadInteractAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact_Alternate);
        keyGamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);
    }

    private void Show()
    {
        // Set the GameObject active to show the UI
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        // Set the GameObject inactive to hide the UI
        gameObject.SetActive(false);
    }
}
