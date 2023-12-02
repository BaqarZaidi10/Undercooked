using BehaviourTree;
using System.Collections.Generic;

public class GuardBT : Tree
{
    public UnityEngine.Transform[] waypoints;

    public static float speed = 1f;
    public static float fovRange = 6f;
    public static float attackRange = 1f;

    protected override Node SetupTree()
    {
        Node root = new Selector
            (new List<Node>
                {
                    new Sequence //Node 1 (first priority)
                    (new List<Node>
                        {
                            new CheckEnemyInAttackRange(transform),
                            new TaskAttack(transform),
                        }
                    ),
                    new Sequence //Node 2 (second priority) - runs only when node 1 fails
                    (new List<Node>
                        {
                            new CheckEnemyInFOVRange(transform),
                            new TaskGoToTarget(transform),
                        }
                    ),
                    new TaskPatrol(transform, waypoints), //Default node: only runs when all other nodes fail
                }
            );
        
        return root;
    }
}
