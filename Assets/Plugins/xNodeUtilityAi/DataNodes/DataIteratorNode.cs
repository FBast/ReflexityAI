using System;
using System.Collections.Generic;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;
using XNode;
using Object = UnityEngine.Object;

namespace Plugins.xNodeUtilityAi.DataNodes {
    public class DataIteratorNode : DataNode {

        [Input(ShowBackingValue.Never, ConnectionType.Override)] public List<Object> DataList;
        [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] public DataIteratorNode LinkedOption;

        public Tuple<string, Type, List<object>> IteratedReflectionData;
        public Tuple<string, Type, object> CollectionReflectionData;
        public int CollectionCount => IteratedReflectionData.Item3?.Count ?? 0;
        public int Index { get; set; }
        
        public override void OnCreateConnection(NodePort from, NodePort to) {
            base.OnCreateConnection(from, to);
            if (to.fieldName == nameof(DataList) && to.node == this) {
                Tuple<string, Type, object> reflectionData = GetInputValue<Tuple<string, Type, object>>(nameof(DataList));
                if (reflectionData.Item2.IsGenericType && reflectionData.Item2.GetGenericTypeDefinition() == typeof(List<>)) {
                    Type type = reflectionData.Item2.GetGenericArguments()[0];
                    IteratedReflectionData = new Tuple<string, Type, List<object>>(type.Name, type, null);
                    AddDynamicOutput(IteratedReflectionData.Item2, ConnectionType.Multiple, 
                        TypeConstraint.Inherited, IteratedReflectionData.Item1);
                } else {
                    Debug.LogError("DataList can only take List");
                }
            }
        }
        
        public override void OnRemoveConnection(NodePort port) {
            base.OnRemoveConnection(port);
            if (port.fieldName == nameof(DataList) && port.node == this) {
                IteratedReflectionData = null;
                ClearDynamicPorts();
            }
        }
        
        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(LinkedOption)) {
                return this;
            }
            if (port.fieldName == IteratedReflectionData.Item1) {
                if (!Application.isPlaying) return IteratedReflectionData;
                CollectionReflectionData = GetInputValue<Tuple<string, Type, object>>(nameof(DataList));
                List<object> collection = (List<object>) CollectionReflectionData.Item3;
                return new Tuple<string, Type, object>(CollectionReflectionData.Item1, IteratedReflectionData.Item2, collection[Index]);
            }
            return null;
        }

    }
}