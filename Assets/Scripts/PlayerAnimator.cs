using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    // Animation parameter name for walking state in the Animator controller
    private const string IS_WALKING = "IsWalking";

    // Reference to the Animator component attached to the player GameObject
    private Animator animator;

    // Reference to the PlayerController script attached to the player GameObject
    [SerializeField] private PlayerController player;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Get the Animator component attached to the player GameObject
        animator = GetComponent<Animator>();

        // Set the "IsWalking" parameter in the Animator based on the initial state from the PlayerController
        animator.SetBool(IS_WALKING, player.IsWalking());
    }

    // Update is called once per frame
    private void Update()
    {
        // Update the "IsWalking" parameter in the Animator based on the current state from the PlayerController
        animator.SetBool(IS_WALKING, player.IsWalking());
    }
}
