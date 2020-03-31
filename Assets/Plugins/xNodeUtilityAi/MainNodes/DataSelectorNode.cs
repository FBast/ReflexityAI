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

        public MemberInfo SelectedMemberInfo;
        public List<MemberInfo> MemberInfos = new List<MemberInfo>();

        private void OnValidate() {
            MemberInfos.Clear();
            ReflectionData reflectionData = GetInputValue<ReflectionData>(nameof(Data));
            MemberInfos = reflectionData.Type.GetMemberInfos().ToList();
        }
        
        public override void OnCreateConnection(NodePort from, NodePort to) {
            base.OnCreateConnection(from, to);
            if (to.fieldName == nameof(Data) && to.node == this) {
                ReflectionData reflectionData = GetInputValue<ReflectionData>(nameof(Data));
                MemberInfos = reflectionData.Type.GetMemberInfos().ToList();
            }
        }
        
        public override void OnRemoveConnection(NodePort port) {
            base.OnRemoveConnection(port);
            if (port.fieldName == nameof(Data) && port.node == this) {
                MemberInfos.Clear();
            }
        }
        
        public override object GetValue(NodePort port) {
            return Application.isPlaying
                ? GetFullValue(port.fieldName)
                : GetReflectedValue(port.fieldName);
        }

        public override ReflectionData GetReflectedValue(string portName) {
            return new ReflectionData(SelectedMemberInfo.Name, SelectedMemberInfo.FieldType(), null);
        }

        public override ReflectionData GetFullValue(string portName) {
            ReflectionData reflectionData = GetInputValue<ReflectionData>(nameof(Data));
            return new ReflectionData(SelectedMemberInfo.Name, SelectedMemberInfo.FieldType(), reflectionData.Data);
        }
        
    }
}