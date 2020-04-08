using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;
using XNode;
using Object = UnityEngine.Object;

namespace Plugins.xNodeUtilityAi.DataNodes {
    public class DataSelectorNode : DataNode {
        
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] public Object Data;
        [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] public Object Output;

        public SerializableFieldOrProperty selectedSerializableFieldOrProperty;
        public List<SerializableFieldOrProperty> SerializableDatas = new List<SerializableFieldOrProperty>();
        [HideInInspector] public int ChoiceIndex;
        
        public override void OnCreateConnection(NodePort from, NodePort to) {
            base.OnCreateConnection(from, to);
            if (to.fieldName == nameof(Data) && to.node == this) {
                Tuple<string, Type, object> reflectionData = GetInputValue<Tuple<string, Type, object>>(nameof(Data));
                SerializableDatas.AddRange(reflectionData.Item2
                    .GetFields(SerializableMemberInfo.DefaultBindingFlags)
                    .Select(info => new SerializableFieldOrProperty(info)));
                SerializableDatas.AddRange(reflectionData.Item2
                    .GetProperties(SerializableMemberInfo.DefaultBindingFlags)
                    .Select(info => new SerializableFieldOrProperty(info)));
            }
        }
        
        public override void OnRemoveConnection(NodePort port) {
            base.OnRemoveConnection(port);
            if (port.fieldName == nameof(Data) && port.node == this) {
                SerializableDatas.Clear();
            }
        }
        
        public override object GetValue(NodePort port) {
            Tuple<string, Type, object> reflectionData = GetInputValue<Tuple<string, Type, object>>(nameof(Data));
            return Application.isPlaying
                ? selectedSerializableFieldOrProperty.GetRuntimeValue(reflectionData.Item3)
                : selectedSerializableFieldOrProperty.GetEditorValue();
        }

    }
}