using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;
using XNode;

namespace Plugins.xNodeUtilityAi.DataNodes {
    [NodeWidth(300)]
    public class DataReaderNode : DataNode, IContextual, ICacheable {

        public ReflexityAI Context { get; set; }

        [SerializeField, HideInInspector] public SerializableInfoDictionary InfoDictionary = new SerializableInfoDictionary();
        [SerializeField, HideInInspector] private string _contextTypeName;

        public void UpdateData<T>() {
            // Check if context type has change
            if (!string.Equals(typeof(T).AssemblyQualifiedName, _contextTypeName)) {
                _contextTypeName = typeof(T).AssemblyQualifiedName;
                InfoDictionary.Clear();
                ClearDynamicPorts();
            }
            // Handle fields
            FieldInfo[] fieldInfos = typeof(T).GetFields(SerializableInfo.DefaultBindingFlags);
            foreach (FieldInfo fieldInfo in fieldInfos) {
                if (InfoDictionary.Any(pair => pair.Value.Name == fieldInfo.Name)) continue;
                SerializableInfo serializableFieldInfo = new SerializableInfo(fieldInfo);
                AddDynamicOutput(serializableFieldInfo.Type, ConnectionType.Multiple, TypeConstraint.None, serializableFieldInfo.PortName);
                InfoDictionary.Add(serializableFieldInfo.PortName, serializableFieldInfo);
            }
            // Handle properties
            PropertyInfo[] propertyInfos = typeof(T).GetProperties(SerializableInfo.DefaultBindingFlags);
            foreach (PropertyInfo propertyInfo in propertyInfos) {
                if (InfoDictionary.Any(pair => pair.Value.Name == propertyInfo.Name)) continue;
                SerializableInfo serializablePropertyInfo = new SerializableInfo(propertyInfo);
                AddDynamicOutput(serializablePropertyInfo.Type, ConnectionType.Multiple, TypeConstraint.None, serializablePropertyInfo.PortName);
                InfoDictionary.Add(serializablePropertyInfo.PortName, serializablePropertyInfo);
            }
            // Remove old ports
            for (int i = InfoDictionary.Count - 1; i >= 0; i--) {
                if (fieldInfos.Any(info => info.Name == InfoDictionary.ElementAt(i).Value.Name)) continue;
                if (propertyInfos.Any(info => info.Name == InfoDictionary.ElementAt(i).Value.Name)) continue;
                RemoveDynamicPort(InfoDictionary.ElementAt(i).Key);
                InfoDictionary.Remove(InfoDictionary.ElementAt(i).Key);
            }
        }
        
        public override object GetValue(NodePort port) {
            SerializableInfo serializableFieldInfo;
            InfoDictionary.TryGetValue(port.fieldName, out serializableFieldInfo);
            if (serializableFieldInfo != null) {
                return Application.isPlaying ? serializableFieldInfo.GetRuntimeValue(Context) : serializableFieldInfo.GetEditorValue();
            }
            throw new Exception("No reflected data found for " + port.fieldName);
        }

        public void ClearCache() {
            foreach (KeyValuePair<string,SerializableInfo> valuePair in InfoDictionary) {
                valuePair.Value.ClearCache();
            }
        }

        public void ClearShortCache() { }
        
    }
}


