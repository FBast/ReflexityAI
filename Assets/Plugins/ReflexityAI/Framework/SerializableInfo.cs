using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Plugins.ReflexityAI.Framework {
    [Serializable]
    public class SerializableInfo {

        public const BindingFlags DefaultBindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
        
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
        
        private Type _cachedType;
        public Type Type {
            get {
                if (_cachedType == null && TypeName != null) 
                    _cachedType = Type.GetType(TypeName);
                return _cachedType;
            }
        }

        private MemberInfo _cachedMemberInfo;
        public MemberInfo MemberInfo {
            get {
                if (_cachedMemberInfo == null) {
                    Type declaringType = Type.GetType(DeclaringTypeName);
                    if (declaringType == null) throw new Exception("Cannot find declaring type : " + DeclaringTypeName);
                    _cachedMemberInfo = declaringType.GetMember(Name, MemberTypes, DefaultBindingFlags).FirstOrDefault();
                }
                return _cachedMemberInfo;
            }
        }
        
        [CanBeNull] private object _cachedValue;
        
        public SerializableInfo(FieldInfo fieldInfo, bool isShortCache = false) {
            MemberTypes = fieldInfo.MemberType;
            DeclaringTypeName = fieldInfo.DeclaringType?.AssemblyQualifiedName;
            Name = fieldInfo.Name;
            Type fieldType = fieldInfo.FieldType;
            TypeName = fieldType.AssemblyQualifiedName;
            PortName = fieldInfo.Name + " (" + fieldInfo.MemberType + ")";
            Order = fieldInfo.MetadataToken;
            IsPrimitive = fieldType.IsPrimitive;
            IsIteratable = typeof(IEnumerable).IsAssignableFrom(fieldType);
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
            IsIteratable = typeof(IEnumerable).IsAssignableFrom(propertyType);
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
            _cachedValue = null;
        }

        public object GetEditorValue() {
            return IsPrimitive ? (object) null : new ReflectionData(Type, null);
        }
        
        /// <summary>
        /// Return a primitive or a reflection data including a value field as data
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public object GetRuntimeValue(object context) {
            if (context == null) return GetEditorValue();
            if (_cachedValue == null || Type.IsValueType) _cachedValue = GetValue(context);
            return IsPrimitive ? _cachedValue : new ReflectionData(Type, _cachedValue);
        }

        public void SetValue(object context, object value) {
            switch (MemberInfo) {
                case FieldInfo fieldInfo:
                    fieldInfo.SetValue(context, value);
                    break;
                case PropertyInfo propertyInfo:
                    propertyInfo.SetValue(context, value);
                    break;
                default:
                    throw new Exception("SetValue only available for fieldInfo or propertyInfo, not " + MemberInfo.MemberType);
            }
        }
        
        public void Invoke(object context, object[] parameters) {
            switch (MemberInfo) {
                case MethodInfo methodInfo:
                    methodInfo.Invoke(context, parameters);
                    break;
                default:
                    throw new Exception("Invoke only available for methodInfo");
            }
        }
        
        private object GetValue(object context) {
            switch (MemberInfo) {
                case FieldInfo fieldInfo:
                    return fieldInfo.GetValue(context);
                case PropertyInfo propertyInfo:
                    return propertyInfo.GetValue(context);
                default:
                    throw new Exception("GetValue only available for FieldInfo or PropertyInfo, not " + MemberInfo.MemberType);
            }
        }

    }
}