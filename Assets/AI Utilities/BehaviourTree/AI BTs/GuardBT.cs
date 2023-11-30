using BehaviourTree;
using System.Collections.Generic;

public class GuardBT : Tree
{
    public UnityEngine.Transform[] waypoints;

    public static float speed = 2f;
    public static float fovRange = 6f;
    public static float attackRange = 1f;

    protected override Node SetupTree()
    {
        Node root = new Selector
            (new List<Node>
                {
                    new Sequence
                    (new List<Node>
                        {
                            new CheckEnemyInAttackRange(transform),
                            new TaskAttack(transform),
                        }
                    ),
                    new Sequence //Node 1 (first priority)
                    (new List<Node>
                        {
                            new CheckEnemyInFOVRange(transform),
                            new TaskGoToTarget(transform),
                        }
                    ),
                    new TaskPatrol(transform, waypoints), //Node 2 (second priority) - runs only when node 1 fails
                }
            );
        
        return root;
    }
}
