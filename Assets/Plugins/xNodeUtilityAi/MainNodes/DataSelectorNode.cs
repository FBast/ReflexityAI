using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                ReflectionData reflectionData = GetInputValue<ReflectionData>(nameof(Data));
                SerializableMemberInfos = reflectionData.Type.GetMemberInfos()
                    .Select(info => info.ToSerializableMemberInfo()).ToList();
            }
        }

        public override void OnRemoveConnection(NodePort port) {
            base.OnRemoveConnection(port);
            if (port.fieldName == nameof(Data) && port.node == this) {
                SerializableMemberInfos.Clear();
            }
        }
        
        public override object GetValue(NodePort port) {
            return Application.isPlaying
                ? GetFullValue(port.fieldName)
                : GetReflectedValue(port.fieldName);
        }

        public override object GetReflectedValue(string portName) {
            MemberInfo selectedMemberInfo = SelectedSerializableMemberInfo.ToMemberInfo();
            return selectedMemberInfo.FieldType().IsPrimitive ? null : 
                new ReflectionData(selectedMemberInfo.Name, selectedMemberInfo.FieldType(), null);
        }

        public override object GetFullValue(string portName) {
            MemberInfo selectedMemberInfo = SelectedSerializableMemberInfo.ToMemberInfo();
            ReflectionData reflectionData = GetInputValue<ReflectionData>(nameof(Data));
            MemberInfo firstOrDefault = reflectionData.Type.GetMemberInfos()
                .FirstOrDefault(info => info.Name == selectedMemberInfo.Name);
            if (firstOrDefault != null) {
                return firstOrDefault.FieldType().IsPrimitive
                    ? firstOrDefault.GetValue(reflectionData.Data)
                    : new ReflectionData(firstOrDefault.Name, firstOrDefault.FieldType(),
                        firstOrDefault.GetValue(reflectionData.Data));
            }
            throw new Exception("Cannot select member info based on selectedMemberInfo");
        }
        
    }
}