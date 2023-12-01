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
            Collider[] colliders = Physics.OverlapSphere(transform.position, GuardBT.fovRange, enemyLayer);

            if(colliders.Length > 0)
            {
                parent.parent.SetData("target", colliders[0].transform);

                state = NODESTATE.SUCCESS;
                return state;
            } 

            state = NODESTATE.FAILURE;
            return state;            
        }

        Debug.Log("Checking fov");
        state = NODESTATE.SUCCESS;
        return state;
    } 
}
