using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    // Animator parameter name for walking state
    private const string IS_WALKING = "IsWalking";

    // Reference to the Player script
    [SerializeField] private Player player;

    // Reference to the Animator component
    private Animator animator;

    // Awake method for initialization
    private void Awake()
    {
        // Get the Animator component on the same GameObject
        animator = GetComponent<Animator>();
    }

    // Update method to check and update the walking state in the Animator
    private void Update()
    {
        // Set the "IsWalking" parameter in the Animator based on the player's walking state
        animator.SetBool(IS_WALKING, player.IsWalking());
    }
}
