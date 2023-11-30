using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    // Reference to the text displaying the number of recipes delivered
    [SerializeField] private TextMeshProUGUI recipesDeliveredText; 
    [SerializeField] private TextMeshProUGUI playerText; 
    [SerializeField] private GameObject scoreUI; 

    private void Start()
    {
        scoreUI = GameObject.Find("ScoreUI");
        playerText = GameObject.Find("PlayerText").GetComponent<TextMeshProUGUI>();
        // Subscribe to the game state changed event
        GameManager_.Instance.OnStateChanged += GameManager_OnStateChanged;

        // Initially hide the game over UI
        Hide();
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
            recipesDeliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipesAmount().ToString();
            if(ScoreUI.instance.p1Wins > ScoreUI.instance.p2Wins)
            {
                playerText.text = "1";
            }
            else if(ScoreUI.instance.p1Wins < ScoreUI.instance.p2Wins)
            {
                playerText.text = "2";
            }
            else
            {
                if (ScoreUI.instance.p1Fails > ScoreUI.instance.p2Fails)
                {
                    playerText.text = "2";
                }
                else if (ScoreUI.instance.p1Fails < ScoreUI.instance.p2Fails)
                {
                    playerText.text = "1";
                }
                else
                {
                    playerText.text = "3";
                }
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
        scoreUI.SetActive(false);
    }

    // Method to hide the game over UI
    private void Hide()
    {
        gameObject.SetActive(false);
        scoreUI.SetActive(true);
    }
}
