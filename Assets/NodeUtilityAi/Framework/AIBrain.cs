using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace NodeUtilityAi.Framework {
    [Serializable, CreateAssetMenu(fileName = "UtilityAIBrain", menuName = "UtilityAI/Brain")]
    public class AIBrain : NodeGraph {
        
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