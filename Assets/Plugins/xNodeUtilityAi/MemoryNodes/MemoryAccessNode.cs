using System.Collections.Generic;
using Plugins.xNodeUtilityAi.AbstractNodes;
using Plugins.xNodeUtilityAi.Framework;
using XNode;
using Object = UnityEngine.Object;

namespace Plugins.xNodeUtilityAi.MemoryNodes {
    public class MemoryAccessNode : ActionNode {

        [Input(ShowBackingValue.Never, ConnectionType.Override)] public TaggedData Data;
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
            context.SaveToMemory(firstData.Key, firstData.Value);
        }
        
    }

}