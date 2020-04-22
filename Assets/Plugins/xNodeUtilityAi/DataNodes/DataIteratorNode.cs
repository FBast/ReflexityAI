using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;
using XNode;
using Object = UnityEngine.Object;

namespace Plugins.xNodeUtilityAi.DataNodes {
    public class DataIteratorNode : DataNode {

        [Input(ShowBackingValue.Never, ConnectionType.Override)] public List<Object> List;
        [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] public DataIteratorNode LinkedOption;

        public int Index { get; set; }
        
        [SerializeField, HideInInspector] private string _typeAssemblyName;
        
        public override void OnCreateConnection(NodePort from, NodePort to) {
            base.OnCreateConnection(from, to);
            if (to.fieldName == nameof(List) && to.node == this) {
                ReflectionData reflectionData = GetInputValue<ReflectionData>(nameof(List));
                if (reflectionData.Type.IsGenericType && reflectionData.Type.GetGenericTypeDefinition() == typeof(List<>)) {
                    Type type = reflectionData.Type.GetGenericArguments()[0];
                    _typeAssemblyName = type.AssemblyQualifiedName;
                    AddDynamicOutput(type, ConnectionType.Multiple, TypeConstraint.Inherited, type.Name);
                } else {
                    Debug.LogError("DataList can only take List");
                }
            }
        }
        
        public override void OnRemoveConnection(NodePort port) {
            base.OnRemoveConnection(port);
            if (port.fieldName == nameof(List) && port.node == this) {
                ClearDynamicPorts();
            }
        }

        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(LinkedOption)) {
                return this;
            } else {
                Type type = Type.GetType(_typeAssemblyName);
                if (!Application.isPlaying) 
                    return new ReflectionData(type, null);
                return new ReflectionData(type, GetCollection().ElementAt(Index));
            }
        }
        
        public IEnumerable<object> GetCollection() {
            ReflectionData reflectionData = GetInputValue<ReflectionData>(nameof(List));
            return (IEnumerable<object>) reflectionData.Content;
        }

    }
}