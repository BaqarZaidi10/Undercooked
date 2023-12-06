using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GordonRamseyBT : BehaviourTree.Tree
{
    public UnityEngine.Transform[] waypoints;

    public static float speed = 1f;
    public static float fovRange = 6f;
    public static float attackRange = 1f;
    public static float cooldown = 0f, cooldownReset = 5f;
    public bool canAttack = true;
    public static GordonRamseyBT instance;

    private void Awake()
    {
        if (instance)
            Destroy(instance.gameObject);        
        else
            instance = this;
    }

    protected override Node SetupTree()
    {
        Node root = new Selector
            (new List<Node>
                {
                    new Sequence //Node 1 (first priority)
                    (new List<Node>
                        {
                            new CheckRawFood(transform),
                            new ConditionalDecorator(CanAttack),
                            new TaskAttack(transform),
                        }
                    ),
                    new Sequence //Node 2 (second priority) - runs only when node 1 fails
                    (new List<Node>
                        {
                            new CheckFoodOnGround(transform),
                            new ConditionalDecorator(CanAttack),
                            new TaskGoToTarget(transform),
                        }
                    ),
                    new TaskPatrol(transform, waypoints), //Default node: only runs when all other nodes fail
                }
            );
        
        return root;
    }

    // Custom method to check if the attack is on cooldown
    private NODESTATE CanAttack()
    {
        if (canAttack)
        {
            return NODESTATE.SUCCESS;
        }
        else
        {
            return NODESTATE.FAILURE;
        }
    }

    public IEnumerator AttackCooldown(float waitTime)
    {
        canAttack = false;
        yield return new WaitForSeconds(waitTime);
        canAttack = true;
    }
}
