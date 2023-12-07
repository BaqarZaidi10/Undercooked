using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GordonRamseyBT : BehaviourTree.Tree
{
    public UnityEngine.Transform[] waypoints;

    public static float speed = 1f;
    public static float fovRange = 6f;
    public static float attackRange = 2f;
    public static float cooldown = 10f;
    public bool canAttack, foodTrashed;
    public static GordonRamseyBT instance;
    public static Transform trasher;

    private void Awake()
    {
        if (instance)
            Destroy(instance.gameObject);        
        else
            instance = this;
    }

    private void OnEnable()
    {
        TrashCounter.onFoodTrashed += FoodTrashedAction;
    }
    
    private void OnDisable()
    {
        TrashCounter.onFoodTrashed -= FoodTrashedAction;
    }

    private void FoodTrashedAction(Transform player)
    {
        foodTrashed = true;
        Invoke(nameof(ResetFoodTrash), 3f);
        trasher = player;
    }

    public void ResetFoodTrash()
    {
        foodTrashed = false;
        trasher = null;
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
                            new ConditionalDecorator(FoodTrashed),
                            new CheckTrashFood(transform),
                            new TaskTrashAttack(transform),
                        }
                    ), 
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
                            new CheckDroppedFood(transform),
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
        Debug.Log(canAttack);
        if (canAttack)
        {
            return NODESTATE.SUCCESS;
        }
        else
        {
            return NODESTATE.FAILURE;
        }
    }
    
    private NODESTATE FoodTrashed()
    {
        Debug.Log(foodTrashed);
        if (foodTrashed)
        {
            return NODESTATE.SUCCESS;
        }
        else
        {
            return NODESTATE.FAILURE;
        }
    }
}
