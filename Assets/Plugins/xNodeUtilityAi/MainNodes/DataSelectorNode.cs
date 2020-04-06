using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.xNodeUtilityAi.Framework;
using Plugins.xNodeUtilityAi.Utils;
using UnityEngine;
using XNode;
using Object = UnityEngine.Object;

namespace Plugins.xNodeUtilityAi.MainNodes {
    public class DataSelectorNode : DataNode {
        
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] public Object Data;
        [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] public Object Output;

        public SerializableMemberInfo SelectedSerializableMemberInfo;
        public List<SerializableMemberInfo> SerializableMemberInfos = new List<SerializableMemberInfo>();
        [HideInInspector] public int ChoiceIndex;
        
        public override void OnCreateConnection(NodePort from, NodePort to) {
            base.OnCreateConnection(from, to);
            if (to.fieldName == nameof(Data) && to.node == this) {
                Tuple<string, Type, object> reflectionData = GetInputValue<Tuple<string, Type, object>>(nameof(Data));
                SerializableMemberInfos = reflectionData.Item2.GetFieldAndPropertyInfos()
                    .Select(info => new SerializableMemberInfo(info)).ToList();
            }
        }
        
        public override void OnRemoveConnection(NodePort port) {
            base.OnRemoveConnection(port);
            if (port.fieldName == nameof(Data) && port.node == this) {
                SerializableMemberInfos.Clear();
            }
        }
        
        public override object GetValue(NodePort port) {
            Tuple<string, Type, object> reflectionData = GetInputValue<Tuple<string, Type, object>>(nameof(Data));
            return Application.isPlaying
                ? SelectedSerializableMemberInfo.GetRuntimeValue(reflectionData.Item3)
                : SelectedSerializableMemberInfo.GetEditorValue();
        }

    }
}