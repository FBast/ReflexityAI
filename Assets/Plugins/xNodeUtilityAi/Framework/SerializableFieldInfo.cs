using System;
using System.Collections.Generic;
using System.Reflection;
using Plugins.xNodeUtilityAi.Utils;

namespace Plugins.xNodeUtilityAi.Framework {
    [Serializable]
    public class SerializableFieldInfo : SerializableMemberInfo {
        
        public SerializableFieldInfo(FieldInfo fieldInfo) {
            MemberTypes = fieldInfo.MemberType;
            DeclaringTypeName = fieldInfo.DeclaringType?.AssemblyQualifiedName;
            Name = fieldInfo.Name;
            Type fieldType = fieldInfo.FieldType;
            TypeName = fieldType.AssemblyQualifiedName;
            PortName = fieldInfo.Name + " (" + fieldInfo.MemberType + ")";
            Order = fieldInfo.MetadataToken;
            IsPrimitive = fieldType.IsPrimitive;
            if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>)) {
                IsIteratable = true;
            }
        }
        
    }
}