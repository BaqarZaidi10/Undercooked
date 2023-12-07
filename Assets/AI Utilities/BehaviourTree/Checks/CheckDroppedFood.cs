using BehaviourTree;
using UnityEngine;

public class CheckDroppedFood : Node
{
    private Transform transform;
    private LayerMask enemyLayer = 1 << 7;
    private LayerMask foodLayer = 1 << 9;

    public CheckDroppedFood(Transform transform)
    {
        this.transform = transform;
    }

    public override NODESTATE Evaluate()
    {
        object t = GetData("target");

        if(t == null)
        {
            Collider[] enemyColliders = Physics.OverlapSphere(transform.position + (transform.forward * GordonRamseyBT.fovRange), GordonRamseyBT.fovRange, enemyLayer);
            Collider[] foodColliders = Physics.OverlapSphere(transform.position + (transform.forward * GordonRamseyBT.fovRange), GordonRamseyBT.fovRange, foodLayer);

            if (foodColliders.Length > 0)
            {
                foreach (Collider f in foodColliders)
                {
                    if(f.gameObject.CompareTag("Slip") && f.gameObject.activeInHierarchy)
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
            //f.transform.position.y - GameObject.Find("Floor").transform.position.y < 0.5f
            state = NODESTATE.FAILURE;
            return state;            
        }

        state = NODESTATE.SUCCESS;
        return state;
    }
}
