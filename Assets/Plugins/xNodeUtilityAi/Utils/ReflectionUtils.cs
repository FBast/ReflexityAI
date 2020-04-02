using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Plugins.xNodeUtilityAi.Utils {
    public static class ReflectionUtils {

        private const BindingFlags _defaultBindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        public static IEnumerable<MemberInfo> GetMemberInfos(this Type type, BindingFlags bindingFlags = _defaultBindingFlags) {
            return type.GetFields(bindingFlags).Cast<MemberInfo>()
                .Concat(type.GetProperties(bindingFlags));
            // List<ReflectionData> reflectionDatas = new List<ReflectionData>();
            // foreach (MemberInfo memberInfo in memberInfos) {
            //     object data = null;
            //     if (context != null)
            //         data = memberInfo.GetValue(context);
            //     reflectionDatas.Add(new ReflectionData(memberInfo.Name, memberInfo.FieldType(), data));
            // }
            // return reflectionDatas;
        }

        public static SerializableMemberInfo ToSerializableMemberInfo(this MemberInfo memberInfo) {
            return new SerializableMemberInfo {
                FieldName = memberInfo.Name, 
                TypeName = memberInfo.FieldType().AssemblyQualifiedName
            };
        }

        public static MemberInfo ToMemberInfo(this SerializableMemberInfo serializableMemberInfo) {
            Type type = Type.GetType(serializableMemberInfo.TypeName);
            if (type == null)
                throw new Exception("Cannot find type : " + serializableMemberInfo.TypeName);
            return type.GetMember(serializableMemberInfo.FieldName).FirstOrDefault();
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

        public string FieldName;
        public string TypeName;

        public SerializableMemberInfo() {}

        public SerializableMemberInfo(string fieldName, string typeName) {
            FieldName = fieldName;
            TypeName = typeName;
        }

    }
}