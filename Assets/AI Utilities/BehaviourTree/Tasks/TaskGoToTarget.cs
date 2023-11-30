using BehaviourTree;
using UnityEngine;

public class TaskGoToTarget : Node
{
    private Transform transform;
    private Animator animator;

    public TaskGoToTarget(Transform transform)
    {
        this.transform = transform;
    }

    public override NODESTATE Evaluate()
    {
        Transform target = (Transform)GetData("target");

        if(Vector3.Distance(transform.position, target.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, GuardBT.speed * Time.deltaTime);
            transform.LookAt(target.position);
        }
        Debug.Log("going to target");
        state = NODESTATE.RUNNING; 
        return state;
    }
}
