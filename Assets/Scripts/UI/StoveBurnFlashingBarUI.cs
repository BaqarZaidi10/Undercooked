using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnFlashingBarUI : MonoBehaviour
{
    private const string IS_FLASHING = "IsFlashing";

    [SerializeField] private StoveCounter stoveCounter;
    private Animator animator;

    private void Awake()
    {
        // Cache the Animator component
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Subscribe to the stoveCounter's progress changed event
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;

        // Set the initial flashing state to false
        animator.SetBool(IS_FLASHING, false);
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        // Define a threshold to show the flashing (e.g., 50% progress)
        float burnShowProgressAmount = 0.5f;

        // Check if the stove is fried and progress is above the threshold
        bool show = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;

        // Set the flashing state based on the condition
        animator.SetBool(IS_FLASHING, show);
    }
}
