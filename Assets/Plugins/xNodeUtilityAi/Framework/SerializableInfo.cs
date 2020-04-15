using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Plugins.xNodeUtilityAi.Utils;

namespace Plugins.xNodeUtilityAi.Framework {
    [Serializable]
    public class SerializableInfo {

        public const BindingFlags DefaultBindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
        
        public MemberTypes MemberTypes;
        public string DeclaringTypeName;
        public string Name;
        public string TypeName;
        public string PortName;
        public int Order;
        public bool IsIteratable;
        public bool IsPrimitive;
        public List<Parameter> Parameters = new List<Parameter>();

        public Type Type => Type.GetType(TypeName);

        public SerializableInfo(FieldInfo fieldInfo) {
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
        
        public SerializableInfo(PropertyInfo propertyInfo) {
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
        
        public SerializableInfo(MethodInfo methodInfo) {
            MemberTypes = methodInfo.MemberType;
            DeclaringTypeName = methodInfo.DeclaringType?.AssemblyQualifiedName;
            Name = methodInfo.Name;
            Type returnType = methodInfo.ReturnType;
            TypeName = returnType.AssemblyQualifiedName;
            PortName = methodInfo.Name + " (" + methodInfo.MemberType + ")";
            Order = methodInfo.MetadataToken;
            foreach (ParameterInfo parameterInfo in methodInfo.GetParameters()) {
                Parameters.Add(new Parameter(parameterInfo.Name, parameterInfo.ParameterType.AssemblyQualifiedName));
            }
        }

        public object GetEditorValue() {
            return IsPrimitive ? (object) null : new ReflectionData(Type, null);
        }
        
        public object GetRuntimeValue(object context) {
            MemberInfo memberInfo = GetMemberInfo();
            if (context == null) return GetEditorValue();
            return IsPrimitive ? memberInfo.GetValue(context) : new ReflectionData(Type, memberInfo.GetValue(context));
        }

        public void SetValue(object context, object value) {
            FieldInfo fieldInfo = (FieldInfo) GetMemberInfo();
            if (fieldInfo != null) {
                fieldInfo.SetValue(context, value);
            } else {
                PropertyInfo propertyInfo = (PropertyInfo) GetMemberInfo();
                if (propertyInfo != null) {
                    propertyInfo.SetValue(context, value);
                }
            }
        }
        
        public void Invoke(object context, object[] parameters) {
            MethodInfo methodInfo = (MethodInfo) GetMemberInfo();
            methodInfo.Invoke(context, parameters);
        }
        
        private MemberInfo GetMemberInfo() {
            Type declaringType = Type.GetType(DeclaringTypeName);
            if (declaringType == null) throw new Exception("Cannot find declaring type : " + DeclaringTypeName);
            return declaringType.GetMember(Name, MemberTypes, DefaultBindingFlags).FirstOrDefault();
        }


    }
    
    [Serializable]
    public struct Parameter {

        public string Name;
        public string TypeName;

        public Parameter(string name, string typeName) {
            Name = name;
            TypeName = typeName;
        }

    }

    [Serializable]
    public struct ReflectionData {

        public Type Type;
        public object Content;

        public ReflectionData(Type type, object content) {
            Type = type;
            Content = content;
        }

    }
}