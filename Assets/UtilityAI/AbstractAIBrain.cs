using System.Collections.Generic;
using UtilityAI.Nodes;
using XNode;

namespace UtilityAI {
    public abstract class AbstractAIBrain<T> : NodeGraph {
        
        public List<EntryNode<T>> GetEntryNodes() {
            List<EntryNode<T>> entryNodes = new List<EntryNode<T>>();
            foreach (Node node in nodes) {
                if (node as EntryNode<T>) 
                    entryNodes.Add((EntryNode<T>) node);
            }
            return entryNodes;
        }
        
        public List<OptionNode<T>> GetOptionNodes() {
            List<OptionNode<T>> optionNodes = new List<OptionNode<T>>();
            foreach (Node node in nodes) {
                if (node as OptionNode<T>) 
                    optionNodes.Add((OptionNode<T>) node);
            }
            return optionNodes;
        }

    }
}