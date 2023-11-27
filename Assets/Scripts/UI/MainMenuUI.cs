using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    // UI buttons for play and quit
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Add a listener to the play button to load the GameScene when clicked
        playButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.GameScene);
        });

        // Add a listener to the quit button to close the application when clicked
        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });

        // Ensure the game is not paused when returning to the main menu
        Time.timeScale = 1f;
    }
}
