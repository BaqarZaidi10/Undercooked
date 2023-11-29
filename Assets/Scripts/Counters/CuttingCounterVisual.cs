using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    // Animator trigger parameter name for the cut animation
    private const string CUT = "Cut";

    // Reference to the associated CuttingCounter script
    [SerializeField] private CuttingCounter cuttingCounter;

    // Reference to the Animator component
    private Animator animator;

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();
    }

    // Called on the frame when a script is enabled just before any of the Update methods are called the first time
    private void Start()
    {
        // Subscribe to the OnCut event of the associated CuttingCounter
        cuttingCounter.OnCut += CuttingCounter_OnCut;
    }

    // Event handler for the OnCut event
    private void CuttingCounter_OnCut(object sender, EventArgs e)
    {
        // Set the "Cut" trigger in the Animator to play the cut animation
        animator.SetTrigger(CUT);
    }
}
