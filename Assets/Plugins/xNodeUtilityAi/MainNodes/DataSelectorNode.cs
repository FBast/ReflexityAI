using System;
using System.Collections.Generic;
using System.Reflection;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;
using XNode;
using Object = UnityEngine.Object;

namespace Plugins.xNodeUtilityAi.MainNodes {
    public class DataSelectorNode : DataNode {
        
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] public Object Data;
        
        [HideInInspector] public List<string> SelectedFields = new List<string>();
        [HideInInspector] public StringFieldInfoDictionary Fields;

        public override object GetValue(NodePort port) {
            if (Fields.ContainsKey(port.fieldName) ) {
                Tuple<FieldInfo, object> tuple = GetInputValue<Tuple<FieldInfo, object>>(nameof(Data));
                object value = null;
                if (tuple.Item2 != null) value = Fields[port.fieldName].GetValue(tuple.Item2);
                return new Tuple<FieldInfo, object>(Fields[port.fieldName], value);
            }
            return null;
        }

        public override void OnCreateConnection(NodePort from, NodePort to) {
            base.OnCreateConnection(from, to);
            if (to.fieldName == nameof(Data) && to.node == this) {
                Fields = new StringFieldInfoDictionary();
                Tuple<FieldInfo, object> tuple = GetInputValue<Tuple<FieldInfo, object>>(nameof(Data));
                foreach (FieldInfo fieldInfo in tuple.Item1.FieldType
                    .GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)) {
                    // AddDynamicOutput(fieldInfo.FieldType, fieldName: fieldInfo.Name);
                    Fields.Add(fieldInfo.Name, fieldInfo);
                }
            }
        }

        public override void OnRemoveConnection(NodePort port) {
            base.OnRemoveConnection(port);
            if (port.fieldName == nameof(Data) && port.node == this) {
                ClearDynamicPorts();
                Fields.Clear();
            }
        }

    }
}