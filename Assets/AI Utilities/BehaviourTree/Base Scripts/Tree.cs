using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    //Tree: start of behaviour tree. setup root of behaviour tree.
    public abstract class Tree : MonoBehaviour
    {
        private Node root = null;
        private bool isPaused = true;

        protected virtual void Start()
        {
            root = SetupTree();
        }

        protected virtual void Update()
        {
            if (root != null && !isPaused)
            {
                root.Evaluate();
            }
        }

        protected abstract Node SetupTree();

        public void PauseTree()
        {
            isPaused = true;
        }
        
        public void ResumeTree()
        {
            isPaused = false;
        }
    }
}
