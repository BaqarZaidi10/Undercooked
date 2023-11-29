using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnFlashingBarUI : MonoBehaviour
{
    // Animator parameter name for the flashing animation
    private const string IS_FLASHING = "IsFlashing";

    // Reference to the StoveCounter script
    [SerializeField] private StoveCounter stoveCounter;

    // Animator component for controlling animations
    private Animator animator;

    private void Awake()
    {
        // Get the Animator component on the GameObject
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Subscribe to the OnProgressChanged event of the StoveCounter
        stoveCounter.OnProgressChanged += stoveCounter_OnProgressChanged;

        // Set the initial state of the flashing animation to false
        animator.SetBool(IS_FLASHING, false);
    }

    private void stoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        // Define a threshold progress amount to show the flashing
        float burnShowProgressAmount = .5f;

        // Check if the stove is in the fried state and progress is beyond the threshold
        bool show = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;

        // Set the animator parameter to control the flashing animation
        animator.SetBool(IS_FLASHING, show);
    }
}
