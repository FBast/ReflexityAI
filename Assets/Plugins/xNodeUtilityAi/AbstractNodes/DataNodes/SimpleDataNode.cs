using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;
using XNode;

namespace Plugins.xNodeUtilityAi.AbstractNodes.DataNodes {
    public abstract class SimpleDataNode : DataNode {
        
        [Output] public TaggedData DataOut;
        public string DataTag = "Data";
        
        protected abstract Object DataProvider(AbstractAIComponent context);
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(DataOut)) {
                if (_context == null) return null;
                Object data = DataProvider(_context);
                if (data != null)
                    return GetData();
            }
            return null;
        }
        
        public TaggedData GetData() {
            TaggedData taggedData = new TaggedData {
                Data = DataProvider(_context), 
                DataTag = DataTag
            };
            return taggedData;
        }
        
    }
}