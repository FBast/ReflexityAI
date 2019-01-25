using System.Collections.Generic;
using CubeAI;
using UtilityAI.Nodes;
using XNode;

namespace UtilityAI {
    public abstract class UtilityAIBrain<T> : NodeGraph {
        
        public List<EntryNode<T>> GetEntryNode() {
            List<EntryNode<T>> entryNodes = new List<EntryNode<T>>();
            foreach (Node node in nodes) {
                if (node as EntryNode<T>) 
                    entryNodes.Add((EntryNode<T>) node);
            }
            return entryNodes;
        }
        
        public List<ActionNode<T>> GetActionNode() {
            List<ActionNode<T>> actionNodes = new List<ActionNode<T>>();
            foreach (Node node in nodes) {
                if (node as ActionNode<T>) 
                    actionNodes.Add((ActionNode<T>) node);
            }
            return actionNodes;
        }

    }
}