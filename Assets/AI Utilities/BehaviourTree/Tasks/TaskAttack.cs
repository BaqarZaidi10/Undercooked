using BehaviourTree;
using UnityEngine;

public class TaskAttack : Node
{
    private Transform lastTarget;

    private float attackTime = 1f;
    private float attackCounter = 0f;

    public TaskAttack(Transform transform)
    {

    }

    public override NODESTATE Evaluate()
    {
        Transform target = (Transform)GetData("target");

        if(target != lastTarget)
        {
            lastTarget = target;
        }

        attackCounter += Time.deltaTime;
        if(attackCounter >= attackTime)
        {
            bool enemyIsDead = true;// = NPC_AI_Manager.TakeHit();
            if(enemyIsDead)
            {
                ClearData("target");
            }
            else
            {
                attackCounter = 0f;
            }
        }

        Debug.Log("attacking");
        state = NODESTATE.RUNNING;
        return state;
    }
}
