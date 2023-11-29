using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    // Animation trigger for opening and closing the container
    private const string OPEN_CLOSE = "OpenClose";

    // Reference to the associated ContainerCounter script
    [SerializeField] private ContainerCounter containerCounter;

    // Animator component for controlling animations
    private Animator animator;

    // Called when the script is awakened
    private void Awake()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();
    }

    // Called when the script starts
    private void Start()
    {
        // Subscribe to the event triggered when the player grabs an object from the container
        containerCounter.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObject;
    }

    // Event handler for the player grabbing an object from the container
    private void ContainerCounter_OnPlayerGrabbedObject(object sender, EventArgs e)
    {
        // Trigger the animation for opening and closing the container
        animator.SetTrigger(OPEN_CLOSE);
    }
}
