using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Plugins.xNodeUtilityAi.Framework;
using Plugins.xNodeUtilityAi.Utils;
using UnityEngine;
using XNode;

namespace Plugins.xNodeUtilityAi.MainNodes {
    public class DataReaderNode : DataNode, IContextual {

        public AbstractAIComponent Context { get; set; }

        private List<SerializableMemberInfo> SerializableMemberInfos = new List<SerializableMemberInfo>();

        protected override void Init() {
            base.Init();
            OnValidate();
        }

        private void OnValidate() {
            if (graph is AIBrainGraph brainGraph && brainGraph.ContextType != null) {
                List<MemberInfo> memberInfos = brainGraph.ContextType.Type.GetMemberInfos().ToList();
                // Add new ports
                foreach (MemberInfo memberInfo in memberInfos) {
                    if (SerializableMemberInfos.Any(info => info.FieldName == memberInfo.Name)) continue;
                    AddDynamicOutput(memberInfo.FieldType(), ConnectionType.Multiple, TypeConstraint.None, memberInfo.Name);
                    SerializableMemberInfos.Add(memberInfo.ToSerializableMemberInfo());
                }
                // Remove old ports
                for (int i = SerializableMemberInfos.Count - 1; i >= 0; i--) {
                    if (memberInfos.Any(info => info.Name == SerializableMemberInfos[i].FieldName)) continue;
                    RemoveDynamicPort(SerializableMemberInfos[i].FieldName);
                    SerializableMemberInfos.RemoveAt(i);
                }
            } else {
                throw new Exception("No brain graph context type found, please select one");
            }
        }

        public override object GetValue(NodePort port) {
            return Application.isPlaying
                ? GetFullValue(port.fieldName)
                : GetReflectedValue(port.fieldName);
        }

        public override object GetReflectedValue(string portName) {
            SerializableMemberInfo firstOrDefault = SerializableMemberInfos.FirstOrDefault(info => info.FieldName == portName);
            if (firstOrDefault == null) throw new Exception("No reflected data found for " + portName);
            MemberInfo memberInfo = firstOrDefault.ToMemberInfo();
            return memberInfo.FieldType().IsPrimitive ? null : 
                new ReflectionData(memberInfo.Name, memberInfo.FieldType(), null);
        }

        public override object GetFullValue(string portName) {
            SerializableMemberInfo firstOrDefault = SerializableMemberInfos.FirstOrDefault(info => info.FieldName == portName);
            if (firstOrDefault == null) throw new Exception("No reflected data found for " + portName);
            MemberInfo memberInfo = firstOrDefault.ToMemberInfo();
            return memberInfo.FieldType().IsPrimitive ? memberInfo.GetValue(Context) : 
                new ReflectionData(memberInfo.Name, memberInfo.FieldType(), memberInfo.GetValue(Context));
        }

    }
}


