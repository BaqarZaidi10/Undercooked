using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;

    private IHasProgress hasProgress;

    private void Start()
    {
        // Get the IHasProgress component from the specified game object
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();

        // Check if the component is found
        if (hasProgress == null)
        {
            Debug.LogError("Game Object " + hasProgressGameObject + " does not have a component that implements IHasProgress!");
        }

        // Subscribe to the progress changed event
        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;

        // Initialize the fill amount to zero
        barImage.fillAmount = 0f;

        // Hide the progress bar at the start
        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        // Update the fill amount based on the normalized progress
        barImage.fillAmount = e.progressNormalized;

        // Check if the progress is at the extremes (0 or 100%) to hide the progress bar
        if (e.progressNormalized == 0f || e.progressNormalized == 1f)
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
        // Set the game object active to show the progress bar
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        // Set the game object inactive to hide the progress bar
        gameObject.SetActive(false);
    }
}
