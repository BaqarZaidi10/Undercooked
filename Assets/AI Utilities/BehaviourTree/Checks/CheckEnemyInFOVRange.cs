using BehaviourTree;
using UnityEngine;

public class CheckEnemyInFOVRange : Node
{
    private Transform transform;
    private LayerMask enemyLayer = 1 << 7;

    public CheckEnemyInFOVRange(Transform transform)
    {
        this.transform = transform;
    }

    public override NODESTATE Evaluate()
    {
        object t = GetData("target");

        if(t == null)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, GuardBT.fovRange, enemyLayer);

            if(colliders.Length > 0)
            {
                parent.parent.SetData("target", colliders[0].transform);

                state = NODESTATE.SUCCESS;
                return state;
            } 

            state = NODESTATE.FAILURE;
            return state;            
        }

        state = NODESTATE.SUCCESS;
        return state;
    } 
}
