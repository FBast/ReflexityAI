using System;
using System.Collections.Generic;
using System.Reflection;
using Plugins.xNodeUtilityAi.Framework;
using Plugins.xNodeUtilityAi.Utils;
using XNode;
using Object = UnityEngine.Object;

namespace Plugins.xNodeUtilityAi.MainNodes {
    public class DataIteratorNode : DataNode {

        [Input(ShowBackingValue.Never, ConnectionType.Override)] public List<Object> DataList;
        [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] public DataIteratorNode LinkedOption;
        [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] public Object Data;
        
        public int Index { get; set; }

        public override object GetValue(NodePort port) {
            if (port.fieldName == nameof(LinkedOption)) {
                return this;
            }
            if (port.fieldName == nameof(Data)) {
                // Récupération d'un tuple avec pour member info un type List<object> et object un context
                ReflectionData reflectionData = GetInputValue<ReflectionData>(nameof(DataList));
                if(reflectionData.Type.IsGenericType && reflectionData.Type.GetGenericTypeDefinition() == typeof(List<>)) {
                    Type genericType = reflectionData.Type.GetGenericArguments()[0];
                    object data = null;
                    if (reflectionData.Data != null) {
                        List<object> collection = (List<object>) reflectionData.Data;
                        if (collection.Count > Index) data = collection[Index];
                    }
                    return new ReflectionData(reflectionData.Name, genericType, data);
                }
                throw new Exception("Data Iterator cannot use other type than List<>");
            }
            return null;
        }
        
    }
}