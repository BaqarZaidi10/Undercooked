using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour {


    public static OptionsUI Instance { get; private set; }


    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAlternateButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button gamepadInteractButton;
    [SerializeField] private Button gamepadInteractAlternateButton;
    [SerializeField] private Button gamepadPauseButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAlternateText;
    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private TextMeshProUGUI gamepadInteractText;
    [SerializeField] private TextMeshProUGUI gamepadInteractAlternateText;
    [SerializeField] private TextMeshProUGUI gamepadPauseText;
    [SerializeField] private Transform pressToRebindKeyTransform;


    private Action onCloseButtonAction;


    private void Awake() {
        Instance = this;

        soundEffectsButton.onClick.AddListener(() => {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() => {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() => {
            Hide();
            onCloseButtonAction();
        });

        moveUpButton.onClick.AddListener(() => { RebindBinding(GameInputP1.Binding.Move_Up); });
        moveDownButton.onClick.AddListener(() => { RebindBinding(GameInputP1.Binding.Move_Down); });
        moveLeftButton.onClick.AddListener(() => { RebindBinding(GameInputP1.Binding.Move_Left); });
        moveRightButton.onClick.AddListener(() => { RebindBinding(GameInputP1.Binding.Move_Right); });
        interactButton.onClick.AddListener(() => { RebindBinding(GameInputP1.Binding.Interact); });
        interactAlternateButton.onClick.AddListener(() => { RebindBinding(GameInputP1.Binding.InteractAlternate); });
        pauseButton.onClick.AddListener(() => { RebindBinding(GameInputP1.Binding.Pause); });
        gamepadInteractButton.onClick.AddListener(() => { RebindBinding(GameInputP1.Binding.Gamepad_Interact); });
        gamepadInteractAlternateButton.onClick.AddListener(() => { RebindBinding(GameInputP1.Binding.Gamepad_InteractAlternate); });
        gamepadPauseButton.onClick.AddListener(() => { RebindBinding(GameInputP1.Binding.Gamepad_Pause); });
    }

    private void Start() {
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;

        UpdateVisual();

        HidePressToRebindKey();
        Hide();
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e) {
        Hide();
    }

    private void UpdateVisual() {
        soundEffectsText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);

        moveUpText.text = GameInputP1.Instance.GetBindingText(GameInputP1.Binding.Move_Up);
        moveDownText.text = GameInputP1.Instance.GetBindingText(GameInputP1.Binding.Move_Down);
        moveLeftText.text = GameInputP1.Instance.GetBindingText(GameInputP1.Binding.Move_Left);
        moveRightText.text = GameInputP1.Instance.GetBindingText(GameInputP1.Binding.Move_Right);
        interactText.text = GameInputP1.Instance.GetBindingText(GameInputP1.Binding.Interact);
        interactAlternateText.text = GameInputP1.Instance.GetBindingText(GameInputP1.Binding.InteractAlternate);
        pauseText.text = GameInputP1.Instance.GetBindingText(GameInputP1.Binding.Pause);
        gamepadInteractText.text = GameInputP1.Instance.GetBindingText(GameInputP1.Binding.Gamepad_Interact);
        gamepadInteractAlternateText.text = GameInputP1.Instance.GetBindingText(GameInputP1.Binding.Gamepad_InteractAlternate);
        gamepadPauseText.text = GameInputP1.Instance.GetBindingText(GameInputP1.Binding.Gamepad_Pause);
    }

    public void Show(Action onCloseButtonAction) {
        this.onCloseButtonAction = onCloseButtonAction;

        gameObject.SetActive(true);

        soundEffectsButton.Select();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKey() {
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }

    private void HidePressToRebindKey() {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInputP1.Binding binding) {
        ShowPressToRebindKey();
        GameInputP1.Instance.RebindBinding(binding, () => {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }

}
