using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Plugins.xNodeUtilityAi.Framework;
using Plugins.xNodeUtilityAi.Utils;
using UnityEngine;
using XNode;
using Object = UnityEngine.Object;

namespace Plugins.xNodeUtilityAi.ActionNodes {
    public class ActionLauncherNode : ActionNode {

        public SerializableMemberInfo SelectedSerializableMemberInfo;
        public List<SerializableMemberInfo> SerializableMemberInfos = new List<SerializableMemberInfo>();
        [HideInInspector] public int ChoiceIndex;

        public override void OnCreateConnection(NodePort from, NodePort to) {
            base.OnCreateConnection(from, to);
            if (to.fieldName == nameof(Data) && to.node == this) {
                Tuple<string, Type, object> reflectionData = GetInputValue<Tuple<string, Type, object>>(nameof(Data));
                SerializableMemberInfos = reflectionData.Item2.GetMembers(MemberTypes.Method)
                    .Select(info => new SerializableMemberInfo(info)).ToList();
            }
        }
        
        public override void OnRemoveConnection(NodePort port) {
            base.OnRemoveConnection(port);
            if (port.fieldName == nameof(Data) && port.node == this) {
                SerializableMemberInfos.Clear();
            }
        }

        public override void Execute(AbstractAIComponent context, Object data) {
            Tuple<string, Type, object> reflectionData = GetInputValue<Tuple<string, Type, object>>(nameof(Data));
            SelectedSerializableMemberInfo.GetRuntimeValue(reflectionData.Item3);
        }

        // public override object GetValue(NodePort port) {
        //     Tuple<string, Type, object> reflectionData = GetInputValue<Tuple<string, Type, object>>(nameof(Data));
        //     return Application.isPlaying
        //         ? SelectedSerializableMemberInfo.GetRuntimeValue(reflectionData.Item3)
        //         : SelectedSerializableMemberInfo.GetEditorValue();
        // }
        
    }
}