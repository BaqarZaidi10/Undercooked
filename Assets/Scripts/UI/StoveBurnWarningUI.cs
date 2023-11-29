using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour
{
    // Reference to the StoveCounter script
    [SerializeField] private StoveCounter stoveCounter;

    private void Start()
    {
        // Subscribe to the OnProgressChanged event of the StoveCounter
        stoveCounter.OnProgressChanged += stoveCounter_OnProgressChanged;

        // Hide the warning UI initially
        Hide();
    }

    private void stoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        // Define a threshold progress amount to show the burn warning
        float burnShowProgressAmount = .5f;

        // Check if the stove is in the fried state and progress is beyond the threshold
        bool show = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;

        // Show or hide the warning UI based on the conditions
        if (show)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        // Set the GameObject active to show the warning UI
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        // Set the GameObject inactive to hide the warning UI
        gameObject.SetActive(false);
    }
}
