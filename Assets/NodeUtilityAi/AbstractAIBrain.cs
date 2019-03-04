using System.Collections.Generic;
using XNode;

namespace NodeUtilityAi {
    public abstract class AbstractAIBrain : NodeGraph {
        
        public List<T> GetNodes<T>() where T : Node {
            List<T> entryNodes = new List<T>();
            foreach (Node node in nodes) {
                if (node as T) 
                    entryNodes.Add((T) node);
            }
            return entryNodes;
        }
        
    }
}