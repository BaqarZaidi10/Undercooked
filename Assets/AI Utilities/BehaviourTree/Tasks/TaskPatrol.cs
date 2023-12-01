using BehaviourTree;
using UnityEngine;

public class TaskPatrol : Node
{
    private Transform transform;
    private Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private float waitTime = 1f, waitCounter = 0f;
    private bool waiting = false;

    public TaskPatrol(Transform transform, Transform[] waypoints)
    {
        this.transform = transform;
        this.waypoints = waypoints;
    }

    public override NODESTATE Evaluate()
    {
        if (waiting)
        {
            waitCounter += Time.deltaTime;
            if (waitCounter >= waitTime)
                waiting = false;
        }
        else
        {
            Transform wp = waypoints[currentWaypointIndex];
            if (Vector3.Distance(transform.position, wp.position) < 0.01f)
            {
                transform.position = wp.position;
                waitCounter = 0f;
                waiting = true;

                //Keep adding 1 to waypoint index. when current waypoint index = waypoints.length,
                //set it back to zero to avoid index out of range exception as last index = waypoints.length - 1.
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, wp.position, GuardBT.speed * Time.deltaTime);
                //transform.LookAt(wp.position);
            }
        }

        Debug.Log("patrolling");
        state = NODESTATE.RUNNING;
        return state;
    }
}
