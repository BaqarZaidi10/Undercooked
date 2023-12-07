using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GordonRamsey : MonoBehaviour
{
    public static GordonRamsey instance;
    private Coroutine currentState;

    public float collideWait = 5f, rawWait = 3f, burntWait = 3f, trashWait = 3f, dropWait = 3f;
    private float punishmentTime = 0f;
    private float playerSpeed;

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
        ChangeState(RAMSEY_STATE.PATROLLING);
    }

    public void ChangeState(RAMSEY_STATE STATE, Transform target)
    {
        CURRENT_STATE = STATE;
        GordonRamseyBT.instance.PauseTree();

        if (currentState != null)
            StopCoroutine(currentState);

        if (target)
        {
            switch (STATE)
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
            }
        }

        else
            ChangeState(RAMSEY_STATE.PATROLLING);
    }

    public void ChangeState(RAMSEY_STATE STATE)
    {
        CURRENT_STATE = STATE;

        if (currentState != null)
            StopCoroutine(currentState);

        switch(STATE)
        {
            case RAMSEY_STATE.COLLIDING:
                currentState = StartCoroutine(Collision());
                break;
            case RAMSEY_STATE.PATROLLING:
                currentState = StartCoroutine(Patrolling());
                break;
        }
    }

    private IEnumerator DroppedFoodPunishment(Transform target)
    {
        RamseySoundManager.instance.PlayFoodDroppedSound();
        playerSpeed = target.GetComponent<PlayerController>().movementSpeed;
        target.GetComponent<PlayerController>().movementSpeed = 0;

        while (punishmentTime < RamseySoundManager.instance.dropped.length)
        {
            punishmentTime += Time.deltaTime;
            yield return null;
        }
        punishmentTime = 0f;

        target.GetComponent<PlayerController>().movementSpeed = playerSpeed;
        GordonRamseyBT.instance.AttackCooldown(dropWait);

        ChangeState(RAMSEY_STATE.PATROLLING);
    }
    
    private IEnumerator TrashedFoodPunishment(Transform target)
    {
        RamseySoundManager.instance.PlayTrashSound();
        playerSpeed = target.GetComponent<PlayerController>().movementSpeed;
        target.GetComponent<PlayerController>().movementSpeed = 0;

        while (punishmentTime < RamseySoundManager.instance.trash.length)
        {
            punishmentTime += Time.deltaTime;
            yield return null;
        }
        punishmentTime = 0f;

        target.GetComponent<PlayerController>().movementSpeed = playerSpeed; 
        GordonRamseyBT.instance.AttackCooldown(trashWait);

        ChangeState(RAMSEY_STATE.PATROLLING);
    }
    
    private IEnumerator BurntFoodPunishment(Transform target)
    {
        RamseySoundManager.instance.PlayBurntSound();
        playerSpeed = target.GetComponent<PlayerController>().movementSpeed;
        target.GetComponent<PlayerController>().movementSpeed = 0;

        float punishmentTime = 0f;
        while (punishmentTime < RamseySoundManager.instance.burnt.length)
        {
            punishmentTime += Time.deltaTime;
            yield return null;
        }

        punishmentTime = 0f;
        target.GetComponent<PlayerController>().movementSpeed = playerSpeed;
        GordonRamseyBT.instance.AttackCooldown(burntWait);

        ChangeState(RAMSEY_STATE.PATROLLING);
    }
    
    private IEnumerator RawFoodPunishment(Transform target)
    {
        RamseySoundManager.instance.PlayRawSound();
        playerSpeed = target.GetComponent<PlayerController>().movementSpeed;
        target.GetComponent<PlayerController>().movementSpeed = 0;

        float punishmentTime = 0f;
        while (punishmentTime < RamseySoundManager.instance.raw.length)
        {
            punishmentTime += Time.deltaTime;
            yield return null;
        }

        punishmentTime = 0f;
        target.GetComponent<PlayerController>().movementSpeed = playerSpeed; 
        GordonRamseyBT.instance.AttackCooldown(rawWait);

        ChangeState(RAMSEY_STATE.PATROLLING);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player") && canCollide && CURRENT_STATE == RAMSEY_STATE.PATROLLING)
        {
            GordonRamseyBT.instance.PauseTree();
            RamseySoundManager.instance.PlayCollideSound(true);
            StartCoroutine(Collision());
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.CompareTag("Player") && !canCollide && CURRENT_STATE == RAMSEY_STATE.PATROLLING)
        {            
            RamseySoundManager.instance.PlayCollideSound(false);
        }
    }

    private IEnumerator Collision()
    {
        canCollide = false;
        yield return new WaitForSeconds(collideWait);
        canCollide = true;
        ChangeState(RAMSEY_STATE.PATROLLING);
    }
    
    private IEnumerator Patrolling()
    {
        GordonRamseyBT.instance.ResumeTree();
        yield return null;
    }
}
