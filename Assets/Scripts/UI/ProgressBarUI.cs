using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    // Reference to the GameObject that has progress
    [SerializeField] private GameObject hasProgressGameObject;

    // Reference to the Image component representing the progress bar
    [SerializeField] private Image barImage;

    // Interface for objects that have progress
    private IHasProgress hasProgress;

    private void Start()
    {
        // Get the component that implements IHasProgress from the specified GameObject
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();

        // Check if the component is found
        if (hasProgress == null)
        {
            Debug.LogError("GameObject " + hasProgressGameObject + " does not have a component that implements IHasProgress!");
        }

        // Subscribe to the OnProgressChanged event
        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;

        // Set the initial fill amount of the progress bar to 0
        barImage.fillAmount = 0f;

        // Hide the progress bar initially
        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        // Update the fill amount of the progress bar based on the normalized progress
        barImage.fillAmount = e.progressNormalized;

        // Check if the progress is at the extremes (0 or 1) to decide whether to hide or show the progress bar
        if (e.progressNormalized == 0 || e.progressNormalized == 1)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        // Activate the GameObject to show the progress bar
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        // Deactivate the GameObject to hide the progress bar
        gameObject.SetActive(false);
    }
}
