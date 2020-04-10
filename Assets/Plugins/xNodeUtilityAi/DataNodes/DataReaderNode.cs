using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Plugins.xNodeUtilityAi.Framework;
using UnityEngine;
using XNode;

namespace Plugins.xNodeUtilityAi.DataNodes {
    [NodeWidth(300)]
    public class DataReaderNode : DataNode, IContextual {

        public AbstractAIComponent Context { get; set; }

        [HideInInspector] public List<SerializableInfo> SerializableInfos = new List<SerializableInfo>();

        protected override void Init() {
            base.Init();
            OnValidate();
        }

        private void OnValidate() {
            if (graph is AIBrainGraph brainGraph && brainGraph.ContextType != null) {
                // Handle fields
                FieldInfo[] fieldInfos = brainGraph.ContextType.Type.GetFields(SerializableInfo.DefaultBindingFlags);
                foreach (FieldInfo fieldInfo in fieldInfos) {
                    if (SerializableInfos.Any(info => info.Name == fieldInfo.Name)) continue;
                    SerializableInfo serializableFieldInfo = new SerializableInfo(fieldInfo);
                    AddDynamicOutput(serializableFieldInfo.Type, ConnectionType.Multiple, TypeConstraint.None, serializableFieldInfo.PortName);
                    SerializableInfos.Add(serializableFieldInfo);
                }
                // Handle properties
                PropertyInfo[] propertyInfos = brainGraph.ContextType.Type.GetProperties(SerializableInfo.DefaultBindingFlags);
                foreach (PropertyInfo propertyInfo in propertyInfos) {
                    if (SerializableInfos.Any(info => info.Name == propertyInfo.Name)) continue;
                    SerializableInfo serializablePropertyInfo = new SerializableInfo(propertyInfo);
                    AddDynamicOutput(serializablePropertyInfo.Type, ConnectionType.Multiple, TypeConstraint.None, serializablePropertyInfo.PortName);
                    SerializableInfos.Add(serializablePropertyInfo);
                }
                // Remove old ports
                for (int i = SerializableInfos.Count - 1; i >= 0; i--) {
                    if (fieldInfos.Any(info => info.Name == SerializableInfos[i].Name)) continue;
                    if (propertyInfos.Any(info => info.Name == SerializableInfos[i].Name)) continue;
                    RemoveDynamicPort(SerializableInfos[i].PortName);
                    SerializableInfos.RemoveAt(i);
                }
            } else {
                throw new Exception("No brain graph context type found, please select one");
            }
        }

        public override object GetValue(NodePort port) {
            SerializableInfo serializableFieldInfo = SerializableInfos.FirstOrDefault(info => info.PortName == port.fieldName);
            if (serializableFieldInfo != null) {
                return Application.isPlaying ? serializableFieldInfo.GetRuntimeValue(Context) : serializableFieldInfo.GetEditorValue();
            }
            throw new Exception("No reflected data found for " + port.fieldName);
        }
        
    }

}


