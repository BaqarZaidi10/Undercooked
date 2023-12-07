using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GordonRamsey : MonoBehaviour
{
    public static GordonRamsey instance;
    private Coroutine currentState;

    public float collideWait = 10f, rawWait = 10f, burntWait = 10f, trashWait = 10f, dropWait = 10f;
    private float AttackTime = 0f;
    private float playerSpeed = 0f;

    private bool canCollide = true;

    public enum RAMSEY_STATE
    {
        PATROLLING,
        FOOD_DROPPED,
        FOOD_BURNT,
        FOOD_RAW,
        FOOD_TRASH,
        COLLIDING,
        COOLDOWN
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
        GordonRamseyBT.instance.canAttack = true;
    }

    public void StartCooldown(float cooldown)
    {
        GordonRamseyBT.instance.canAttack = false;
        Invoke(nameof(CanAttack), cooldown);
    }

    private void CanAttack()
    {
        GordonRamseyBT.instance.canAttack = true;
    }

    public void ChangeState(RAMSEY_STATE STATE, Transform target)
    {
        CURRENT_STATE = STATE;
        StartCooldown(15f);
        GordonRamseyBT.instance.PauseTree();

        if (currentState != null)
            StopCoroutine(currentState);

        if (target)
        {
            switch (STATE)
            {
                case RAMSEY_STATE.FOOD_DROPPED:
                    currentState = StartCoroutine(DroppedFoodAttack(target));
                    break;
                case RAMSEY_STATE.FOOD_BURNT:
                    currentState = StartCoroutine(BurntFoodAttack(target));
                    break;
                case RAMSEY_STATE.FOOD_RAW:
                    currentState = StartCoroutine(RawFoodAttack(target));
                    break;
                case RAMSEY_STATE.FOOD_TRASH:
                    currentState = StartCoroutine(TrashedFoodAttack(target));
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
                //currentState = StartCoroutine(Collision());
                break;
            case RAMSEY_STATE.PATROLLING:
                currentState = StartCoroutine(Patrolling());
                break;
        }
    }

    private IEnumerator DroppedFoodAttack(Transform target)
    {
        RamseySoundManager.instance.PlayFoodDroppedSound();
        playerSpeed = target.GetComponent<PlayerController>().movementSpeed;
        target.GetComponent<PlayerController>().movementSpeed = 0;

        while (AttackTime < RamseySoundManager.instance.dropped.length)
        {
            AttackTime += Time.deltaTime;
            yield return null;
        }
        AttackTime = 0f;

        target.GetComponent<PlayerController>().movementSpeed = playerSpeed;
        //GordonRamseyBT.instance.AttackCooldown(dropWait);

        ChangeState(RAMSEY_STATE.PATROLLING);
    }
    
    private IEnumerator TrashedFoodAttack(Transform target)
    {
        RamseySoundManager.instance.PlayTrashSound();
        playerSpeed = target.GetComponent<PlayerController>().movementSpeed;
        target.GetComponent<PlayerController>().movementSpeed = 0;

        while (AttackTime < RamseySoundManager.instance.trash.length)
        {
            AttackTime += Time.deltaTime;
            yield return null;
        }
        AttackTime = 0f;

        target.GetComponent<PlayerController>().movementSpeed = playerSpeed; 
        //GordonRamseyBT.instance.AttackCooldown(trashWait);

        ChangeState(RAMSEY_STATE.PATROLLING);
    }
    
    private IEnumerator BurntFoodAttack(Transform target)
    {
        RamseySoundManager.instance.PlayBurntSound();
        playerSpeed = target.GetComponent<PlayerController>().movementSpeed;
        target.GetComponent<PlayerController>().movementSpeed = 0;

        while (AttackTime < RamseySoundManager.instance.burnt.length)
        {
            AttackTime += Time.deltaTime;
            yield return null;
        }
        AttackTime = 0f;

        target.GetComponent<PlayerController>().movementSpeed = playerSpeed;
        //GordonRamseyBT.instance.AttackCooldown(burntWait);

        ChangeState(RAMSEY_STATE.PATROLLING);
    }
    
    private IEnumerator RawFoodAttack(Transform target)
    {
        RamseySoundManager.instance.PlayRawSound();
        playerSpeed = target.GetComponent<PlayerController>().movementSpeed;
        target.GetComponent<PlayerController>().movementSpeed = 0;

        while (AttackTime < RamseySoundManager.instance.raw.length)
        {
            AttackTime += Time.deltaTime;
            yield return null;
        }
        AttackTime = 0f;

        target.GetComponent<PlayerController>().movementSpeed = playerSpeed; 
        //GordonRamseyBT.instance.AttackCooldown(rawWait);

        ChangeState(RAMSEY_STATE.PATROLLING);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player") && canCollide && CURRENT_STATE == RAMSEY_STATE.PATROLLING)
        {
            RamseySoundManager.instance.PlayCollideSound(true);
            StartCoroutine(Collision(collision.collider));
        }
    }
    
    //private void OnCollisionExit(Collision collision)
    //{
    //    if(collision.collider.CompareTag("Player") && !canCollide && CURRENT_STATE == RAMSEY_STATE.PATROLLING)
    //    {            
    //        RamseySoundManager.instance.PlayCollideSound(false);
    //    }
    //}

    private IEnumerator Collision(Collider collider)
    {
        canCollide = false;
        float timeElapsed = 0f;
        playerSpeed = collider.GetComponent<PlayerController>().movementSpeed;  
        collider.GetComponent<PlayerController>().movementSpeed = 0;
        GordonRamseyBT.instance.PauseTree();

        while(timeElapsed < RamseySoundManager.instance.collideEnter.length)
        {
            transform.LookAt(collider.transform.position);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        collider.GetComponent<PlayerController>().movementSpeed = playerSpeed;
        ChangeState(RAMSEY_STATE.PATROLLING);

        yield return new WaitForSeconds(collideWait);
        canCollide = true;
    }
    
    private IEnumerator Patrolling()
    {
        GordonRamseyBT.instance.ResumeTree();
        yield return null;
    }
}
