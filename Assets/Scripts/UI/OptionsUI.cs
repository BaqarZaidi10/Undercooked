using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System;

public class OptionsUI : MonoBehaviour
{
    // Singleton instance
    public static OptionsUI Instance { get; private set; }

    // UI elements
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAltText;
    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private TextMeshProUGUI moveUpArrowsText;
    [SerializeField] private TextMeshProUGUI moveDownArrowsText;
    [SerializeField] private TextMeshProUGUI moveLeftArrowsText;
    [SerializeField] private TextMeshProUGUI moveRightArrowsText;
    [SerializeField] private TextMeshProUGUI interactArrowsText;
    [SerializeField] private TextMeshProUGUI interactAltArrowsText;
    [SerializeField] private TextMeshProUGUI pauseArrowsText;
    [SerializeField] private TextMeshProUGUI gamepadInteractText;
    [SerializeField] private TextMeshProUGUI gamepadInteractAltText;
    [SerializeField] private TextMeshProUGUI gamepadPauseText;

    [SerializeField] private Button moveUpWASDButton;
    [SerializeField] private Button moveDownWASDButton;
    [SerializeField] private Button moveLeftWASDButton;
    [SerializeField] private Button moveRightWASDButton;
    [SerializeField] private Button interactWASDButton;
    [SerializeField] private Button interactAltWASDButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button moveUpArrowsButton;
    [SerializeField] private Button moveDownArrowsButton;
    [SerializeField] private Button moveLeftArrowsButton;
    [SerializeField] private Button moveRightArrowsButton;
    [SerializeField] private Button interactArrowsButton;
    [SerializeField] private Button interactAltArrowsButton;
    [SerializeField] private Button pauseArrowsButton;
    [SerializeField] private Button gamepadInteractButton;
    [SerializeField] private Button gamepadInteractAltButton;
    [SerializeField] private Button gamepadPauseButton;
    [SerializeField] private Transform pressToRebindKeyTransform;

    // Action to be performed when the close button is clicked
    private Action onCloseButtonAction;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Set up the singleton instance
        Instance = this;

        // Add listeners for button clicks
        soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        closeButton.onClick.AddListener(() =>
        {
            Hide();
            onCloseButtonAction();
        });

        moveUpWASDButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Up_WASD); });
        moveDownWASDButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Down_WASD); });
        moveLeftWASDButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Left_WASD); });
        moveRightWASDButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Right_WASD); });
        interactWASDButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact_WASD); });
        interactAltWASDButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact_Alt_WASD); });
        pauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Pause); });

        moveUpArrowsButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Up_Arrows); });
        moveDownArrowsButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Down_Arrows); });
        moveLeftArrowsButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Left_Arrows); });
        moveRightArrowsButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Right_Arrows); });
        interactArrowsButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact_Arrows); });
        interactAltArrowsButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact_Alt_Arrows); });
        pauseArrowsButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Pause_Alt_Arrows); });

        gamepadInteractButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_Interact); });
        gamepadInteractAltButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_Interact_Alternate); });
        gamepadPauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_Pause); });
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Add listener for the game unpaused event
        GameManager_.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;

        // Update the visual elements and hide the UI
        UpdateVisual();
        Hide();
        HidePressToRebindKey();
    }

    // Event handler for the game unpaused event
    private void GameManager_OnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    // Update the visual elements based on current settings
    private void UpdateVisual()
    {
        soundEffectsText.text = "Sound effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f).ToString();
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f).ToString();

        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up_WASD);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down_WASD);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left_WASD);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right_WASD);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact_WASD);
        interactAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact_Alt_WASD);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);

        moveUpArrowsText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up_Arrows);
        moveDownArrowsText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down_Arrows);
        moveLeftArrowsText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left_Arrows);
        moveRightArrowsText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right_Arrows);
        interactArrowsText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact_Arrows);
        interactAltArrowsText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact_Alt_Arrows);
        pauseArrowsText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause_Alt_Arrows);

        gamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
        gamepadInteractAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact_Alternate);
        gamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);
    }

    // Show the UI with a specified action to be performed when closed
    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;
        gameObject.SetActive(true);

        soundEffectsButton.Select();
    }

    // Hide the UI
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    // Show the "Press to Rebind Key" message
    private void ShowPressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }

    // Hide the "Press to Rebind Key" message
    private void HidePressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }

    // Rebind a specific input binding
    private void RebindBinding(GameInput.Binding binding)
    {
        // Show the "Press to Rebind Key" message
        ShowPressToRebindKey();

        // Initiate the rebind process for the specified binding
        GameInput.Instance.RebindBinding(binding, () =>
        {
            // Hide the "Press to Rebind Key" message
            HidePressToRebindKey();

            // Update the visual elements after rebind
            UpdateVisual();
        });
    }
}
