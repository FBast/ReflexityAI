using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Plugins.xNodeUtilityAi.Utils {
    public static class ReflectionUtils {

        private const BindingFlags _defaultBindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        public static IEnumerable<MemberInfo> GetMemberInfos(this Type type, BindingFlags bindingFlags = _defaultBindingFlags) {
            return type.GetFields(bindingFlags).Cast<MemberInfo>().Concat(type.GetProperties(bindingFlags));
        }

        public static SerializableMemberInfo ToSerializableMemberInfo(this MemberInfo memberInfo) {
            return new SerializableMemberInfo {
                DeclaringTypeName = memberInfo.DeclaringType?.AssemblyQualifiedName,
                FieldName = memberInfo.Name, 
                TypeName = memberInfo.FieldType().AssemblyQualifiedName
            };
        }
        
        public static Type FieldType(this MemberInfo memberInfo) {
            switch (memberInfo) {
                case FieldInfo fieldInfo:
                    return fieldInfo.FieldType;
                case PropertyInfo propertyInfo:
                    return propertyInfo.PropertyType;
                default:
                    throw new Exception("MemberInfo must be a FieldInfo or PropertyInfo, not a " + memberInfo.MemberType);
            }
        }

        public static object GetValue(this MemberInfo memberInfo, object context) {
            switch (memberInfo) {
                case FieldInfo fieldInfo:
                    return fieldInfo.GetValue(context);
                case PropertyInfo propertyInfo:
                    return propertyInfo.GetValue(context);
                default:
                    throw new Exception("MemberInfo must be a FieldInfo or PropertyInfo, not a " + memberInfo.MemberType);
            }
        }
        
    }
    
    public class ReflectionData {

        public string Name;
        public Type Type;
        public object Data;
        
        public ReflectionData() {}
        
        public ReflectionData(string name, Type type, object data) {
            Name = name;
            Type = type;
            Data = data;
        }

    }

    [Serializable]
    public class SerializableMemberInfo {

        public string DeclaringTypeName;
        public string FieldName;
        public string TypeName;

        public MemberInfo ToMemberInfo() {
            Type declaringType = Type.GetType(DeclaringTypeName);
            if (declaringType == null) throw new Exception("Cannot find declaring type : " + DeclaringTypeName);
            return declaringType.GetMember(FieldName).FirstOrDefault();
        }
        
    }
}