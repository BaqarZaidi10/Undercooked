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
                    new Sequence 
                    (new List<Node>
                        {
                            new ConditionalDecorator(CanAttack),
                            new CheckRawFood(transform),
                            new TaskRawAttack(transform),
                        }
                    ),
                    new Sequence 
                    (new List<Node>
                        {
                            new ConditionalDecorator(CanAttack),
                            new CheckBurntFood(transform),
                            new TaskBurnAttack(transform),
                        }
                    ),
                    new Sequence 
                    (new List<Node>
                        {
                            new ConditionalDecorator(CanAttack),
                            new CheckFoodOnGround(transform),
                            new TaskDropAttack(transform),
                        }
                    ),
                    new TaskPatrol(transform, waypoints),
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
