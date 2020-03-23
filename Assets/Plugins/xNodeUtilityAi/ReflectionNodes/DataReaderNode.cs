using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;
using XNode;

namespace Plugins.xNodeUtilityAi.ReflectionNodes {
    public class DataReaderNode : EntryNode {

        public bool IsReady;
        
        private readonly Dictionary<string, FieldInfo> _fields = new Dictionary<string, FieldInfo>();

        protected override void Init() {
            if (IsReady) return;
            Read();
        }

        [ContextMenu("Read")]
        public void Read() {
            ClearDynamicPorts();
            _fields.Clear();
            if (graph is AIBrainGraph brainGraph && brainGraph.ContextType != null) {
                foreach (FieldInfo fieldInfo in brainGraph.ContextType.Type 
                    .GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)) {
                    AddDynamicOutput(fieldInfo.FieldType, fieldName: fieldInfo.Name);
                    _fields.Add(fieldInfo.Name, fieldInfo);
                }
                IsReady = true;
            }
        }

        public override object GetValue(NodePort port) {
            if (_fields.ContainsKey(port.fieldName)) {
                object value = null;
                if (graph is AIBrainGraph brainGraph && brainGraph.CurrentContext != null) {
                    value = _fields[port.fieldName].GetValue(brainGraph.CurrentContext);
                }
                return new Tuple<FieldInfo, object>(_fields[port.fieldName], value);
            }
            return null;
        }

    }

}

