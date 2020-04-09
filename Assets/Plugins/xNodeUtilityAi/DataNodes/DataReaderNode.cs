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
        
        [HideInInspector] public List<SerializableMemberInfo> SerializableMemberInfos = new List<SerializableMemberInfo>();

        protected override void Init() {
            base.Init();
            OnValidate();
        }

        private void OnValidate() {
            if (graph is AIBrainGraph brainGraph && brainGraph.ContextType != null) {
                // Handle fields
                FieldInfo[] fieldInfos = brainGraph.ContextType.Type.GetFields(SerializableMemberInfo.DefaultBindingFlags);
                foreach (FieldInfo fieldInfo in fieldInfos) {
                    if (SerializableMemberInfos.Any(info => info.Name == fieldInfo.Name)) continue;
                    SerializableFieldInfo serializableFieldInfo = new SerializableFieldInfo(fieldInfo);
                    AddDynamicOutput(serializableFieldInfo.Type, ConnectionType.Multiple, TypeConstraint.None, serializableFieldInfo.PortName);
                    SerializableMemberInfos.Add(serializableFieldInfo);
                }
                // Handle properties
                PropertyInfo[] propertyInfos = brainGraph.ContextType.Type.GetProperties(SerializableMemberInfo.DefaultBindingFlags);
                foreach (PropertyInfo propertyInfo in propertyInfos) {
                    if (SerializableMemberInfos.Any(info => info.Name == propertyInfo.Name)) continue;
                    SerializablePropertyInfo serializableFieldOrProperty = new SerializablePropertyInfo(propertyInfo);
                    AddDynamicOutput(serializableFieldOrProperty.Type, ConnectionType.Multiple, TypeConstraint.None, serializableFieldOrProperty.PortName);
                    SerializableMemberInfos.Add(serializableFieldOrProperty);
                }
                // Remove old ports
                for (int i = SerializableMemberInfos.Count - 1; i >= 0; i--) {
                    if (fieldInfos.Any(info => info.Name == SerializableMemberInfos[i].Name)) continue;
                    if (propertyInfos.Any(info => info.Name == SerializableMemberInfos[i].Name)) continue;
                    RemoveDynamicPort(SerializableMemberInfos[i].PortName);
                    SerializableMemberInfos.RemoveAt(i);
                }
            } else {
                throw new Exception("No brain graph context type found, please select one");
            }
        }

        public override object GetValue(NodePort port) {
            SerializableMemberInfo serializableMemberInfo = SerializableMemberInfos.FirstOrDefault(info => info.PortName == port.fieldName);
            if (serializableMemberInfo != null) {
                return Application.isPlaying ? serializableMemberInfo.GetRuntimeValue(Context) : serializableMemberInfo.GetEditorValue();
            }
            throw new Exception("No reflected data found for " + port.fieldName);
        }
        
    }

}


