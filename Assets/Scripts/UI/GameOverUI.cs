using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    // Reference to the text displaying the number of recipes delivered
    [SerializeField] private TextMeshProUGUI scoreText; 
    [SerializeField] private TextMeshProUGUI playerText; 

    public Color p1Color, p2Color, drawColor;

    private void Start()
    {
        // Subscribe to the game state changed event
        GameManager_.Instance.OnStateChanged += GameManager_OnStateChanged;

        // Initially hide the game over UI
        Hide();
    }

    private void Update()
    {
        if(Input.anyKey)
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        }
    }

    // Event handler for the game state changed event
    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        // Check if the game is in the "Game Over" state
        if (GameManager_.Instance.IsGameOver())
        {
            // Show the game over UI
            Show();
            // Update the recipesDeliveredText with the number of successful recipes delivered
            if(ScoreUI.instance.p1Score > ScoreUI.instance.p2Score)
            {
                scoreText.text = ScoreUI.instance.ScoreText(ScoreUI.instance.p1Score);
                playerText.color = p1Color;
                playerText.text = "PLAYER 1 WINS!";
            }
            else if(ScoreUI.instance.p1Score < ScoreUI.instance.p2Score)
            {
                scoreText.text = ScoreUI.instance.ScoreText(ScoreUI.instance.p2Score);
                playerText.color = p2Color;
                playerText.text = "PLAYER 2 WINS!";
            }
            else
            {
                scoreText.text = ScoreUI.instance.ScoreText(ScoreUI.instance.p1Score);
                playerText.color = drawColor;
                playerText.text = "DRAW";
            }
        }
        else
        {
            // Hide the game over UI if the game is not in the "Game Over" state
            Hide();
        }
    }

    // Method to show the game over UI
    private void Show()
    {
        gameObject.SetActive(true);
        //scoreUI.SetActive(false);
    }

    // Method to hide the game over UI
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
