using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{

    // Reference to the Player script attached to the same GameObject
    private Player player;

    // Timer to control the frequency of footstep sounds
    private float footstepTimer;

    // Maximum time between playing footstep sounds
    private float footstepTimerMax = .1f;

    // Called when the script instance is being loaded
    private void Awake()
    {
        player = GetComponent<Player>(); // Get the Player component attached to the same GameObject
    }

    // Update is called once per frame
    private void Update()
    {
        // Decrement the footstep timer
        footstepTimer -= Time.deltaTime;

        // Check if it's time to play a footstep sound
        if (footstepTimer < 0f)
        {
            footstepTimer = footstepTimerMax; // Reset the timer

            // Check if the player is currently walking
            if (player.IsWalking())
            {
                float volume = 1f; // Set the volume for the footstep sound
                SoundManager.Instance.PlayFootstepsSound(player.transform.position, volume); // Play the footstep sound
            }
        }
    }
}
