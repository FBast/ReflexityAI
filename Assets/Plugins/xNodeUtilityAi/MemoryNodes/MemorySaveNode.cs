using System.Collections.Generic;
using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using XNode;
using Object = UnityEngine.Object;

namespace Plugins.xNodeUtilityAi.MemoryNodes {
    public class MemorySaveNode : ActionNode {

        [Output] public TaggedData LoadData;
        public string DataTag;
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == "LoadData") {
                return _context.LoadFromMemory(DataTag);
            }
            return base.GetValue(port);
        }
        
        public override void Execute(AbstractAIComponent context, AIData aiData) {
            KeyValuePair<string, Object> firstData = aiData.GetFirstData();
            context.SaveInMemory(firstData.Key, firstData.Value);
        }
        
    }

}