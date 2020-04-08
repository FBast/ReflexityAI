using UnityEngine;
using XNode;

namespace Plugins.xNodeUtilityAi.Framework {
    public abstract class SimpleDataNode : DataNode {

        [Output] public Object DataOut;
        
        protected abstract object ValueProvider(AbstractAIComponent context);
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(DataOut)) {
                // if (graph is AIBrainGraph brainGraph && brainGraph.CurrentContext != null) {
                //     object data = ValueProvider(brainGraph.CurrentContext);
                //     if (data != null) return data;
                // }
                return null;
            }
            return null;
        }

    }
}