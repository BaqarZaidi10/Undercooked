using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{

    // Player preferences key for storing the music volume
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

    // Singleton pattern to ensure only one instance of MusicManager exists
    public static MusicManager Instance { get; private set; }

    // Reference to the AudioSource component
    private AudioSource audioSource;

    // Initial volume for the music
    private float volume = .3f;

    // Called when the script instance is being loaded
    private void Awake()
    {
        Instance = this; // Set the singleton instance
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component

        // Load the music volume from player preferences or use a default value
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, .3f);
        audioSource.volume = volume; // Set the volume of the AudioSource
    }

    // Method to change the volume of the music
    public void ChangeVolume()
    {
        volume += .1f; // Increase the volume by 0.1
        if (volume > 1f)
        {
            volume = 0f; // Reset to 0 if the volume exceeds 1
        }
        audioSource.volume = volume; // Set the new volume to the AudioSource

        // Save the new volume to player preferences
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save(); // Save the player preferences
    }

    // Get the current volume of the music
    public float GetVolume()
    {
        return volume;
    }
}
