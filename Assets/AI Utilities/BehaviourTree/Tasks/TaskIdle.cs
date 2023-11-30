using BehaviourTree;
using UnityEngine;

public class TaskIdle : Node
{
    private Transform transform;
    private Transform target;
    private Animator animator;


    public TaskIdle(Transform transform)
    {
        this.transform = transform;
    }

    public override NODESTATE Evaluate()
    {     
        Debug.Log("idling");     

        state = NODESTATE.RUNNING;
        return state;
    }
}
