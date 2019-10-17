using System.Collections.Generic;
using NodeUtilityAi.Framework;
using XNode;
using Object = UnityEngine.Object;

namespace NodeUtilityAi.AbstractNodes {
    public abstract class CollectionEntryNode : EntryNode {
        
        [Output] public CollectionEntryNode LinkedOption;
        [Output] public TaggedData DataOut;
        public string DataTag = "Data";
        public int Index;

        public int CollectionCount => CollectionProvider(_context)?.Count ?? 0;

        protected abstract List<Object> CollectionProvider(AbstractAIComponent context);
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == "LinkedOption") {
                return this;
            }
            if (port.fieldName == "DataOut") {
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