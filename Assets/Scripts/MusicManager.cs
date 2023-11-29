using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Constant for PlayerPrefs key for storing music volume
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

    // Static instance to make this class accessible from other scripts
    public static MusicManager Instance { get; private set; }

    // Reference to the AudioSource component
    private AudioSource audioSource;

    // Initial volume value
    private float volume = .3f;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Singleton pattern to ensure there is only one instance of this class
        Instance = this;

        // Get the AudioSource component attached to the same GameObject as this script
        audioSource = GetComponent<AudioSource>();

        // Set the initial volume from PlayerPrefs or use a default value
        float defaultVolume = 1f;
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, defaultVolume);

        // Apply the volume to the AudioSource component
        audioSource.volume = volume;
    }

    // Method to change the music volume
    public void ChangeVolume()
    {
        // Increase volume by 0.1f
        volume += .1f;

        // If volume exceeds 1, set it back to 0
        if (volume > 1f)
        {
            volume = 0f;
        }

        // Apply the new volume to the AudioSource component
        audioSource.volume = volume;

        // Save the volume preference using PlayerPrefs
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    // Method to get the current music volume
    public float GetVolume()
    {
        return volume;
    }
}
