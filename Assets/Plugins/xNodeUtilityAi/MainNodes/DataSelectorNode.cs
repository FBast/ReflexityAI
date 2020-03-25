using System;
using System.Collections.Generic;
using System.Reflection;
using Plugins.xNodeUtilityAi.Framework;
using Plugins.xNodeUtilityAi.Utils;
using XNode;
using Object = UnityEngine.Object;

namespace Plugins.xNodeUtilityAi.MainNodes {
    public class DataSelectorNode : DataNode {
        
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] public Object Data;
        [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] public Object Output;

        public MemberInfo SelectedMemberInfo;
        public List<MemberInfo> MemberInfos = new List<MemberInfo>();

        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(Output)) {
                Tuple<MemberInfo, object> tuple = GetInputValue<Tuple<MemberInfo, object>>(nameof(Data));
                object context = null;
                if (tuple.Item2 != null) {
                    context = SelectedMemberInfo.GetValue(tuple.Item2);
                }
                return new Tuple<MemberInfo, object>(SelectedMemberInfo, context);
            }
            return null;
        }

        public override void OnCreateConnection(NodePort from, NodePort to) {
            base.OnCreateConnection(from, to);
            if (to.fieldName == nameof(Data) && to.node == this) {
                Tuple<MemberInfo, object> tuple = GetInputValue<Tuple<MemberInfo, object>>(nameof(Data));
                foreach (MemberInfo memberInfo in tuple.Item1.FieldType().GetFieldAndProperties()) {
                    MemberInfos.Add(memberInfo);
                }
            }
        }
        
        public override void OnRemoveConnection(NodePort port) {
            base.OnRemoveConnection(port);
            if (port.fieldName == nameof(Data) && port.node == this) {
                MemberInfos.Clear();
            }
        }

    }
}