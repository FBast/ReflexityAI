using System;
using System.Collections.Generic;
using Plugins.xNodeUtilityAi.Utils.ClassTypeReference;
using UnityEngine;
using XNode;

namespace Plugins.xNodeUtilityAi.Framework {
    [CreateAssetMenu(fileName = "UtilityAIBrain", menuName = "UtilityAI/AIBrainGraph")]
    public class AIBrainGraph : NodeGraph {
        
        [ClassExtends(typeof(AbstractAIComponent), Grouping = ClassGrouping.None)]
        public ClassTypeReference ContextType;

        [Header("Tags")] 
        public List<string> HistoricTags = new List<string>();
        public List<string> MemoryTags = new List<string>();

        [NonSerialized] public AbstractAIComponent CurrentContext;
        
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