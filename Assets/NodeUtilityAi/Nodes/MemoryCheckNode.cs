using UnityEngine;

namespace NodeUtilityAi.Nodes {
    public class MemoryCheckNode : DataEntryNode {

        protected override int ValueProvider(AbstractAIComponent context) {
            return GetData<Object>() != null ? 1 : 0;
        }
        
    }
}