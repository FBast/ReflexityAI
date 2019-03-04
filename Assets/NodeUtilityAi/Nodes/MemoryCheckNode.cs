using UnityEngine;

namespace NodeUtilityAi.Nodes {
    public class MemoryCheckNode : SimpleEntryNode {

        protected override int ValueProvider(AbstractAIComponent context) {
            return GetData<Object>() != null ? 1 : 0;
        }
        
    }
}