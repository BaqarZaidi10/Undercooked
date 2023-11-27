// Required using directives for Unity classes and data structures
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This attribute makes it possible to create instances of this scriptable object in the Unity Editor
[CreateAssetMenu()]
public class AudioClipRefsSO : ScriptableObject
{

    // Array of audio clips for chopping sounds
    public AudioClip[] chop;

    // Array of audio clips for delivery failure sounds
    public AudioClip[] deliveryFail;

    // Array of audio clips for delivery success sounds
    public AudioClip[] deliverySuccess;

    // Array of audio clips for footstep sounds
    public AudioClip[] footstep;

    // Array of audio clips for object dropping sounds
    public AudioClip[] objectDrop;

    // Array of audio clips for object pickup sounds
    public AudioClip[] objectPickup;

    // Single audio clip for stove sizzling sound
    public AudioClip stoveSizzle;

    // Array of audio clips for trash-related sounds
    public AudioClip[] trash;

    // Array of audio clips for warning sounds
    public AudioClip[] warning;
}
