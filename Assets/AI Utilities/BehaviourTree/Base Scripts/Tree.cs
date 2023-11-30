using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    //Tree: start of behaviour tree. setup root of behaviour tree.
    public abstract class Tree : MonoBehaviour
    {
        private Node root = null;

        private void Start()
        {
            root = SetupTree();
        }

        private void Update()
        {
            if (root != null)
            {
                root.Evaluate();
            }
        }

        protected abstract Node SetupTree();
    }
}
