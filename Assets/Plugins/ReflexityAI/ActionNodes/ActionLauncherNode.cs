using System.Collections.Generic;
using System.Linq;
using Plugins.ReflexityAI.Framework;
using UnityEngine;
using XNode;

namespace Plugins.ReflexityAI.ActionNodes {
    public class ActionLauncherNode : ActionNode {

        [HideInInspector] public SerializableInfo SelectedSerializableInfo;
        [HideInInspector] public List<SerializableInfo> SerializableInfos = new List<SerializableInfo>();
        [HideInInspector] public int ChoiceIndex;

        public override void OnCreateConnection(NodePort from, NodePort to) {
            base.OnCreateConnection(from, to);
            if (to.fieldName == nameof(Data) && to.node == this) {
                ReflectionData reflectionData = GetInputValue<ReflectionData>(nameof(Data));
                SerializableInfos.AddRange(reflectionData.Type
                    .GetMethods(SerializableInfo.DefaultBindingFlags)
                    .Select(info => new SerializableInfo(info)));
            }
        }
        
        public override void OnRemoveConnection(NodePort port) {
            base.OnRemoveConnection(port);
            if (port.fieldName == nameof(Data) && port.node == this) {
                SerializableInfos.Clear();
            }
        }

        public override object GetContext() {
            return GetInputValue<ReflectionData>(nameof(Data)).Value;
        }
        
        public override object[] GetParameters() {
            object[] parameters = null;
            if (SelectedSerializableInfo.Parameters.Count > 0) {
                parameters = SelectedSerializableInfo.Parameters
                    .Select(parameter => GetInputValue<ReflectionData>(nameof(parameter.Name)).Value)
                    .ToArray();
            }
            return parameters;
        }
        
        public override void Execute(object context, object[] parameters) {
            SelectedSerializableInfo.Invoke(context, parameters);
        }
        
        public override object GetValue(NodePort port) {
            return port.fieldName == nameof(LinkedOption) ? this : null;
        }
        
    }
}