using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    //Selector = composite node. OR logic gate. Returns success when any children succeed.
    public class Selector : Node
    {
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }

        public override NODESTATE Evaluate()
        {
            foreach(Node node in children)
            {
                switch(node.Evaluate())
                {
                    case NODESTATE.FAILURE:
                        state = NODESTATE.FAILURE;
                        continue;
                    case NODESTATE.SUCCESS:
                        state = NODESTATE.SUCCESS;
                        return state;                    
                    case NODESTATE.RUNNING:
                        state = NODESTATE.RUNNING;
                        return state;
                    default:
                        continue;
                }
            }

            state = NODESTATE.FAILURE;
            return state;
        }
    }
}
