using System.Collections.Generic;
using Plugins.xNodeUtilityAi.Framework;
using XNode;
using Object = UnityEngine.Object;

namespace Plugins.xNodeUtilityAi.AbstractNodes.DataNodes {
    public abstract class CollectionDataNode : DataNode {
        
        [Output] public CollectionDataNode LinkedOption;
        [Output] public TaggedData DataOut;
        public string DataTag = "Data";
        public int Index;

        public int CollectionCount => CollectionProvider(_context)?.Count ?? 0;

        protected abstract List<Object> CollectionProvider(AbstractAIComponent context);
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(LinkedOption)) {
                return this;
            }
            if (port.fieldName == nameof(DataOut)) {
                if (_context == null) return null;
                List<Object> collection = CollectionProvider(_context);
                if (collection != null && collection.Count > Index)
                    return GetData();
            }
            return null;
        }
        
        public TaggedData GetData() {
            TaggedData taggedData = new TaggedData {
                Data = CollectionProvider(_context)[Index], 
                DataTag = DataTag
            };
            return taggedData;
        }
        
    }
}