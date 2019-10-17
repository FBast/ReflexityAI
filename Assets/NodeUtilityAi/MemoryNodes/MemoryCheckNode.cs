using NodeUtilityAi.AbstractNodes;
using NodeUtilityAi.Framework;
using UnityEngine;

namespace NodeUtilityAi.MemoryNodes {
    public class MemoryCheckNode : DataEntryNode {

        protected override int ValueProvider(AbstractAIComponent context) {
            return GetData<Object>() != null ? 1 : 0;
        }
        
    }
}