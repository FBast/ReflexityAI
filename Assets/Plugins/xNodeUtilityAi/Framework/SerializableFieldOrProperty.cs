using System;
using System.Collections.Generic;
using System.Reflection;
using Plugins.xNodeUtilityAi.Utils;

namespace Plugins.xNodeUtilityAi.Framework {
    [Serializable]
    public class SerializableFieldOrProperty : SerializableMemberInfo {
        
        public bool IsPrimitive;
        public bool IsIteratable;

        public SerializableFieldOrProperty(FieldInfo fieldInfo) {
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

        public SerializableFieldOrProperty(PropertyInfo propertyInfo) {
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
        
        /// <summary>
        /// Return a limited value with name and type
        /// </summary>
        /// <returns></returns>
        public object GetEditorValue() {
            return IsPrimitive ? null : 
                new Tuple<string, Type, object>(Name, Type, null);
        }

        /// <summary>
        /// Return a complete value with name, type and object value
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public object GetRuntimeValue(object context) {
            MemberInfo memberInfo = GetMemberInfo();
            if (context == null) throw new Exception("Cannot get Runtime value if context is null");
            return IsPrimitive ? memberInfo.GetValue(context) : 
                new Tuple<string, Type, object>(Name, Type, memberInfo.GetValue(context));
        }

    }
}