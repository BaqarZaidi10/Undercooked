using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamseySoundManager : MonoBehaviour
{
    public static RamseySoundManager instance;
    public AudioSource audioSource;

    public AudioClip burnt, raw, dropped, collideEnter, collideExit, trash;

    private Coroutine currentSound;

    private void Awake()
    {
        if (instance)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayBurntSound()
    {
        PlaySound(burnt);
        //audioSource.PlayOneShot(burnt);
    }

    public void PlayRawSound()
    {
        PlaySound(raw);
        //audioSource.PlayOneShot(raw);
    }

    public void PlayCollideSound(bool entered)
    {
        if (entered)
            PlaySound(collideEnter);
        //audioSource.PlayOneShot(collideEnter);
        else
            PlaySound(collideExit);
        //audioSource.PlayOneShot(collideExit);
    }

    public void PlayTrashSound()
    {
        PlaySound(trash);
        //audioSource.PlayOneShot(trash);
    }

    public void PlayFoodDroppedSound()
    {
        PlaySound(dropped);
        //audioSource.PlayOneShot(dropped);
    }

    private void PlaySound(AudioClip clip)
    {
        if(currentSound != null)        
            StopCoroutine(currentSound);
        
        currentSound = StartCoroutine(WaitForAudioClip(clip));
    }

    private IEnumerator WaitForAudioClip(AudioClip clip)
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        audioSource.PlayOneShot(clip);
    }
}