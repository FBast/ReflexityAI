using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Plugins.xNodeUtilityAi.Framework;
using Plugins.xNodeUtilityAi.Utils;
using UnityEngine;
using XNode;

namespace Plugins.xNodeUtilityAi.DataNodes {
    [NodeWidth(300)]
    public class DataReaderNode : DataNode, IContextual {

        public AbstractAIComponent Context { get; set; }

        [HideInInspector] public List<SerializableMemberInfo> SerializableMemberInfos = new List<SerializableMemberInfo>();

        protected override void Init() {
            base.Init();
            OnValidate();
        }

        private void OnValidate() {
            if (graph is AIBrainGraph brainGraph && brainGraph.ContextType != null) {
                List<MemberInfo> memberInfos = brainGraph.ContextType.Type.GetMembers(MemberTypes.Field | MemberTypes.Property).ToList();
                // Add new ports
                foreach (MemberInfo memberInfo in memberInfos) {
                    if (SerializableMemberInfos.Any(info => info.Name == memberInfo.Name)) continue;
                    SerializableMemberInfo serializableMemberInfo = new SerializableMemberInfo(memberInfo);
                    AddDynamicOutput(memberInfo.FieldType(), ConnectionType.Multiple, TypeConstraint.None, serializableMemberInfo.PortName);
                    SerializableMemberInfos.Add(serializableMemberInfo);
                }
                // Remove old ports
                for (int i = SerializableMemberInfos.Count - 1; i >= 0; i--) {
                    if (memberInfos.Any(info => info.Name == SerializableMemberInfos[i].Name)) continue;
                    RemoveDynamicPort(SerializableMemberInfos[i].PortName);
                    SerializableMemberInfos.RemoveAt(i);
                }
            } else {
                throw new Exception("No brain graph context type found, please select one");
            }
        }

        public override object GetValue(NodePort port) {
            SerializableMemberInfo firstOrDefault = SerializableMemberInfos.FirstOrDefault(info => info.PortName == port.fieldName);
            if (firstOrDefault == null) throw new Exception("No reflected data found for " + port.fieldName);
            return Application.isPlaying ? firstOrDefault.GetRuntimeValue(Context) : firstOrDefault.GetEditorValue();
        }
        
    }

}


