using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;
using XNode;

namespace Plugins.xNodeUtilityAi.ActionNodes {
    public class ActionLauncherNode : ActionNode {

        public SerializableMethodInfo SelectedSerializableMethodInfo;
        public List<SerializableMethodInfo> SerializableMethodInfos = new List<SerializableMethodInfo>();
        [HideInInspector] public int ChoiceIndex;

        public override void OnCreateConnection(NodePort from, NodePort to) {
            base.OnCreateConnection(from, to);
            if (to.fieldName == nameof(Data) && to.node == this) {
                Tuple<string, Type, object> reflectionData = GetInputValue<Tuple<string, Type, object>>(nameof(Data));
                SerializableMethodInfos.AddRange(reflectionData.Item2
                    .GetMethods(SerializableMemberInfo.DefaultBindingFlags)
                    .Select(info => new SerializableMethodInfo(info)));
            }
        }
        
        public override void OnRemoveConnection(NodePort port) {
            base.OnRemoveConnection(port);
            if (port.fieldName == nameof(Data) && port.node == this) {
                SerializableMethodInfos.Clear();
            }
        }

        public override object GetContext() {
            Tuple<string, Type, object> inputValue = GetInputValue<Tuple<string, Type, object>>(nameof(Data));
            return inputValue.Item3;
        }
        
        public override object[] GetParameters() {
            object[] parameters = null;
            if (SelectedSerializableMethodInfo.Parameters.Count > 0) {
                parameters = SelectedSerializableMethodInfo.Parameters
                    .Select(parameter => GetInputValue<Tuple<string, Type, object>>(nameof(parameter.Name))?.Item3)
                    .ToArray();
            }
            return parameters;
        }
        
        public override void Execute(object context, object[] parameters) {
            SelectedSerializableMethodInfo.Invoke(context, parameters);
        }
        
        public override object GetValue(NodePort port) {
            return port.fieldName == nameof(LinkedOption) ? this : null;
        }
        
    }

    public class ActionSetterNode : ActionNode {

        public SerializableMethodInfo SelectedSerializableMethodInfo;
        public List<SerializableMethodInfo> SerializableMethodInfos = new List<SerializableMethodInfo>();
        [HideInInspector] public int ChoiceIndex;

        public override void OnCreateConnection(NodePort from, NodePort to) {
            base.OnCreateConnection(from, to);
            if (to.fieldName == nameof(Data) && to.node == this) {
                Tuple<string, Type, object> reflectionData = GetInputValue<Tuple<string, Type, object>>(nameof(Data));
                SerializableMethodInfos.AddRange(reflectionData.Item2
                    .GetMethods(SerializableMemberInfo.DefaultBindingFlags)
                    .Select(info => new SerializableMethodInfo(info)));
            }
        }
        
        public override void OnRemoveConnection(NodePort port) {
            base.OnRemoveConnection(port);
            if (port.fieldName == nameof(Data) && port.node == this) {
                SerializableMethodInfos.Clear();
            }
        }
        
        public override void Execute(object context, object[] parameters) {
            throw new NotImplementedException();
        }

        public override object GetContext() {
            throw new NotImplementedException();
        }

        public override object[] GetParameters() {
            throw new NotImplementedException();
        }
        
    }
}