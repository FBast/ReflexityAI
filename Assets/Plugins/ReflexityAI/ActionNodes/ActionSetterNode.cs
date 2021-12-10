using System.Collections.Generic;
using System.Linq;
using Plugins.Reflexity.Framework;
using UnityEngine;
using XNode;
using Object = UnityEngine.Object;

namespace Plugins.Reflexity.ActionNodes {
    [CreateNodeMenu("Reflexity/Action/Setter")]
    public class ActionSetterNode : ActionNode {

        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)] public Object Value;
        [HideInInspector] public SerializableInfo SelectedSerializableInfo;
        [HideInInspector] public List<SerializableInfo> SerializableInfos = new List<SerializableInfo>();
        [HideInInspector] public int ChoiceIndex;

        public override void OnCreateConnection(NodePort from, NodePort to) {
            base.OnCreateConnection(from, to);
            if (to.fieldName == nameof(Data) && to.node == this) {
                ReflectionData reflectionData = GetInputValue<ReflectionData>(nameof(Data));
                SerializableInfos.AddRange(reflectionData.Type
                    .GetFields(SerializableInfo.DefaultBindingFlags)
                    .Select(info => new SerializableInfo(info)));
                SerializableInfos.AddRange(reflectionData.Type
                    .GetProperties(SerializableInfo.DefaultBindingFlags)
                    .Select(info => new SerializableInfo(info)));
            }
        }
        
        public override void OnRemoveConnection(NodePort port) {
            base.OnRemoveConnection(port);
            if (port.fieldName == nameof(Data) && port.node == this) {
                SerializableInfos.Clear();
                SelectedSerializableInfo = null;
            }
        }

        public override object GetContext() {
            return GetInputValue<ReflectionData>(nameof(Data)).Value;
        }

        public override object[] GetParameters() {
            return new[] {GetInputValue<ReflectionData>(nameof(Value)).Value};
        }

        public override void Execute(object context, object[] parameters) {
            SelectedSerializableInfo.SetValue(context, parameters[0]);
        }
        
        public override object GetValue(NodePort port) {
            return port.fieldName == nameof(LinkedOption) ? this : null;
        }

    }
}