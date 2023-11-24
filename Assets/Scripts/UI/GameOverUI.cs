using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI recipesDeliveredText;

    private void Start()
    {
        // Subscribe to the game state change event
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        // Hide the game over UI on start
        Hide();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        // Check if the game is in the "Game Over" state
        if (KitchenGameManager.Instance.IsGameOver())
        {
            // Show the game over UI
            Show();

            // Update the text to display the number of successful recipes delivered
            recipesDeliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipesAmount().ToString();
        }
        else
        {
            // Hide the game over UI if the game state is not "Game Over"
            Hide();
        }
    }

    private void Show()
    {
        // Activate the UI game object
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        // Deactivate the UI game object
        gameObject.SetActive(false);
    }
}
