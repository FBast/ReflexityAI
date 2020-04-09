using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;
using XNode;
using Object = UnityEngine.Object;

namespace Plugins.xNodeUtilityAi.DataNodes {
    public class DataIteratorNode : DataNode {

        [Input(ShowBackingValue.Never, ConnectionType.Override)] public List<Object> DataList;
        [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] public DataIteratorNode LinkedOption;

        public int Index { get; set; }
        
        private string _typeAssemblyName;
        
        public override void OnCreateConnection(NodePort from, NodePort to) {
            base.OnCreateConnection(from, to);
            if (to.fieldName == nameof(DataList) && to.node == this) {
                Tuple<string, Type, object> inputValue = GetInputValue<Tuple<string, Type, object>>(nameof(DataList));
                if (inputValue.Item2.IsGenericType && inputValue.Item2.GetGenericTypeDefinition() == typeof(List<>)) {
                    Type type = inputValue.Item2.GetGenericArguments()[0];
                    _typeAssemblyName = type.AssemblyQualifiedName;
                    AddDynamicOutput(type, ConnectionType.Multiple, TypeConstraint.Inherited, type.Name);
                } else {
                    Debug.LogError("DataList can only take List");
                }
            }
        }
        
        public override void OnRemoveConnection(NodePort port) {
            base.OnRemoveConnection(port);
            if (port.fieldName == nameof(DataList) && port.node == this) {
                ClearDynamicPorts();
            }
        }

        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(LinkedOption)) {
                return this;
            } else {
                Tuple<string, Type, object> inputValue = GetInputValue<Tuple<string, Type, object>>(nameof(DataList));
                Type type = Type.GetType(_typeAssemblyName);
                if (!Application.isPlaying) 
                    return new Tuple<string, Type, object>(port.fieldName, type, null);
                List<object> collection = ((IEnumerable) inputValue.Item3).Cast<object>().ToList();
                return new Tuple<string, Type, object>(port.fieldName, type, collection[Index]);
            }
        }

        public int GetCollectionCount() {
            Tuple<string, Type, object> inputValue = GetInputValue<Tuple<string, Type, object>>(nameof(DataList));
            return ((IEnumerable) inputValue.Item3).Cast<object>().Count();
        }
        
    }
}