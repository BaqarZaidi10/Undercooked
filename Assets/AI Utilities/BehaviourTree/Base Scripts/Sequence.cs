using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    //Sequence = composite node. AND logic gate. only returns success when all children succeed.
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

        public override NODESTATE Evaluate()
        {
            bool anyChildIsRunning = false;

            foreach(Node node in children)
            {
                switch(node.Evaluate())
                {
                    case NODESTATE.FAILURE:
                        state = NODESTATE.FAILURE;
                        return state;
                    case NODESTATE.SUCCESS:
                        state = NODESTATE.SUCCESS;
                        continue;                    
                    case NODESTATE.RUNNING:
                        state = NODESTATE.RUNNING;
                        continue;
                    default:
                        state = NODESTATE.SUCCESS;
                        return state;
                }
            }

            state = anyChildIsRunning ? NODESTATE.RUNNING : NODESTATE.SUCCESS;
            return state;
        }
    }
}
