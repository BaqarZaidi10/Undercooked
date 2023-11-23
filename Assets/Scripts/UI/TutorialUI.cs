using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour {


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


    private void Start() {
        GameInputP1.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        UpdateVisual();

        Show();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (KitchenGameManager.Instance.IsCountdownToStartActive()) {
            Hide();
        }
    }

    private void GameInput_OnBindingRebind(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        keyMoveUpText.text = GameInputP1.Instance.GetBindingText(GameInputP1.Binding.Move_Up);
        keyMoveDownText.text = GameInputP1.Instance.GetBindingText(GameInputP1.Binding.Move_Down);
        keyMoveLeftText.text = GameInputP1.Instance.GetBindingText(GameInputP1.Binding.Move_Left);
        keyMoveRightText.text = GameInputP1.Instance.GetBindingText(GameInputP1.Binding.Move_Right);
        keyInteractText.text = GameInputP1.Instance.GetBindingText(GameInputP1.Binding.Interact);
        keyInteractAlternateText.text = GameInputP1.Instance.GetBindingText(GameInputP1.Binding.InteractAlternate);
        keyPauseText.text = GameInputP1.Instance.GetBindingText(GameInputP1.Binding.Pause);
        keyGamepadInteractText.text = GameInputP1.Instance.GetBindingText(GameInputP1.Binding.Gamepad_Interact);
        keyGamepadInteractAlternateText.text = GameInputP1.Instance.GetBindingText(GameInputP1.Binding.Gamepad_InteractAlternate);
        keyGamepadPauseText.text = GameInputP1.Instance.GetBindingText(GameInputP1.Binding.Gamepad_Pause);
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}