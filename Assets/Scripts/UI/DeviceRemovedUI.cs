using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DeviceRemovedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI deviceRemovedText; // Reference to the text displaying the removed device's name
    [SerializeField] private Button mainMenuButton; // Reference to the main menu button
    [SerializeField] private Button removePlayerButton; // Reference to the remove player button

    public event EventHandler OnRemovePlayer; // Event triggered when the remove player button is clicked

    private void Awake()
    {
        // Set up button click event listeners
        mainMenuButton.onClick.AddListener(() => Loader.Load(Loader.Scene.MainMenuScene));
        removePlayerButton.onClick.AddListener(RemovePlayer);
    }

    private void Start()
    {
        // Subscribe to game events
        GameManager_.Instance.OnDeviceLost += GameManager_OnDeviceLost;
        GameManager_.Instance.OnStateChanged += GameManager_OnStateChanged;

        // Initially hide the device removed UI
        Hide();
    }

    // Event handler for the game state changed event
    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        // Check if the game is in the countdown to restart state
        if (GameManager_.Instance.IsCountdownToRestartActive())
        {
            // Hide the device removed UI
            Hide();
        }
    }

    // Event handler for the device lost event
    private void GameManager_OnDeviceLost(object sender, GameManager_.EventArgsOnDeviceLost e)
    {
        // Show the device removed UI and update the displayed device name
        Show();
        deviceRemovedText.text = e.deviceName;
    }

    // Method to invoke the OnRemovePlayer event
    private void RemovePlayer()
    {
        OnRemovePlayer?.Invoke(this, EventArgs.Empty);
    }

    // Method to show the device removed UI
    private void Show()
    {
        gameObject.SetActive(true);
    }

    // Method to hide the device removed UI
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
