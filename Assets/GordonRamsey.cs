using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GordonRamsey : MonoBehaviour
{
    public static GordonRamsey instance;
    private Coroutine currentState;
    private float punishmentTime = 0f;

    private void Awake()
    {
        if(instance)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        GuardBT.instance.ResumeTree();
    }

    public void FoodOnGround(Transform target)
    {       
        if(currentState != null)
            currentState = StartCoroutine(DroppedFoodPunishment(target));
    }

    private IEnumerator DroppedFoodPunishment(Transform target)
    {
        GuardBT.instance.PauseTree();
        RamseySoundManager.instance.PlayFoodDroppedSound();
        target.GetComponent<PlayerController>().enabled = false;

        while (punishmentTime < RamseySoundManager.instance.audioSource.clip.length)
        {
            punishmentTime += Time.deltaTime;
            yield return null;
        }

        punishmentTime = 0;
        target.GetComponent<PlayerController>().enabled = true; 
        GuardBT.instance.ResumeTree();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            GuardBT.instance.PauseTree();
            RamseySoundManager.instance.PlayCollideSound(true);
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {            
            RamseySoundManager.instance.PlayCollideSound(false);
            GuardBT.instance.ResumeTree();
        }
    }
}
