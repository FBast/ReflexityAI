using System.Collections.Generic;
using System.Linq;
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
        
        public List<T> GetNodes<T>() {
            return nodes.Where(node => node is T).Cast<T>().ToList();
        }

    }
}