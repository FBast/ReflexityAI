using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;
using XNode;
using Object = UnityEngine.Object;

namespace Plugins.xNodeUtilityAi.ActionNodes {
    public class ActionSetterNode : ActionNode {

        [Input] public Object Value;
        [HideInInspector] public SerializableInfo SelectedSerializableInfo;
        [HideInInspector] public List<SerializableInfo> SerializableInfos = new List<SerializableInfo>();
        [HideInInspector] public int ChoiceIndex;

        public override void OnCreateConnection(NodePort from, NodePort to) {
            base.OnCreateConnection(from, to);
            if (to.fieldName == nameof(Data) && to.node == this) {
                Tuple<string, Type, object> reflectionData = GetInputValue<Tuple<string, Type, object>>(nameof(Data));
                SerializableInfos.AddRange(reflectionData.Item2
                    .GetFields(SerializableInfo.DefaultBindingFlags)
                    .Select(info => new SerializableInfo(info)));
                SerializableInfos.AddRange(reflectionData.Item2
                    .GetProperties(SerializableInfo.DefaultBindingFlags)
                    .Select(info => new SerializableInfo(info)));
            }
        }
        
        public override void OnRemoveConnection(NodePort port) {
            base.OnRemoveConnection(port);
            if (port.fieldName == nameof(Data) && port.node == this) {
                SerializableInfos.Clear();
            }
        }
        
        public override void Execute(object context, object[] parameters) {
//            SelectedSerializableInfo.SetValue();
        }

        public override object GetContext() {
            return GetInputValue<Tuple<string, Type, object>>(nameof(Data)).Item3;
        }

        public override object[] GetParameters() {
            throw new NotImplementedException();
        }
        
    }
}