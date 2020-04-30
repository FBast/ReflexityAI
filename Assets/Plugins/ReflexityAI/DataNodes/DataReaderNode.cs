using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Plugins.ReflexityAI.Framework;
using UnityEngine;
using UnityEngine.Serialization;
using XNode;

namespace Plugins.ReflexityAI.DataNodes {
    [NodeWidth(300)]
    public class DataReaderNode : DataNode, IContextual, ICacheable {
        
        public Framework.ReflexityAI Context { get; set; }

        [HideInInspector] public PortDictionary PortDictionary = new PortDictionary();
        [HideInInspector] public InfoDictionary InfoDictionary = new InfoDictionary();
        
        [SerializeField, HideInInspector] private string _declaringTypeName;

        public void UpdateData<T>() {
            // Check if context type has change
            if (!string.Equals(typeof(T).AssemblyQualifiedName, _declaringTypeName)) {
                _declaringTypeName = typeof(T).AssemblyQualifiedName;
                foreach (KeyValuePair<string,SerializableInfo> pair in InfoDictionary) {
                    pair.Value.DeclaringTypeName = _declaringTypeName;
                }
            }
            // Handle fields
            FieldInfo[] fieldInfos = typeof(T).GetFields(SerializableInfo.DefaultBindingFlags);
            foreach (FieldInfo fieldInfo in fieldInfos) {
                if (PortDictionary.ContainsKey(fieldInfo.Name)) continue;
                SerializableInfo serializableFieldInfo = new SerializableInfo(fieldInfo);
                PortDictionary.Add(fieldInfo.Name, serializableFieldInfo.PortName);
                NodePort newPort = AddDynamicOutput(serializableFieldInfo.Type, ConnectionType.Multiple, TypeConstraint.None, serializableFieldInfo.PortName);
                InfoDictionary.Add(serializableFieldInfo.PortName, serializableFieldInfo);
                // Redirect port using FormerlySerializedAsAttribute
                FormerlySerializedAsAttribute attribute = fieldInfo.GetCustomAttribute<FormerlySerializedAsAttribute>();
                if (attribute != null && PortDictionary.ContainsKey(attribute.oldName)) {
                    GetPort(PortDictionary[attribute.oldName]).SwapConnections(newPort);
                }
            }
            // Handle properties
            PropertyInfo[] propertyInfos = typeof(T).GetProperties(SerializableInfo.DefaultBindingFlags);
            foreach (PropertyInfo propertyInfo in propertyInfos) {
                if (PortDictionary.ContainsKey(propertyInfo.Name)) continue;
                SerializableInfo serializablePropertyInfo = new SerializableInfo(propertyInfo);
                PortDictionary.Add(propertyInfo.Name, serializablePropertyInfo.PortName);
                NodePort newPort = AddDynamicOutput(serializablePropertyInfo.Type, ConnectionType.Multiple, TypeConstraint.None, serializablePropertyInfo.PortName);
                InfoDictionary.Add(serializablePropertyInfo.PortName, serializablePropertyInfo);
                // Redirect port using FormerlySerializedAsAttribute
                FormerlySerializedAsAttribute attribute = propertyInfo.GetCustomAttribute<FormerlySerializedAsAttribute>();
                if (attribute != null && PortDictionary.ContainsKey(attribute.oldName)) {
                    GetPort(PortDictionary[attribute.oldName]).SwapConnections(newPort);
                }
            }
            // Remove old ports
            for (int i = PortDictionary.Count - 1; i >= 0; i--) {
                if (fieldInfos.Any(info => info.Name == PortDictionary.ElementAt(i).Key)) continue;
                if (propertyInfos.Any(info => info.Name == PortDictionary.ElementAt(i).Key)) continue;
                RemoveDynamicPort(PortDictionary.ElementAt(i).Value);
                InfoDictionary.Remove(PortDictionary.ElementAt(i).Value);
                PortDictionary.Remove(PortDictionary.ElementAt(i).Key);
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


