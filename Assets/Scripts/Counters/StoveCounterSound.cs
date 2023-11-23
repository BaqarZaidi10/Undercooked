using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{

    // Reference to the associated StoveCounter
    [SerializeField] private StoveCounter stoveCounter;

    // Audio source to play sounds
    private AudioSource audioSource;

    // Timer for playing warning sound
    private float warningSoundTimer;

    // Flag to indicate whether to play the warning sound
    private bool playWarningSound;

    private void Awake()
    {
        // Get the AudioSource component on this GameObject
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // Subscribe to the StoveCounter's events
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    // Event handler for progress changes in the StoveCounter
    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        // Threshold for showing warning based on progress
        float burnShowProgressAmount = .5f;

        // Set the flag to play the warning sound if the stove is fried and progress is sufficient
        playWarningSound = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;
    }

    // Event handler for state changes in the StoveCounter
    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        // Play the stove sound when the state changes to frying or fried
        bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        if (playSound)
        {
            audioSource.Play();
        }
        else
        {
            // Pause the audio source if not frying or fried
            audioSource.Pause();
        }
    }

    private void Update()
    {
        // Check if the warning sound should be played
        if (playWarningSound)
        {
            // Decrement the timer
            warningSoundTimer -= Time.deltaTime;

            // If the timer is up, play the warning sound and reset the timer
            if (warningSoundTimer <= 0f)
            {
                float warningSoundTimerMax = .2f;
                warningSoundTimer = warningSoundTimerMax;

                // Play the warning sound at the stove's position
                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            }
        }
    }
}
