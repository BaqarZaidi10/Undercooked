using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{

    // Animation trigger name for opening and closing the container
    private const string OPEN_CLOSE = "OpenClose";

    // Reference to the ContainerCounter associated with this visual
    [SerializeField] private ContainerCounter containerCounter;

    // Reference to the Animator component
    private Animator animator;

    // Called when the script is first loaded
    private void Awake()
    {
        // Get the Animator component on the same GameObject
        animator = GetComponent<Animator>();
    }

    // Called at the start of the script's execution
    private void Start()
    {
        // Subscribe to the OnPlayerGrabbedObject event of the associated ContainerCounter
        containerCounter.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObject;
    }

    // Event handler for the OnPlayerGrabbedObject event
    private void ContainerCounter_OnPlayerGrabbedObject(object sender, System.EventArgs e)
    {
        // Trigger the "OpenClose" animation when the player grabs an object from the container
        animator.SetTrigger(OPEN_CLOSE);
    }
}
