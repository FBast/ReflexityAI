using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        public readonly bool IsShortCache;
        
        public Type Type => Type.GetType(TypeName);
        
        private object _cachedObject;
        
        public SerializableInfo(FieldInfo fieldInfo, bool isShortCache = false) {
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
            IsShortCache = isShortCache;
        }
        
        public SerializableInfo(PropertyInfo propertyInfo, bool isShortCache = false) {
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
            IsShortCache = isShortCache;
        }
        
        public SerializableInfo(MethodInfo methodInfo, bool isShortCache = false) {
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
            IsShortCache = isShortCache;
        }

        public void ClearCache() {
            _cachedObject = null;
        }

        public object GetEditorValue() {
            return IsPrimitive ? (object) null : new ReflectionData(Type, null);
        }
        
        public object GetRuntimeValue(object context) {
            if (context == null) return GetEditorValue();
            if (_cachedObject == null) {
                MemberInfo memberInfo = GetMemberInfo();
                switch (memberInfo) {
                    case FieldInfo fieldInfo:
                        _cachedObject = fieldInfo.GetValue(context);
                        break;
                    case PropertyInfo propertyInfo:
                        _cachedObject = propertyInfo.GetValue(context);
                        break;
                    default:
                        throw new Exception("GetValue only available for FieldInfo or PropertyInfo, not " + memberInfo.MemberType);
                }
            }
            return IsPrimitive ? _cachedObject : new ReflectionData(Type, _cachedObject);
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

    public struct ReflectionData {

        public Type Type;
        public object Content;
        public bool FromIteration;
        
        public ReflectionData(Type type, object content, bool fromIteration = false) {
            Type = type;
            Content = content;
            FromIteration = fromIteration;
        }

    }
}