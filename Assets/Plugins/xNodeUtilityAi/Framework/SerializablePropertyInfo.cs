using System;
using System.Collections.Generic;
using System.Reflection;

namespace Plugins.xNodeUtilityAi.Framework {
    [Serializable]
    public class SerializablePropertyInfo : SerializableMemberInfo {
        
        public SerializablePropertyInfo(PropertyInfo propertyInfo) {
            MemberTypes = propertyInfo.MemberType;
            DeclaringTypeName = propertyInfo.DeclaringType?.AssemblyQualifiedName;
            Name = propertyInfo.Name;
            Type propertyType = propertyInfo.PropertyType;
            TypeName = propertyType.AssemblyQualifiedName;
            PortName = propertyInfo.Name + " (" + propertyInfo.MemberType + ")";
            Order = propertyInfo.MetadataToken;
            IsPrimitive = propertyType.IsPrimitive;
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>)) {
                IsIteratable = true;
            }
        }

    }
}