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
        
        [HideInInspector] public List<SerializableFieldOrProperty> SerializableDatas = new List<SerializableFieldOrProperty>();

        protected override void Init() {
            base.Init();
            OnValidate();
        }

        private void OnValidate() {
            if (graph is AIBrainGraph brainGraph && brainGraph.ContextType != null) {
                // Handle fields
                FieldInfo[] fieldInfos = brainGraph.ContextType.Type.GetFields(SerializableMemberInfo.DefaultBindingFlags);
                foreach (FieldInfo fieldInfo in fieldInfos) {
                    if (SerializableDatas.Any(info => info.Name == fieldInfo.Name)) continue;
                    SerializableFieldOrProperty serializableFieldOrProperty = new SerializableFieldOrProperty(fieldInfo);
                    AddDynamicOutput(serializableFieldOrProperty.Type, ConnectionType.Multiple, TypeConstraint.None, serializableFieldOrProperty.PortName);
                    SerializableDatas.Add(serializableFieldOrProperty);
                }
                // Handle properties
                PropertyInfo[] propertyInfos = brainGraph.ContextType.Type.GetProperties(SerializableMemberInfo.DefaultBindingFlags);
                foreach (PropertyInfo propertyInfo in propertyInfos) {
                    if (SerializableDatas.Any(info => info.Name == propertyInfo.Name)) continue;
                    SerializableFieldOrProperty serializableFieldOrProperty = new SerializableFieldOrProperty(propertyInfo);
                    AddDynamicOutput(serializableFieldOrProperty.Type, ConnectionType.Multiple, TypeConstraint.None, serializableFieldOrProperty.PortName);
                    SerializableDatas.Add(serializableFieldOrProperty);
                }
                // Remove old ports
                for (int i = SerializableDatas.Count - 1; i >= 0; i--) {
                    if (fieldInfos.Any(info => info.Name == SerializableDatas[i].Name)) continue;
                    if (propertyInfos.Any(info => info.Name == SerializableDatas[i].Name)) continue;
                    RemoveDynamicPort(SerializableDatas[i].PortName);
                    SerializableDatas.RemoveAt(i);
                }
            } else {
                throw new Exception("No brain graph context type found, please select one");
            }
        }

        public override object GetValue(NodePort port) {
            SerializableFieldOrProperty serializableFieldOrProperty = SerializableDatas.FirstOrDefault(info => info.PortName == port.fieldName);
            if (serializableFieldOrProperty != null) {
                return Application.isPlaying ? serializableFieldOrProperty.GetRuntimeValue(Context) : serializableFieldOrProperty.GetEditorValue();
            }
            throw new Exception("No reflected data found for " + port.fieldName);
        }
        
    }

}


