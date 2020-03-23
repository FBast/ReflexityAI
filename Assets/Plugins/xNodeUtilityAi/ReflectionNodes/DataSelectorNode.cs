using System;
using System.Collections.Generic;
using System.Reflection;
using Plugins.xNodeUtilityAi.Framework;
using XNode;
using Object = UnityEngine.Object;

namespace Plugins.xNodeUtilityAi.ReflectionNodes {
    public class DataSelectorNode : EntryNode {
        
        [Input(ShowBackingValue.Never, ConnectionType.Override)] public Object Data;
        
        private readonly Dictionary<string, FieldInfo> _fields = new Dictionary<string, FieldInfo>();
        private object _selectionContext;
        
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

        public override void OnCreateConnection(NodePort from, NodePort to) {
            base.OnCreateConnection(from, to);
            if (to.fieldName == nameof(Data) && to.node == this) {
                Tuple<FieldInfo, object> tuple = GetInputValue<Tuple<FieldInfo, object>>(nameof(Data));
                foreach (FieldInfo fieldInfo in tuple.Item1.FieldType
                    .GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)) {
                    AddDynamicOutput(fieldInfo.FieldType, fieldName: fieldInfo.Name);
                    _fields.Add(fieldInfo.Name, fieldInfo);
                }
            }
        }

        public override void OnRemoveConnection(NodePort port) {
            base.OnRemoveConnection(port);
            if (port.fieldName == nameof(Data)) {
                ClearDynamicPorts();
                _fields.Clear();
            }
        }

    }
}