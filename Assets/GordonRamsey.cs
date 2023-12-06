using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GordonRamsey : MonoBehaviour
{
    public static GordonRamsey instance;
    private Coroutine currentState;

    public float collideWait = 5f, rawWait = 3f, burntWait = 3f, trashWait = 3f, dropWait = 3f;
    private float punishmentTime = 0f;

    private bool canCollide = true;

    public enum RAMSEY_STATE
    {
        PATROLLING,
        FOOD_DROPPED,
        FOOD_BURNT,
        FOOD_RAW,
        FOOD_TRASH,
        COLLIDING
    }
    public RAMSEY_STATE CURRENT_STATE = RAMSEY_STATE.PATROLLING;

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
        GordonRamseyBT.instance.ResumeTree();
    }

    public void ChangeState(RAMSEY_STATE STATE, Transform target)
    {
        CURRENT_STATE = STATE;
        StopCoroutine(currentState);
        switch(STATE)
        {
            case RAMSEY_STATE.FOOD_DROPPED:
                    currentState = StartCoroutine(DroppedFoodPunishment(target));
                break;
            case RAMSEY_STATE.FOOD_BURNT:
                currentState = StartCoroutine(BurntFoodPunishment(target));
                break;
            case RAMSEY_STATE.FOOD_RAW:
                currentState = StartCoroutine(RawFoodPunishment(target));
                break;
            case RAMSEY_STATE.FOOD_TRASH:
                currentState = StartCoroutine(TrashedFoodPunishment(target));
                break; 
            case RAMSEY_STATE.COLLIDING:
               // currentState = StartCoroutine(Collision(target));
                break;
        }
    }

    private IEnumerator DroppedFoodPunishment(Transform target)
    {
        GordonRamseyBT.instance.PauseTree();
        RamseySoundManager.instance.PlayFoodDroppedSound();
        target.GetComponent<PlayerController>().enabled = false;

        while (punishmentTime < RamseySoundManager.instance.audioSource.clip.length)
        {
            punishmentTime += Time.deltaTime;
            yield return null;
        }
        punishmentTime = 0f;

        target.GetComponent<PlayerController>().enabled = true; 
        GordonRamseyBT.instance.ResumeTree();
        GordonRamseyBT.instance.AttackCooldown(dropWait);

        CURRENT_STATE = RAMSEY_STATE.PATROLLING;
    }
    
    private IEnumerator TrashedFoodPunishment(Transform target)
    {
        GordonRamseyBT.instance.PauseTree();
        RamseySoundManager.instance.PlayTrashSound();
        target.GetComponent<PlayerController>().enabled = false;

        while (punishmentTime < RamseySoundManager.instance.audioSource.clip.length)
        {
            punishmentTime += Time.deltaTime;
            yield return null;
        }
        punishmentTime = 0f;

        target.GetComponent<PlayerController>().enabled = true; 
        GordonRamseyBT.instance.ResumeTree();
        GordonRamseyBT.instance.AttackCooldown(trashWait);

        CURRENT_STATE = RAMSEY_STATE.PATROLLING;
    }
    
    private IEnumerator BurntFoodPunishment(Transform target)
    {
        GordonRamseyBT.instance.PauseTree();
        RamseySoundManager.instance.PlayBurntSound();
        target.GetComponent<PlayerController>().enabled = false;

        float punishmentTime = 0f;
        while (punishmentTime < RamseySoundManager.instance.audioSource.clip.length)
        {
            punishmentTime += Time.deltaTime;
            yield return null;
        }

        punishmentTime = 0f;
        target.GetComponent<PlayerController>().enabled = true; 
        GordonRamseyBT.instance.ResumeTree();
        GordonRamseyBT.instance.AttackCooldown(burntWait);

        CURRENT_STATE = RAMSEY_STATE.PATROLLING;
    }
    
    private IEnumerator RawFoodPunishment(Transform target)
    {
        GordonRamseyBT.instance.PauseTree();
        RamseySoundManager.instance.PlayRawSound();
        target.GetComponent<PlayerController>().enabled = false;

        float punishmentTime = 0f;
        while (punishmentTime < RamseySoundManager.instance.audioSource.clip.length)
        {
            punishmentTime += Time.deltaTime;
            yield return null;
        }

        punishmentTime = 0f;
        target.GetComponent<PlayerController>().enabled = true; 
        GordonRamseyBT.instance.ResumeTree();
        GordonRamseyBT.instance.AttackCooldown(rawWait);

        CURRENT_STATE = RAMSEY_STATE.PATROLLING;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player") && canCollide)
        {
            GordonRamseyBT.instance.PauseTree();
            RamseySoundManager.instance.PlayCollideSound(true);
            StartCoroutine(Collision(collideWait));
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.CompareTag("Player") && !canCollide)
        {            
            RamseySoundManager.instance.PlayCollideSound(false);
            GordonRamseyBT.instance.ResumeTree();
        }
    }

    private IEnumerator Collision(float collisionCooldown)
    {
        canCollide = false;
        yield return new WaitForSeconds(collisionCooldown);
        canCollide = true;
        CURRENT_STATE = RAMSEY_STATE.PATROLLING;
    }
}
