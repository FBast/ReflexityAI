using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;

namespace Plugins.xNodeUtilityAi.MemoryNodes {
    public class MemoryCheckNode : EntryBoolNode {

        protected override bool ValueProvider(AbstractAIComponent context) {
            return GetData<Object>() != null;
        }
        
    }
}