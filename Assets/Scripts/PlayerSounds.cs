using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    // Reference to the PlayerController component
    private PlayerController player;

    // Timer variables for controlling footstep sounds
    private float footStepTimer;
    private float footStepTimerMax = .1f;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Get a reference to the PlayerController component attached to the same GameObject
        player = GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Subscribe to the OnPickedSomething event in the PlayerController
        player.OnPickedSomething += PlayerController_OnPickedSomething;
    }

    // Update is called once per frame
    private void Update()
    {
        // Decrement the footstep timer
        footStepTimer -= Time.deltaTime;

        // Check if it's time to play a footstep sound
        if (footStepTimer < 0)
        {
            // Reset the timer
            footStepTimer = footStepTimerMax;

            // Check if the player is walking
            if (player.IsWalking())
            {
                // Play footsteps sound with a volume of 1
                float volume = 1f;
                SoundManager.Instance.PlayFoottepsSound(player.transform.position, volume);
            }
        }
    }

    // Event handler for the OnPickedSomething event in the PlayerController
    private void PlayerController_OnPickedSomething(object sender, EventArgs e)
    {
        // Play a picked something sound with a volume of 1
        float volume = 1f;
        SoundManager.Instance.PlayPickedSomethingSound(player.transform.position, volume);
    }
}
