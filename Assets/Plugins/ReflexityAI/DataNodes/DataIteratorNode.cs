using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.ReflexityAI.Framework;
using UnityEngine;
using XNode;
using Object = UnityEngine.Object;

namespace Plugins.ReflexityAI.DataNodes {
    [CreateNodeMenu("Reflexity/Data/Iterator")]
    public class DataIteratorNode : DataNode, ICacheable {

        [Input(ShowBackingValue.Never, ConnectionType.Override)] public List<Object> Enumerable;
        [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] public DataIteratorNode LinkedOption;

        public int Index { get; set; }

        public Type _argumentType;
        public Type ArgumentType {
            get {
                if (_argumentType == null)
                    _argumentType = Type.GetType(_typeArgumentName);
                return _argumentType;
            }
        }

        public object _cachedCurrentValue;
        public object CurrentValue {
            get {
                if (_cachedCurrentValue == null) {
                    _cachedCurrentValue = GetCollection().ElementAt(Index);
                }
                return _cachedCurrentValue;
            }
        }
        
        public int CollectionCount => GetCollection().Count();

        [SerializeField, HideInInspector] private string _typeArgumentName;
        
        public override void OnCreateConnection(NodePort from, NodePort to) {
            if (to.fieldName == nameof(Enumerable) && to.node == this) {
                ClearDynamicPorts();
                ReflectionData reflectionData = GetInputValue<ReflectionData>(nameof(Enumerable));
                if (reflectionData.Type.IsGenericType && reflectionData.Type.GetGenericTypeDefinition().GetInterface(typeof(IEnumerable<>).FullName) != null) {
                    Type type = reflectionData.Type.GetGenericArguments()[0];
                    _typeArgumentName = type.AssemblyQualifiedName;
                    AddDynamicOutput(type, ConnectionType.Multiple, TypeConstraint.Inherited, type.Name);
                } else {
                    Debug.LogError("Enumerable need to be a generic type (List or Array for example)");
                }
            }
        }
        
        public override void OnRemoveConnection(NodePort port) {
            if (port.fieldName == nameof(Enumerable) && port.node == this) {
//                ClearDynamicPorts();
            }
        }

        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(LinkedOption)) {
                return this;
            } 
            else {
                if (!Application.isPlaying) 
                    return new ReflectionData(ArgumentType, null, true);
                return new ReflectionData(ArgumentType, CurrentValue, true);
            }
        }
        
        private IEnumerable<object> GetCollection() {
            ReflectionData reflectionData = GetInputValue<ReflectionData>(nameof(Enumerable));
            return (IEnumerable<object>) reflectionData.Value;
        }

        public void ClearCache() {
            _cachedCurrentValue = null;
        }

        public void ClearShortCache() {
            _cachedCurrentValue = null;
        }
        
    }
}