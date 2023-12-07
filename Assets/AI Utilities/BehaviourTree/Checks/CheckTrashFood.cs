using BehaviourTree;
using Unity.VisualScripting;
using UnityEngine;

public class CheckTrashFood : Node
{
    private Transform transform; 
    private LayerMask enemyLayer = 1 << 7;
    private LayerMask foodLayer = 1 << 9;

    public CheckTrashFood(Transform transform)
    {
        this.transform = transform;
    }

    public override NODESTATE Evaluate()
    {
        object t = GetData("Ttarget");

        if (t == null)
        {
            if(GordonRamseyBT.trasher)
            {
                parent.parent.SetData("Ttarget", GordonRamseyBT.trasher);

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
