using System.Collections.Generic;
using Plugins.xNodeUtilityAi.Framework;
using Plugins.xNodeUtilityAi.Utils;
using UnityEngine;
using XNode;
using Object = UnityEngine.Object;

namespace Plugins.xNodeUtilityAi.MainNodes {
    public class DataIteratorNode : DataNode {

        [Input(ShowBackingValue.Never, ConnectionType.Override)] public List<Object> DataList;
        [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] public DataIteratorNode LinkedOption;
        // [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] public Object Data;

        public ReflectionData IteratedReflectionData;
        public ReflectionData CollectionReflectionData;
        public int CollectionCount => ((List<object>) IteratedReflectionData.Data)?.Count ?? 0;
        public int Index { get; set; }

        public override void OnCreateConnection(NodePort from, NodePort to) {
            base.OnCreateConnection(from, to);
            if (to.fieldName == nameof(DataList) && to.node == this) {
                ReflectionData reflectionData = GetInputValue<ReflectionData>(nameof(DataList));
                if (reflectionData.Type.IsGenericType && reflectionData.Type.GetGenericTypeDefinition() == typeof(List<>)) {
                    IteratedReflectionData = new ReflectionData {Type = reflectionData.Type.GetGenericArguments()[0]};
                    IteratedReflectionData.Name = IteratedReflectionData.Type.Name;
                    AddDynamicOutput(IteratedReflectionData.Type, ConnectionType.Multiple, 
                        TypeConstraint.Inherited, IteratedReflectionData.Name);
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
            if (port.fieldName == IteratedReflectionData.Name) {
                return Application.isPlaying ? GetFullValue(port.fieldName) : GetReflectedValue(port.fieldName);
            }
            return null;
        }

        public override object GetReflectedValue(string portName) {
            return IteratedReflectionData;
        }

        public override object GetFullValue(string portName) {
            CollectionReflectionData = GetInputValue<ReflectionData>(nameof(DataList));
            List<object> collection = (List<object>) CollectionReflectionData.Data;
            return new ReflectionData(CollectionReflectionData.Name, IteratedReflectionData.Type, collection[Index]);
        }
        
    }
}