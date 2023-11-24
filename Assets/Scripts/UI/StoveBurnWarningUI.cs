using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private void Start()
    {
        // Subscribe to the stoveCounter's progress changed event
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;

        // Hide the warning UI initially
        Hide();
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        // Define a threshold to show the warning (e.g., 50% progress)
        float burnShowProgressAmount = 0.5f;

        // Check if the stove is fried and progress is above the threshold
        bool show = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;

        // Show or hide the warning based on the condition
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
        // Activate the warning UI
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        // Deactivate the warning UI
        gameObject.SetActive(false);
    }
}
