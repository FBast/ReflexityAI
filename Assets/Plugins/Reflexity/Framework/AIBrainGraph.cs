using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.Reflexity.DataNodes;
using UnityEngine;
using XNode;

namespace Plugins.Reflexity.Framework {
    /// <summary>
    /// This class is used for the AIBrainGraphEditor, dont inherit from this
    /// </summary>
    public abstract class AIBrainGraph : NodeGraph {
        
        [Header("Brain Parameters")] 
        public List<string> HistoricTags = new List<string>();
        public List<string> MemoryTags = new List<string>();

        public IEnumerable<U> GetNodes<U>() {
            return nodes.Where(node => node is U).Cast<U>();
        }

        public void ClearCache() {
            foreach (ICacheable cacheable in GetNodes<ICacheable>()) {
                cacheable.ClearCache();
            }
        }

    }
    /// <summary>
    /// Inherit from this class if you want to create an AI brain graph
    /// </summary>
    /// <typeparam name="T">Reflexity derived class</typeparam>
    public abstract class AIBrainGraph<T> : AIBrainGraph {
        
        public override Node AddNode(Type type) {
            Node node = base.AddNode(type);
            if (node is DataReaderNode dataReaderNode) {
                dataReaderNode.UpdateData<T>();
            }
            return node;
        }

        private void OnValidate() {
            foreach (DataReaderNode dataReaderNode in GetNodes<DataReaderNode>()) {
                dataReaderNode.UpdateData<T>();
            }
        }

    }
}