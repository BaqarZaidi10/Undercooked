using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsButton;

    private void Awake()
    {
        // Set up button click listeners
        resumeButton.onClick.AddListener(() => {
            KitchenGameManager.Instance.TogglePauseGame();
        });
        mainMenuButton.onClick.AddListener(() => {
            // Load the main menu scene when the main menu button is clicked
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        optionsButton.onClick.AddListener(() => {
            // Hide the pause UI, show the options UI, and pass a callback to show the pause UI back after closing options
            Hide();
            OptionsUI.Instance.Show(Show);
        });
    }

    private void Start()
    {
        // Subscribe to game pause and unpause events
        KitchenGameManager.Instance.OnGamePaused += KitchenGameManager_OnGamePaused;
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;

        // Hide the pause UI on start
        Hide();
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        // Hide the pause UI when the game is unpaused
        Hide();
    }

    private void KitchenGameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        // Show the pause UI when the game is paused
        Show();
    }

    private void Show()
    {
        // Activate the UI game object and set the resume button as selected
        gameObject.SetActive(true);
        resumeButton.Select();
    }

    private void Hide()
    {
        // Deactivate the UI game object
        gameObject.SetActive(false);
    }
}
