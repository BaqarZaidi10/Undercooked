using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GamePausedUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton; // Reference to the resume button
    [SerializeField] private Button mainMenuButton; // Reference to the main menu button
    [SerializeField] private Button optionsButton; // Reference to the options button

    private void Awake()
    {
        // Add a listener to the main menu button to load the main menu scene
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        // Add a listener to the resume button to toggle the game pause menu
        resumeButton.onClick.AddListener(() =>
        {
            GameManager_.Instance.TogglePauseMenu();
        });

        // Add a listener to the options button to hide the current UI and show the options UI
        optionsButton.onClick.AddListener(() =>
        {
            Hide();
            OptionsUI.Instance.Show(Show);
        });
    }

    private void Start()
    {
        // Subscribe to the game paused and unpaused events
        GameManager_.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager_.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;

        // Initially hide the game paused UI
        Hide();
    }

    // Method to show the game paused UI
    private void Show()
    {
        gameObject.SetActive(true);
        resumeButton.Select(); // Set focus on the resume button
    }

    // Method to hide the game paused UI
    private void Hide()
    {
        gameObject.SetActive(false);
    }

    // Event handler for the game paused event
    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Show(); // Show the game paused UI when the game is paused
    }

    // Event handler for the game unpaused event
    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide(); // Hide the game paused UI when the game is unpaused
    }
}
