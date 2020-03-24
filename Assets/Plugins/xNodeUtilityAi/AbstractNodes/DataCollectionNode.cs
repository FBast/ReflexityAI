using System.Collections.Generic;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;
using XNode;

namespace Plugins.xNodeUtilityAi.AbstractNodes {
    public abstract class DataCollectionNode : DataNode, IContextual {
        
        [Output] public DataCollectionNode LinkedOption;
        [Output] public Object DataOut;
        public int Index { get; set; }
        
        public AbstractAIComponent Context { get; set; }
        public int CollectionCount => CollectionProvider(Context)?.Count ?? 0;

        protected abstract List<Object> CollectionProvider(AbstractAIComponent context);
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(LinkedOption)) {
                return this;
            }
            if (port.fieldName == nameof(DataOut)) {
                if (Context == null) return null;
                List<Object> collection = CollectionProvider(Context);
                if (collection != null && collection.Count > Index)
                    return collection[Index];
            }
            return null;
        }

    }
}