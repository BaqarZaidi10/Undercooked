using BehaviourTree;
using UnityEngine;

public class CheckEnemyInAttackRange : Node
{
    private Transform transform;

    public CheckEnemyInAttackRange(Transform transform)
    {
        this.transform = transform;
    }

    public override NODESTATE Evaluate()
    {
        object t = GetData("target");

        if (t == null)
        {
            state = NODESTATE.FAILURE;
            return state;
        }

        Transform target = (Transform)t;

        if(Vector2.Distance(transform.position, target.position) <= GuardBT.attackRange)
        {
            state = NODESTATE.SUCCESS;
            return state;
        }

        state = NODESTATE.FAILURE;
        return state;
    }
}
