using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{

    // Animation trigger name for the cutting action
    private const string CUT = "Cut";

    // Reference to the associated CuttingCounter
    [SerializeField] private CuttingCounter cuttingCounter;

    // Animator component for handling animations
    private Animator animator;

    // Called when the script instance is loaded
    private void Awake()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();
    }

    // Called at the start of the script's execution
    private void Start()
    {
        // Subscribe to the OnCut event of the associated CuttingCounter
        cuttingCounter.OnCut += CuttingCounter_OnCut;
    }

    // Event handler for the OnCut event
    private void CuttingCounter_OnCut(object sender, System.EventArgs e)
    {
        // Trigger the "Cut" animation in response to a cut event
        animator.SetTrigger(CUT);
    }
}
