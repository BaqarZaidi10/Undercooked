using BehaviourTree;
using UnityEngine;

public class TaskTrashAttack : Node
{
    private Transform transform;
    private CharacterController controller;

    public TaskTrashAttack(Transform transform)
    {
        this.transform = transform;
        controller = transform.GetComponent<CharacterController>();
    }

    public override NODESTATE Evaluate()
    {
        Transform target = (Transform)GetData("Ttarget");

        if (Vector3.Distance(transform.position, target.position) < GordonRamseyBT.attackRange)
        {
            transform.LookAt(target.position);
            GordonRamsey.instance.ChangeState(GordonRamsey.RAMSEY_STATE.FOOD_TRASH, target);

            parent.parent.ClearData("Ttarget");
            state = NODESTATE.SUCCESS;
            return state;
        }

        if (Vector3.Distance(transform.position, target.position) > GordonRamseyBT.attackRange)
        {
            controller.Move((target.position - transform.position) * GordonRamseyBT.speed * Time.deltaTime);
            //transform.position = Vector3.MoveTowards(transform.position, target.position, GuardBT.speed * Time.deltaTime);
            transform.LookAt(target.position);
        }

        Debug.Log("going to target");
        state = NODESTATE.RUNNING;
        return state;
    }
}
