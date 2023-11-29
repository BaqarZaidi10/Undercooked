using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    // Reference to the BaseCounter associated with this visual
    [SerializeField] private BaseCounter baseCounter;

    // Array of visual game objects to control visibility
    [SerializeField] private GameObject[] visualGameObjectArray;

    private void Start()
    {
        // Commented out since PlayerController.Instance.OnSelectedCounterChanged is currently commented

        // Subscribe to the event when this script starts
        // PlayerController.Instance.OnSelectedCounterChanged += PlayerController_OnSelectedCounterChanged;
    }

    // Event handler for the OnSelectedCounterChanged event
    private void PlayerController_OnSelectedCounterChanged(object sender, PlayerController.OnSelectedCounterChangedEventArgs e)
    {
        // Check if the selected counter matches the associated BaseCounter
        if (e.selectedCounter == baseCounter)
        {
            // If yes, show the visuals
            Show();
        }
        else
        {
            // If not, hide the visuals
            Hide();
        }
    }

    // Method to show the visual game objects
    private void Show()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(true);
        }
    }

    // Method to hide the visual game objects
    private void Hide()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }
}
