using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Plugins.xNodeUtilityAi.Framework;
using XNode;

namespace Plugins.xNodeUtilityAi.MainNodes {
    public class DataReaderNode : DataNode, IContextual {

        public AbstractAIComponent Context { get; set; }
        
        private StringFieldInfoDictionary _fields;
        
        protected override void Init() {
            _fields = new StringFieldInfoDictionary();
            if (graph is AIBrainGraph brainGraph && brainGraph.ContextType != null) {
                FieldInfo[] fieldInfos = brainGraph.ContextType.Type
                    .GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
                // Add new field infos
                foreach (FieldInfo fieldInfo in fieldInfos) {
                    if (_fields.ContainsKey(fieldInfo.Name)) continue;
                    AddDynamicOutput(fieldInfo.FieldType, ConnectionType.Multiple, 
                        TypeConstraint.InheritedInverse, fieldInfo.Name);
                    _fields.Add(fieldInfo.Name, fieldInfo);
                }
                // Remove old field infos
                foreach (KeyValuePair<string, FieldInfo> pair in _fields) {
                    if (fieldInfos.Contains(pair.Value)) continue;
                    RemoveDynamicPort(pair.Value.Name);
                    _fields.Remove(pair.Key);
                }
            }
        }

        public override object GetValue(NodePort port) {
            if (_fields.ContainsKey(port.fieldName)) {
                object value = null;
                if (Context != null) value = _fields[port.fieldName].GetValue(Context);
                return new Tuple<FieldInfo, object>(_fields[port.fieldName], value);
            }
            return null;
        }
        
    }
    
    [Serializable]
    public class StringFieldInfoDictionary : Dictionary<string, FieldInfo> {}

}

