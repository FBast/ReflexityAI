using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;
using XNode;

namespace Plugins.xNodeUtilityAi.ActionNodes {
    public class ActionLauncherNode : ActionNode {

        [HideInInspector] public SerializableInfo SelectedSerializableInfo;
        [HideInInspector] public List<SerializableInfo> SerializableInfos = new List<SerializableInfo>();
        [HideInInspector] public int ChoiceIndex;

        public override void OnCreateConnection(NodePort from, NodePort to) {
            base.OnCreateConnection(from, to);
            if (to.fieldName == nameof(Data) && to.node == this) {
                Tuple<string, Type, object> reflectionData = GetInputValue<Tuple<string, Type, object>>(nameof(Data));
                SerializableInfos.AddRange(reflectionData.Item2
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
            return GetInputValue<Tuple<string, Type, object>>(nameof(Data)).Item3;
        }
        
        public override object[] GetParameters() {
            object[] parameters = null;
            if (SelectedSerializableInfo.Parameters.Count > 0) {
                parameters = SelectedSerializableInfo.Parameters
                    .Select(parameter => GetInputValue<Tuple<string, Type, object>>(nameof(parameter.Name))?.Item3)
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