using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    // UI buttons for play and quit
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Add a listener to the playButton's click event
        playButton.onClick.AddListener(() =>
        {
            // Load the LobbyScene when the playButton is clicked
            Loader.Load(Loader.Scene.LobbyScene);
        });

        // Add a listener to the quitButton's click event
        quitButton.onClick.AddListener(() =>
        {
            // Quit the application when the quitButton is clicked
            Application.Quit();
        });

        // Set the time scale to 1, ensuring normal time flow
        Time.timeScale = 1f;
    }
}
