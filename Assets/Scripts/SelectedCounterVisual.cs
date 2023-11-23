using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{

    // Reference to the associated BaseCounter
    [SerializeField] private BaseCounter baseCounter;

    // Array of visual GameObjects to control visibility
    [SerializeField] private GameObject[] visualGameObjectArray;

    // Subscribe to the player's OnSelectedCounterChanged event when the script starts
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    // Event handler for the player's OnSelectedCounterChanged event
    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        // Check if the selected counter matches the associated BaseCounter
        if (e.selectedCounter == baseCounter)
        {
            // If matched, show the visual elements
            Show();
        }
        else
        {
            // If not matched, hide the visual elements
            Hide();
        }
    }

    // Show the visual elements by setting them active
    private void Show()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(true);
        }
    }

    // Hide the visual elements by setting them inactive
    private void Hide()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }
}
