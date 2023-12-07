using BehaviourTree;
using UnityEngine;

public class TaskTrashAttack : Node
{
    private Transform transform;
    private Animator animator;
    private CharacterController controller;

    private float punishmentTime = 3f, punishmentCooldown = 0f;

    public TaskTrashAttack(Transform transform)
    {
        this.transform = transform;
        controller = transform.GetComponent<CharacterController>();
    }

    public override NODESTATE Evaluate()
    {
        Transform target = (Transform)GetData("target");

        if(Vector3.Distance(transform.position, target.position) > 1f)
        {
            controller.Move((target.position - transform.position) * GordonRamseyBT.speed * Time.deltaTime);
            //transform.position = Vector3.MoveTowards(transform.position, target.position, GuardBT.speed * Time.deltaTime);
            transform.LookAt(target.position);
        }
        if (Vector3.Distance(transform.position, target.position) < 1f)
            GordonRamsey.instance.ChangeState(GordonRamsey.RAMSEY_STATE.FOOD_TRASH,  target);

        Debug.Log("going to target");
        state = NODESTATE.RUNNING; 
        return state;
    }
}
