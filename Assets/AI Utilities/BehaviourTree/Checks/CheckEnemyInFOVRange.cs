using BehaviourTree;
using UnityEngine;

public class CheckEnemyInFOVRange : Node
{
    private Transform transform;
    private LayerMask enemyLayer = 1 << 7;
    private LayerMask foodLayer = 1 << 9;

    public CheckEnemyInFOVRange(Transform transform)
    {
        this.transform = transform;
    }

    public override NODESTATE Evaluate()
    {
        object t = GetData("target");

        if(t == null)
        {
            Collider[] enemyColliders = Physics.OverlapSphere(transform.position + (Vector3.forward * GuardBT.fovRange), GuardBT.fovRange, enemyLayer);
            Collider[] foodColliders = Physics.OverlapSphere(transform.position + (Vector3.forward * GuardBT.fovRange), GuardBT.fovRange, foodLayer);

            if(foodColliders.Length > 0)
            {
                foreach(Collider f in foodColliders)
                {
                    if(f.transform.position.y - GameObject.Find("Floor").transform.position.y < 0.5f)
                    {
                        if (enemyColliders.Length > 0)
                        {
                            parent.parent.SetData("target", enemyColliders[0].transform);

                            state = NODESTATE.SUCCESS;
                            return state;
                        }
                    }
                }
            }
            

            state = NODESTATE.FAILURE;
            return state;            
        }

        Debug.Log("Checking fov");
        state = NODESTATE.SUCCESS;
        return state;
    } 
}
